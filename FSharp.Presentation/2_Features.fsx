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


// Inner functions
let evens (list: list<int>) =
    let isEven x = (x % 2) = 0
    List.filter isEven list // Last line of a multi-line function is the return value

// Same as above but with a anonomous lambda function
// We dont need to explicitly pass through the list parameter
let evensUsingLambda = List.filter (fun i -> i % 2 = 0)



(* 2.2 Collections
    - F# native collections (linked list, set, map, array) as well as any other .NET collection type.
*)
// List constructed as head and a tail
let twoAndThree = [2; 3;]
let oneToThree = 1 :: twoAndThree
let fourToSeven = [4..7]
let oneToSeven = oneToThree @ fourToSeven



// Maps can be constructed from lists of (key, value) tuples.
let map = [(1, "one"); (2, "two"); (3, "three")] |> Map.ofList


// Arrays can be constructed in many ways
// Building an array of primes less than 100:
let array = [|4..7|]
let isPrime n = [|2..(n-1)|] |> Seq.forall (fun i -> n % i <> 0)
let primesLessThenOneHundred = 
    [| for i in 0..100 do
        if isPrime i then
            yield i|]

primesLessThenOneHundred.[0] <- 123  // Can be mutated



(* 2.3 Types
    - Two main kinds of Types
        Algabreic Data Types (Unions, Records, Tuples)
        Classes
*)
// Unions are types that can be one-of-many options.
type JobTitle = 
    | Junior
    | Mid
    | Senior

// Each option can be of any type
type ParseIntResult = 
    | Ok of int
    | Error of string
    | Other of ParseIntResult    

let success = Ok 1
let failure = Error "Incorrect format"

// Records are structs with immutable values
// Will auto-create methos for GetHashCode(), ToString(), etc.
open System
type Employee = {
    Id: Guid; 
    Name: string;
    DoB: DateTime;
    JobTitle: JobTitle; 
    NiNumber: string }

let bob = { 
    Id = Guid.NewGuid(); 
    Name = "bob"; 
    DoB = DateTime.Parse("01/01/1990"); 
    JobTitle = Junior; 
    NiNumber = "" }

let bobAfterPromotion = { bob with JobTitle = Mid } // copy and update

// Combining records and unions together
type GetEmployeeResult = 
    | Success of Employee
    | Error of string

let result: GetEmployeeResult = ///

match result with
| Success e -> "Employee name is " + e.Name
| Error e -> "Error was" + e
// Classes
type EmployeeApiClient(uri: string) = 
    member this.GetEmployee(id: Guid): GetEmployeeResult = Success bob  // Fetch employee
    interface IDisposable with
        member this.Dispose() = ()                  // Dispose of api resources

// Tuples
type IntAndString = int * string
let intAndStringValue = (1, "two")

// Type keyword can also create a type alias
type EmployeeSerializer = Employee -> string
let employeeToString: EmployeeSerializer = 
    (fun e -> sprintf "Employee Name: %s" e.Name)



(* 2.4 Pattern Matching
    - Conditional branching based on any number of abstract patterns
*)
let rec lengthOfList l = 
    match l with
    | [] -> 0                               // Matches an empty list
    | head :: tail -> 1 + lengthOfList tail // Binds first element to head, rest of list to tail

// Options are a commonly used union type and look like:
// type Option<'a> = Some of 'a | None
let findAndPrintValue key =
    match map.TryFind key with
    | Some v -> printf "Found value %s" v
    | None -> printf "Key %i not found" key

// Unions are well suited for pattern matching
let getEmployeeWithLogging id = 
    use client = new EmployeeApiClient("http://employee-api.com")
    match client.GetEmployee(id) with
    | Success e -> printfn "Found employee with id %A" e.Id
    | Error msg -> printfn "Failed lookup with id %A with error %s." id msg






(* 2.5 Putting it all together
    - Effective programs can be created just from pattern matching, 
        unions and records.
*)
type Json = 
    | Value of string
    | Array of list<Json>
    | Object of Map<string, Json>


let rec serialize json = 
    match json with
    | Value v -> sprintf "\"%s\"" v
    | Array values -> 
        values 
        |> List.map serialize 
        |> String.concat "," 
        |> sprintf "[%s]"
    | Object properties -> 
        properties 
        |> Seq.map (fun kv -> sprintf "\"%s\": %s" kv.Key (serialize kv.Value))
        |> String.concat ","
        |> sprintf "{%s}"

let jsonValue: Json = 
    [
    ("Id", Value "123");
    ("Name", Value "Alice");
    ("PreviousTitles", Array [Value "Junior"; Value "Mid"]);
    ("DoB", 
        [
        ("Year", Value "1987");
        ("Month", Value "Feb");
        ("Day", Value "12")
        ]
        |> Map.ofList
        |> Object
    )] 
    |> Map.ofList 
    |> Object

serialize jsonValue |> printf "%s"
(*
    {
      "DoB": {
        "Day": "12",
        "Month": "Feb",
        "Year": "1987"
      },
      "Id": "123",
      "Name": "Alice",
      "PreviousTitles": [
        "Junior",
        "Mid"
      ]
    }
*)