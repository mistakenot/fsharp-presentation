namespace FSharp.Presentation

module Presentation =

    open System

    // 1 INTRODUCTION

    (* 1.1 What is F#?
        - Created by Microsoft Research ~12 years ago.
        - Open source, cross platform.
        - Syntax inspired by ML family of languages.
        - Historically used in quantitive environments; finance, research etc.
    *)

    let creator = "Don Syme @ Microsoft"

    let platforms = [".NET Framework"; ".NET Core"; "Mono"; "JavaScript"]

    let inspiredBy = ["OCaml"; "Scala"; "ML"]










    (* 1.2 Main Features?
        - Multi-paradym, but encourages a 'functional first' approach.
        - Strong, infered typing system.
        - Good IDE support:
            Visual Studio (Windows)
            Xamarin Studio (Windows / *nix)
            Iodine project (VS Code, Atom)
        - Seamless interop with any .NET binary (C#, Nuget, etc.)
    *)

    // let x = 1 + "two" // Inferred, strong types

    let mutable y = 0 // Imperative programming support
    y <- 1

    type MyObject(name: string) = // Object-orientated support
       member this.SayHello() = printf "Hi, my name is %s" name






    (* 1.3. What is functional programming?
        - Programs are built by composing together 'pure' functions into larger components.
        - Pure Functions: a function that returns a value without reassignment to existing variables.
        - Code looks more like mathmatical expressions instead of step-by-step commands.
        - Business logic is often encoded into the type system.
        - Concurrency is much simpler when you copy data instead of updating it.
    *)

    // Imperative, C style implementation
    let imperativeFactorial x = 
        let mutable y = 1
        if x > 0 then 
            for i in [1..x] do
                y <- y * i
        y // Last value of a function is the return value

    // Functional implementation
    let rec functionalFactorial x =
        if x < 1 then 1
        else x * functionalFactorial (x - 1)


    (* 1.4. Why bother?
        - Pros:
            Conciseness (Code better reflects the programmers intentions)
            Correctness (Better use of the type system)
            Productivity (Eliminates whole classes of bugs)
            Testability (Pure functions are easy to test)
        - Cons:
            Learning curve
            Performance
    *)












    // 2 BASIC LANGUAGE FEATURES

    (* 2.1 Declaring functions and variables
        - 'let' keyword is immutable binding to a function or variable
        - Arguments listed after function name
        - Functions are 'curried' and can be partially applied
        - Types are inferred, but can be stated explicitly
    *)
    // Type of float
    let pi = 3.14

    // Type of (float -> float -> float)
    let multiply x y = x * y

    // Type of (float -> float)
    let square x = multiply x x

    // Type of (float -> float)
    let multiplyByPi = multiply pi

    let areaOfCircle r = multiplyByPi (square r)

    // Or, using 'pipe' operator:
    let areaOfCircleUsingPipe r = square r |> multiplyByPi




    (* 2.x Collections
    *)




    (* 2.x Types
        - Two main kinds of Types
            Algabreic Data Types (Unions, Records, Tuples)
            Classes
    *)
    // Unions are types that can be one-of-many options.
    type JobTitle = 
        | Minion
        | Overlord

    // Records are structs with immutable values
    // Will auto-create methos for GetHashCode(), ToString(), etc.
    type Employee = {Id: Guid; Name: string; DoB: DateTime; JobTitle: JobTitle; NiNumber: string}

    // Creating a record
    let bob = { Id = Guid.NewGuid(); Name = "bob"; DoB = DateTime.Parse("01/01/1990"); JobTitle = Minion; NiNumber = "" } 
    // Updating a record.
    let promoteTo employee newTitle = {employee with JobTitle = newTitle} // copy and update

    // Combining records and unions together
    type GetEmployeeResult = 
        | Success of Employee
        | Error of string

    let getEmployee id = 
        if id = bob.Id then Success bob
        else Error "Not found."

    // Unions are well suited for pattern matching
    let getEmployeeWithLogging id = 
        match (getEmployee id) with
        | Success e -> printfn "Found employee with id %A" e.Id
        | Error msg -> printfn "Failed lookup with id %A with error %s." id msg

    // Classes are supported
    type EmployeeClass(id: Guid) = 
        new() = EmployeeClass(Guid.NewGuid())

    (* 2.2 Pattern Matching
        - Is to a 'switch' statement what a biplane is to the space shuttle
        - Conditional branching based on a number of abstract patterns
    *)
    let rec lengthOfList l = 
        match l with
        | [] -> 0 // Matches an empty list
        | head :: tail -> 1 + lengthOfList tail // Binds first element to head, rest of list to tail

    type Json = 
        | Object of Map<string, Json>
        | Array of Json list
        | Value of string

    let rec serialize json = 
        match json with
        | Object properties -> 
            properties 
            |> Seq.map (fun kv -> sprintf "{'%s': '%s'}" (kv.Key) (serialize kv.Value))
            |> String.concat "," 
            |> sprintf "{%s}"
        | Array values -> 
            values 
            |> List.map serialize 
            |> String.concat "," 
        | Value v -> "'" + v + "'"

    (* 3.x 'Railway' Orientated Programming and Monads
        - In the real world, things go wrong. 
        - How can we handle errors in more complicated real world applications?
        - As an example, we're going to write a program that parses an Employee from a 
            list of chars
    *)

    // Parsing an Employee

    type ParserResult<'a> = 
        | Success of 'a * list<char>
        | Failure

    type Parser<'a> = list<char> -> ParserResult<'a>

    type ParserAccumulator<'a> = ('a * list<char>) -> ParserResult<'a>

    // type Employee = {Id: Guid; Name: string; DoB: DateTime; JobTitle: JobTitle; NiNumber: string}

    let parseGuid: Parser<Guid> = fun c -> Success(Guid.Empty, [])// failwith "Not Implemented"
    let parseString: Parser<string> = fun c -> Success ("string", [])//failwith "Not Implemented"
    let parseDate: Parser<DateTime> = fun c -> Success(DateTime.Now, []) //failwith "Not Implemented"
    let parseJobTitle: Parser<JobTitle> = fun c -> Success(JobTitle.Minion, [] ) //failwith "Not Implemented"

    let defaultEmployee = {Id = Guid.Empty; Name = String.Empty; DoB = new DateTime(); JobTitle = JobTitle.Minion; NiNumber = String.Empty}

    //let parseGuid: ParserAccumulator<Employee> = 
    //    fun (e, chars) ->
    //        match parseGuid chars with
    //        | Success(g, rest) -> Success({e with Id = g}, rest)
    //        | Failure -> Failure

    let Lift (a: Parser<'a>) (combine: 'c -> 'a -> 'c) : ParserAccumulator<'c> = 
        (fun (c, chars) ->
            match a chars with
            | Success (result, rest) -> Success(combine c result, rest)
            | Failure -> Failure)

    let Fold (a: ParserAccumulator<'c>) (b: ParserAccumulator<'c>): ParserAccumulator<'c> =
        (fun (c, chars) ->
            match a (c, chars) with
            | Success (r, rest) -> b (r, rest)
            | Failure -> Failure)

    let parseEmployeeGuid = Lift parseGuid (fun e g -> {e with Id = g})
    let parseEmployeeName = Lift parseString (fun e n -> {e with Name = n})
    let parseEmployeeDob = Lift parseDate (fun e d -> {e with DoB = d})
    let parseEmployeeTitle = Lift parseJobTitle (fun e j -> {e with JobTitle = j})
    let parseEmployeeNi = Lift parseString (fun e n -> {e with NiNumber = n})

    let parseEmployee = Fold parseEmployeeGuid parseEmployeeName |> Fold parseEmployeeDob |> Fold parseEmployeeTitle |> Fold parseEmployeeNi

    //let parseEmployee chars : ParserResult<Employee> = 
    //    match parseGuid chars with
    //    | Success (guid, t1) ->
    //        match parseString t1 with
    //        | Success (name, t2) -> 
    //            match parseDate t2 with
    //            | Success (dob, t3) ->
    //                match parseJobTitle t3 with
    //                | Success (jobTitle, t4) -> 
    //                    match parseString t4 with
    //                    | Success (niNumber, rest) -> Success ({Id = guid; Name = name; DoB = dob; JobTitle = jobTitle; NiNumber = niNumber}, rest)
    //                    | Failure -> Failure
    //                | Failure -> Failure
    //            | Failure -> Failure
    //        | Failure -> Failure
    //    | Failure -> Failure
    //
