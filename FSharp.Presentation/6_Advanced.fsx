(* 4.0 Computational Expressions
    - Allow you modify how keywords act to perform repetitive work
    - Useful for chaining functions together
*)
open System
let tryGetString () : option<string> = failwith "not implemented"
let tryParseInt (str: string) : option<int> = failwith "not implemented"
let tryDivideZeroByX (x: int) : option<int> = failwith "not implemented"

// Now to chain these functions together
let uglyResult: option<int> = 
    match tryGetString() with
    | Some str ->
        match tryParseInt str with
        | Some i -> 
            match tryDivideZeroByX i with
            | Some result -> Some result
            | None -> None
        | None -> None
    | None -> None
// Yuck.

// Or...
// Builders are used to create 'computational expressions'
// Computational expressions allow you to modify what keywords
//  do to minimise boilerplate. (Monads)
type OptionBuilder() =
    member x.Bind(previousResult, nextStep) = // Bind is used when let! is called
        match previousResult with 
        | Some x -> nextStep x
        | None -> None 
    member x.Return(value) = Some value  // Return is used when return is called

// Create a singleton instance
let option = OptionBuilder()

// And use it as a computational expression
let betterResult: option<int> = 
    option {
        let! str = tryGetString()
        let! i = tryParseInt str
        let! result = tryDivideZeroByX i
        return result
    }


(* 4.1 MailboxProcessor
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
