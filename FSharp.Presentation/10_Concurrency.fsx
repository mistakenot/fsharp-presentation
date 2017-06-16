(* CONCURRENCY *)

// Async expressions encapsulate a background task
open System.Net
let req1 = HttpWebRequest.Create("http://fsharp.org")
let req2 = HttpWebRequest.Create("http://google.com")
let req3 = HttpWebRequest.Create("http://bing.com")

let asyncWorkflow = async {
    use! resp1 = req1.AsyncGetResponse()  
    printfn "Downloaded %O" resp1.ResponseUri

    use! resp2 = req2.AsyncGetResponse()  
    printfn "Downloaded %O" resp2.ResponseUri

    use! resp3 = req3.AsyncGetResponse()  
    printfn "Downloaded %O" resp3.ResponseUri

    do! Async.Sleep 1000

    } 

Async.RunSynchronously asyncWorkflow 





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
