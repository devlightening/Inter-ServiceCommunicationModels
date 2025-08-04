using Grpc.Net.Client;
using GrpcService1;

var channel = GrpcChannel.ForAddress("https://localhost:7057/");
var greetClient = new Greeter.GreeterClient(channel);
var reply = greetClient.SayHello(new HelloRequest { Name =  Console.ReadLine() });
await Task.Run(async() =>
{
    while(await reply.ResponseStream.MoveNext(new CancellationToken()))    
        Console.WriteLine($"Gelen Mesaj :  { reply.ResponseStream.Current.Message}");
  

});