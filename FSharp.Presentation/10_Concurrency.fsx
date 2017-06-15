(* CONCURRENCY *)

// Async expressions encapsulate a background task
open System
open System.IO

let fileWriteWithAsync = 
    // create a stream to write to
    use stream = new System.IO.FileStream("test.txt",System.IO.FileMode.Create)
    let asyncResult = stream.BeginWrite(Array.empty, 0, 0, null, null)
    
    // create an async wrapper around an IAsyncResult
    let async = Async.AwaitIAsyncResult(asyncResult) |> Async.Ignore

    // keep working
    printfn "Doing something useful while waiting for write to complete"

    // block on the timer now by waiting for the async to complete
    Async.RunSynchronously async 
    printfn "Async write completed"

let sleepWorkflow  = async {
    printfn "Starting sleep workflow at %O" DateTime.Now.TimeOfDay
    do! Async.Sleep 2000
    printfn "Finished sleep workflow at %O" DateTime.Now.TimeOfDay
}

Async.RunSynchronously sleepWorkflow 





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
