(* 7 Functional validation *)

open System

type Employee = {
    // Can't be 00000000-0000-0000-0000-000000000000
    Id: Guid 
    // Can't be null
    // More than 5 chars, less than 20
    Name: string
    // Must be over 18
    // Must be under 70
    DoB: DateTime
    // Can't be null
    // Exactly 8 chars
    // Hexidecimal
    NiNumber: string }


type ValidationError = {
    Msg: string
    Code: int }

type ValidationRule<'a> = 'a -> option<ValidationError>





// Object is not null
let notNull (o: Object) =
    if o = null 
    then Some ({Msg = "Value is null"; Code = 1}) 
    else None

// Strings
let minLength (i: int) (s: string) =
    if s.Length < i 
    then Some ({Msg = sprintf "Must have at least %i chars." i; Code = 2})
    else None

let maxLength (i: int) (s: string) =
    if s.Length > i
    then Some ({Msg = sprintf "Must have less than %i chars." i; Code = 3})
    else None

let isHex (s: string) =
    let validChars = ['a'..'z'] @ ['A'..'Z'] @ ['0'..'9'] |> Set.ofList
    let isHex = s |> Set.ofSeq |> Set.isSuperset validChars
    if not isHex
    then Some ({Msg = sprintf "Value %s is not hexidecimal" s; Code = 4})
    else None


// Dates
let ageGreaterThan (i: int) (d: DateTime) = 
    if (DateTime.Now - d).TotalDays < float (i * 365)
    then Some({Msg = sprintf "Must be at least %i years old." i; Code = 5})
    else None

let ageLessThan (i: int) (d: DateTime) = 
    if (DateTime.Now - d).TotalDays > float (i * 365)
    then Some({Msg = sprintf "Must be less than %i years old." i; Code = 6})
    else None

// Guids
let isValidGuid (g: Guid) = 
    if g = Guid.Empty 
    then Some({Msg = sprintf "ID %O cant be zero" g; Code = 7})
    else None






// Combines two validators together.
// The first one to fail is returned.
// Aka Bind
let (>=>) va vb value = 
    match va value with 
    | Some error -> Some error
    | None -> vb value

// Now create safer combinations.
let minLengthNotNull length = notNull >=> minLength length
let maxLengthNotNull length  = notNull >=> maxLength length
let isHexNotNull = notNull >=> isHex
let exactLength length = maxLength length >=> minLength length


// We want something like this really.
// let employeeValidator: Employee -> list<ValidationError> = ???





// Turns a validation option result into 
// a list of results 
let asList result = 
    match result with
    | Some error -> [error]
    | None -> []

// For example
let isValidGuidAsList = isValidGuid >> asList


// Combines two validators together.
// Returns all validation errors as list
// rather than just the first one
let (>@>) va vb value = 
    (va value |> asList) @ (vb value |> asList)




//type Employee = {
//    // Can't be 00000000-0000-0000-0000-000000000000
//    Id: Guid 
//    // Can't be null
//    // More than 5 chars, less than 20
//    Name: string
//    // Must be over 18
//    // Must be less than 70
//    DoB: DateTime
//    // Can't be null
//    // Exactly 8 chars
//    // Hexidecimal
//    NiNumber: string }
let idValidator = isValidGuid >> asList
let nameValidator = notNull >=> (minLength 5 >=> maxLength 20) >> asList
let dobValidator = ageGreaterThan 18 >=> ageLessThan 70 >> asList
let niValidator = notNull >=> (exactLength 8 >=> isHex) >> asList




let validate (e: Employee) = 
    idValidator e.Id 
    @ nameValidator e.Name
    @ dobValidator e.DoB 
    @ niValidator e.NiNumber

let r1 = validate {Id = Guid.Empty; Name = "Bob Bobson"; DoB = DateTime.Parse("03/04/1980"); NiNumber = "ab30cf30"}
let r2 = validate {Id = Guid.NewGuid(); Name = null; DoB = DateTime.Parse("12/12/1979"); NiNumber = null}
let r3 = validate {Id = Guid.NewGuid(); Name = "Tim Timpson"; DoB = DateTime.Parse("01/05/1992"); NiNumber = "bf50aa20"}