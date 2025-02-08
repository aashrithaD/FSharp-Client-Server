open System
open System.Net
open System.Net.Sockets
open System.Text
open System.IO
open System.Threading.Tasks

let serverAddress = IPAddress.Parse("192.168.0.33") // Replace with your server's IP address or hostname
let serverPort = 12345 // Replace with the server's port number

let asyncConnectAndCommunicate () =
    async {
        try
            let client = new TcpClient()
            client.Connect(serverAddress, serverPort)
            use stream = client.GetStream()
            use reader = new StreamReader(stream, Encoding.ASCII)
            use writer = new StreamWriter(stream, Encoding.ASCII)
            writer.AutoFlush <- true

            // Receive the server's welcome message
            let welcomeMessage = reader.ReadLine()
            printfn "Server says: %s" welcomeMessage

            while true do
                // Let the user input a command
                printf "Enter a command: "
                let command = Console.ReadLine()

                // Send the command to the server
                writer.WriteLine(command)
                

                // Receive and print the server's response
                let response = reader.ReadLine()
                let errorCodes = Map["-1","incorrect operation command";
                "-2","number of inputs is less than two";
                "-3","number of inputs is more than four";
                "-4","one or more of the input(s) contain non-number(s)"]
                match response with
                |"-1" ->
                    printfn "Server response: %s" errorCodes[response]
                |"-2" ->
                    printfn "Server response: %s" errorCodes[response]
                |"-3" ->
                    printfn "Server response: %s" errorCodes[response]
                |"-4" ->
                    printfn "Server response: %s" errorCodes[response]
                |"-5" -> // Handle the specific error code
                    printfn "exit"
                    Environment.Exit(0)
                | _ ->
                    printfn "Server response: %s" response
        with
        | :? SocketException as ex ->
            printfn "SocketException: %s" ex.Message
        | :? IOException as ex ->
            printfn "IOException: %s" ex.Message
    }

[<EntryPoint>]
let main argv =
    Async.RunSynchronously (asyncConnectAndCommunicate())
    0 // Exit code
