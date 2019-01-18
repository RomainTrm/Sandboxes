

module Infrastructure = 
    
    type EventStore<'Event> = 
        {
            Get : unit -> 'Event List
            Append : 'Event list -> unit
        }

    module EventStore = 

        type Msg<'Event> =
        | AddEvents of 'Event list
        | GetHistory of AsyncReplyChannel<'Event list>

        let agent = 
            MailboxProcessor.Start(fun inbox -> 
                let rec loop history = async {
                    let! msg = inbox.Receive()
                    match msg with     
                    | AddEvents events -> 
                        return! loop (history@events)  
                    | GetHistory replyChannel ->
                        replyChannel.Reply history
                        return! loop history                        
                }
                
                loop [])

        let createInstance () = 
            {
                Get = fun () -> agent.PostAndReply GetHistory
                Append = fun events -> agent.Post (AddEvents events)
            }


module Domain =

    type Event = 
    | Add of int 
    | Remove of int 
    | Reset

module Helper = 

    let printUl = List.iteri (fun i item -> printfn " %i: %A" (i+1) item)

    let printEvents events = 
        events |> List.length |> printfn "History (Length: %i)"
        events |> printUl

open Infrastructure
open Domain

let eventStore : EventStore<Event> = EventStore.createInstance()

eventStore.Append [Add 4]
eventStore.Append [Add 3]
eventStore.Append [Remove 2]
eventStore.Append [Remove 1; Reset]

eventStore.Get () |> Helper.printEvents