(* 4.1 MailboxProcessor
    - Lightweight async actors using the MailboxProcessor type
    - Actor is a recursive function that holds some state
*)
type Message = 
    | Add of int
    | Multiply of int

let rec loop (state: int) (mailbox: MailboxProcessor<Message>) = async {
        let! msg = mailbox.Receive()        // Non-blocking wait for a message
        let newState =                      // Calculate the new state
            match msg with
            | Add x -> state + x
            | Multiply x -> state * x
        printf "Updated to %i\n" newState   // Log the result
        return! loop newState mailbox      // Loop to get next message
    }

let calculatorActor = MailboxProcessor.Start (loop 0)

calculatorActor.Post (Add 1)
calculatorActor.Post (Multiply 5)
calculatorActor.Post (Add 3)

