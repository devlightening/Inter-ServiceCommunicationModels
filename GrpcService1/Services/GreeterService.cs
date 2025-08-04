using Grpc.Core;
using GrpcService1;

namespace GrpcService1.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override async Task SayHello(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            await Task.Run(() =>
            {
                int count = 0;
                while (!context.CancellationToken.IsCancellationRequested && count < 10)
                {
                    _logger.LogInformation("Sending message {Count}", count);
                    responseStream.WriteAsync(new HelloReply
                    {
                        Message = $"Hello {request.Name} {count++}"
                    }).Wait();
                    Thread.Sleep(1000); // Simulate some delay
                }
            });
        }


    }
}
