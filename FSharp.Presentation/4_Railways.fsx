(* 4.1 Railway orientated programming
    - How do these ideas look put together?
*)

// A result that either succeeds with type of 'a, or
//  fails with error type of 'b.
type Result<'TSuccess,'TFailure> = 
    | Success of 'TSuccess
    | Failure of 'TFailure

// Use the employee model from other file
#load "2_Features.fsx"
open System
type Employee = ``2_Features``.Employee

let validateName (e: Employee) =
   if e.Name = null then Failure "Name must not be null"
   else if e.Name = "" then Failure "Name must not be empty"
   else if e.Name.Length > 100 then Failure "Name is too long"
   else Success e

let validateEmail (e: Employee) =
   if e.NiNumber = null then Failure "Name must not be null"
   else if e.NiNumber = "" then Failure "Name must not be empty"
   else if e.NiNumber.Length > 100 then Failure "Name is too long"
   else Success e

let validateDoB (e: Employee) = 
    if (DateTime.Now - e.DoB) < TimeSpan.FromDays(365.0*18.0) then Failure "Employee must be over 18."
    else Success e

let (>=>) switch1 switch2 x = 
    match switch1 x with
    | Success s -> switch2 s
    | Failure f -> Failure f 

let validateEmployee = validateName >=> validateEmail >=> validateDoB