open System
open System.Net
open System.Net.Sockets
open System.Text
open System.IO

let port = 12345 // Set your desired port number
let mutable clientCounter = 0
let mutable result = 0
let mutable isTerminated = false

let handleClient (clientSocket: TcpClient) =
    async {
        clientCounter <- clientCounter + 1
        let clientId = clientCounter
        let clientStream = clientSocket.GetStream()
        let reader = new StreamReader(clientStream, Encoding.ASCII)
        let writer = new StreamWriter(clientStream, Encoding.ASCII)
        writer.AutoFlush <- true
        try
            // Send a welcome message to the client
            writer.WriteLine("Hello!")

            while true do
                // Read client input
                let input = reader.ReadLine()

                // Handle client commands
                match input with
                | null -> // Client disconnected
                    printfn "Client disconnected"
                    return ()
                | "bye" -> // Close the socket for this client
                    result <- -5
                    writer.WriteLine(sprintf "%d" result)
                    printfn "Received: %s" input
                    printfn "Responding to client %d with result: %d" clientId result
                    return ()
                | "terminate" -> // Close all sockets and exit
                    result <- -5
                    writer.WriteLine(sprintf "%d" result)
                    printfn "Received: %s" input
                    printfn "Responding to client %d with result: %d" clientId result
                    isTerminated<-true
                    // Cleanup: Close all client streams
                    Environment.Exit(0)
                    // You can add logic here to close all client sockets if needed
                    return ()
                | _ -> // Handle other commands (e.g., calculations)
                    let parts = input.Split(' ')
                    let op = parts.[0]
                    let nums =  
                        Array.sub parts 1 (parts.Length - 1)
                        |> Array.map (fun num ->
                            match Int32.TryParse(num) with
                            | (true, n) -> n
                            | _ -> -1 // Invalid number format
                        )
                    
                    try
                        if op <> "add" && op <> "subtract" && op <> "multiply" then
                            result <- -1
                        elif nums.Length < 2 then
                            result <- -2 // Number of inputs is less than two
                        elif nums.Length > 4 then
                            result <- -3 // Number of inputs is more than four
                        elif Array.exists (fun n -> n = -1) nums then
                            result <- -4 // One or more inputs contain non-numbers
                        else
                            result <-
                                match op with
                                | "add" -> Array.sum nums
                                | "subtract" -> Array.reduce (-) nums
                                | "multiply" -> Array.fold (*) 1 nums
                                | "divide" -> Array.fold (/) nums.[0] (Array.skip 1 nums)
                                | _ -> -1 // Incorrect operation command
                            if result < 0 then
                               result <- -1 // Incorrect operation command
                        // Print the response with client ID
                        writer.WriteLine(sprintf "%d" result)
                        printfn "Received: %s" input
                        printfn "Responding to client %d with result: %d" clientId result
                    with
                    | :? System.FormatException ->
                        writer.WriteLine("-4") // One or more inputs contain non-numbers
        with
        | :? System.IO.IOException ->
            // Handle any IO errors (e.g., client disconnect)
            return()
        | ex ->
            printfn "Error: %s" ex.Message
    }

[<EntryPoint>]
let main argv =
    let listener = new TcpListener(IPAddress.Any, port)
    listener.Start()

    printfn "Server is listening on port %d" port

    let rec acceptClients () =
       async {
        let clientSocket = listener.AcceptTcpClientAsync().Result
        Async.Start (handleClient clientSocket)
        if not isTerminated then
                return! acceptClients ()
    }

    // Start accepting clients
    Async.RunSynchronously (acceptClients ())


    0 // Exit code