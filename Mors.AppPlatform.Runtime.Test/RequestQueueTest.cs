namespace Mors.AppPlatform.Runtime.Test
{
    public class RequestQueueTest
    {
        /* Actor behavior under test: RequestQueue
         * Its responsibility is to queue requests for execution.
         * 
         * The requests are either commands or queries.
         * The requests are coming from clients.
         * The execution may finish successfuly, producing a result, or fail.
         * It should report the results back to the client, upon successful request execution.
         * It should report the execution failure back to the client, upon it happening.
         *
         * State
         *  per request
         *  
         * 
         * Interactions
         * 
         * with 'request router'
         *  dispatch request for execution
         *  retrieve execution result
         * 
         * with 'client'
         *  accept a request
         *    with request object,
         *    and response actor address (why? what is a response actor?)
         *  spawn a child for processing the request
         *    with request object,
         *    with response actor address
         *    with a timeout
         *    with address of request processor
         *  the child sends 'process' message with self as recipient
         *    child waits for response
         *    if timeout hits send timeouted response to recipient
         *    otherwise send response back to recipient
         *    in both cases die afterwards
         *    what if response comes afterwards?
         *      gets sent to a dead actor
         * Knowledge
         * 
         *  it must know which produces a result?
         *  fire'n'forget or wait for result
         *
         * How to setup actor system?
         * How to avoid boilerplate?
         * How to separate from implementation? namely akka, is there a point to do so?
         * is there a point in trying to avoid akkas assumptions?
         * is less assumptionist approach possible or sensible?
         * you cant build on something contradicting your assumptions
         * is it sensible to create intent with code?
         * add it as another layer while wanting the ability to make swift changes?
         * adjust mechanism to allow intent reveal itself
         * 
         * actor wants to respond to messages
         * actor wants to have non reentrant, single threaded message handlers
         * actor does not want to synchronize with other actor in message handler
         * actor wants to handle messages in order they came in
         * what about order of messages sent to other actors?
         * messages sent from one actor are received in order by the receiver
         * no ordering can be assumed between different receivers 
         * messages are immutable
         * messages may get lost 
         * messages are serializable
         * location is transparent
         *
         * akka adapters:
         * 
         * ISelf
         *  receive message by convention
         *  send message at address at-least-once
         *  own address
         *  
         * actor creation
         *  ordered: child creation and message sent to it afterwards from creator
         *  needs a behavior that processes messages
         */

    }
}
