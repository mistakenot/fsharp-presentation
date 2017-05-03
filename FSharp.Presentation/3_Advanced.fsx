(* 4.0 Computational Expressions
    - Allow you modify how keywords act to perform repetitive work
    - Useful for chaining functions together (Monads)
*)
open System
let tryGetString () = Some "0"      // Tries to read a string, Some if success, None if fail
let tryParseInt str =               // Tries to parse int, Some if success, None if fail
    try
        let i = Int32.Parse str
        Some i
    with
    | :? FormatException -> None

let tryDivide x =                 // Tries to divide two numbers, Some if success, None if fail
    try
        Some (5 / x)
    with
    | :? DivideByZeroException -> None

// Now to chain these functions together
let uglyResult: option<int> = 
    match tryGetString() with
    | Some str ->
        match tryParseInt str with
        | Some i -> 
            match tryDivide i with
            | Some result -> Some result
            | None -> None
        | None -> None
    | None -> None

// Or...
// Builders are used to create computational expressions
// Computational expressions modify how keywords work to perform 
//  repetitive work
type OptionBuilder() =
    member x.Bind(v,f) =        // Bind is used when let! is called
        match v with 
        | Some x -> f x
        | None -> None 
    member x.Return v = Some v  // Return is used when return is called

// Create a singleton instance
let option = OptionBuilder()

// And use it as a computational expression
let betterResult: option<int> = 
    option {
        let! str = tryGetString()
        let! i = tryParseInt str
        let! result = tryDivide i
        return result
    }





(* 4.1 Type Providers
    - Allow you perform strongly typed queries on datasources
    - Providers exist for Json, Sql, OData, etc.
*)

#r "FSharp.Data.TypeProviders.dll"
#r "System.Data.Services.Client.dll"

open Microsoft.FSharp.Data.TypeProviders
open System.Linq

type Northwind = ODataService<"http://services.odata.org/Northwind/Northwind.svc">
let db = Northwind.GetDataContext()

// Using C# linq
let linqQuery = db.Customers.Where(fun c -> c.CustomerID = "1")

// Using F# query expression
let expressionQuery = 
    query {
        for customer in db.Customers do
        where (customer.CustomerID = "1")
        select customer
    }





(* 4.2 MailboxProcessor
    - Lightweight async actors using the MailboxProcessor type
    - Actor is a recursive function that holds some state
*)
type Message = 
    | Add of int
    | Multiply of int

let messageProcessor (mailbox: MailboxProcessor<Message>)  = 
    let rec loop (state: int) = async {
        let! msg = mailbox.Receive()        // Non-blocking wait for a message
        let newState =                      // Update state depending on message
            match msg with
            | Add x -> state + x
            | Multiply x -> state * x
        printf "Updated to %i\n" newState   // Log the result
        return! loop newState               // Non-blocking recursive call with new state
    }
    loop 0                                  // Start the loop with the default state

let calculatorActor = MailboxProcessor.Start messageProcessor

calculatorActor.Post (Add 1)
calculatorActor.Post (Multiply 5)
calculatorActor.Post (Add 3)