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






// Other example of standard builders
open System.Net
let req1 = HttpWebRequest.Create("http://fsharp.org")
let req2 = HttpWebRequest.Create("http://google.com")
let req3 = HttpWebRequest.Create("http://bing.com")

req1.BeginGetResponse((fun r1 -> 
    use resp1 = req1.EndGetResponse(r1)
    printfn "Downloaded %O" resp1.ResponseUri

    req2.BeginGetResponse((fun r2 -> 
        use resp2 = req2.EndGetResponse(r2)
        printfn "Downloaded %O" resp2.ResponseUri

        req3.BeginGetResponse((fun r3 -> 
            use resp3 = req3.EndGetResponse(r3)
            printfn "Downloaded %O" resp3.ResponseUri

            ),null) |> ignore
        ),null) |> ignore
    ),null) |> ignore


// async builder
let asyncWorkflow: Async<unit> = async {
    use! resp1 = req1.AsyncGetResponse()  
    printfn "Downloaded %O" resp1.ResponseUri

    use! resp2 = req2.AsyncGetResponse()  
    printfn "Downloaded %O" resp2.ResponseUri

    use! resp3 = req3.AsyncGetResponse()  
    printfn "Downloaded %O" resp3.ResponseUri

    } 

asyncWorkflow |> Async.RunSynchronously