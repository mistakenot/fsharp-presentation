(* 2.3 Types
    - Classes
    - Unions
    - Records
    - Tuples
    - Alises
    - Generics
*)

// Standard .NET classes
type Dog(name: string, breed: string) = 
    member this.Bark() = printfn "Woof! My name is %s and I am a %s" name breed

// Unions are types that can be one-of-many options.
type JobTitle = 
    | Junior
    | Mid
    | Senior

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

// Tuples are unlabelled, ordered collections of values.
type AnIntAndAString = (int * string)

let myTuple = (1, "two")
let (x, y) = myTuple


// The type keyword can also be used to create aliases
type EmployeeId = Guid

type EmployeeSerializer = Employee -> string

let jsonSerializer: EmployeeSerializer = 
    fun employee -> sprintf "{\"id\": \"%O\", \"name\": \"%s\", \"dob\": \"%O\"}" (employee.Id) (employee.Name) (employee.DoB)




// Using these types together

// The Option type represents a optional value
type Option<'a> = 
    | None
    | Some of 'a

type BinaryTree<'a> = 
    | Empty
    | Node of ('a * BinaryTree<'a> * BinaryTree<'a>) 
