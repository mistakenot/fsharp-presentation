(* 2.3 Types
    - Classes
    - ADT's (Unions, Records)
    - Tuples
    - Alises
    - Generics
*)

// Standard .NET classes
type Dog(name: string, breed: string) = 
    member this.Bark() = printfn "Woof! My name is %s and I am a %s" name breed
    interface System.IDisposable with 
        member this.Dispose() = printfn "Time for the vets."




// Unions are types that can be one-of-many options.
type JobTitle = 
    | Junior
    | Mid
    | Senior

// Good for representing ops that can fail.
type JobTitleApiResult = 
    | Ok of JobTitle
    | InvalidRequest of string
    | HttpError of int

let okResult: JobTitleApiResult = Ok Junior
let httpError: JobTitleApiResult = HttpError 501










// Records are immutable structs
open System

type Employee = {
    Id: Guid; 
    Name: string;
    DoB: DateTime;
    JobTitle: JobTitle }

let bob = { 
    Id = Guid.NewGuid(); 
    Name = "bob"; 
    DoB = DateTime.Parse("01/01/1990"); 
    JobTitle = Junior }

let bobAfterPromotion = { bob with JobTitle = Mid } // copy and update











// Tuples are unlabelled, ordered collection of values.
type AnIntAndAString = (int * string)

let myTuple = (1, "two")
let (x, y) = myTuple












// The type keyword can also be used to create aliases
type EmployeeId = Guid

type EmployeeSerializer = Employee -> string

let jsonSerializer: EmployeeSerializer = 
    fun employee -> sprintf "{\"id\": \"%O\", \"name\": \"%s\", \"dob\": \"%O\"}" (employee.Id) (employee.Name) (employee.DoB)

