using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureSignalRConsoleTest
{
    class Program
    {
        static Task<IServiceHubContext> CreateHubContextAsync()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string signalRConnString = config.GetConnectionString("AzureSignalRConnectionString");

            var serviceManager = new ServiceManagerBuilder()
                .WithOptions(option =>
                {
                    option.ConnectionString = signalRConnString;
                    option.ServiceTransportType = ServiceTransportType.Persistent;
                })
                .Build();

            return serviceManager.CreateHubContextAsync("testhub");
        }

        static async Task TestAsync()
        {
            var hubContext = await CreateHubContextAsync();

            Console.WriteLine("Press any key to send a message, press Spacebar to reconnect");
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Spacebar)
                {
                    hubContext = await CreateHubContextAsync();
                    Console.WriteLine("Reconnected!");
                }

                await hubContext.Clients.All.SendCoreAsync("message-from-server", new[]
                {
                    DateTime.Now.ToString("o")
                });

                Console.WriteLine("Message sent!");
            }
        }

        static void Main(string[] args)
        {
            TestAsync().Wait();
        }
    }
}
