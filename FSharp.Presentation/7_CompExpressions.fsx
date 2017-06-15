(* 4.0 Computational Expressions
    - Allow you modify how keywords act to perform repetitive work
    - Useful for chaining functions together
*)
open System

let tryGetString () : option<string> = 
    failwith "not implemented"

let tryParseInt (str: string) : option<int> = 
    failwith "not implemented"

let tryDivideZeroByX (x: int) : option<int> = 
    failwith "not implemented"

let getString = 
    match tryGetString() with
    | Some s -> printfn "Success! Result was %s" s
    | None -> printfn "Failure."

// type Option<'a> = 
//  | Some 'a
//  | None




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




// Or we could create a custom operator
let (>?>) value next = 
    match value with
    | Some v -> next v
    | None -> None

let bestResult = tryGetString() >?> tryParseInt >?> tryDivideZeroByX