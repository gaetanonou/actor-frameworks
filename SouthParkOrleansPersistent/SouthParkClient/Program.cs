using FluentColorConsole;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using SouthParkInterfaces;
using System;

namespace SouthParkClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //configure the client with proper cluster options, logging and clustering
            var client = new ClientBuilder()
              .UseLocalhostClustering()
              .Configure<ClusterOptions>(options =>
              {
                  options.ClusterId = "dev";
                  options.ServiceId = "SouthPark1";
              })
              .ConfigureLogging(logging => logging.AddConsole())
              .Build();

            //connect the client to the cluster, in this case, which only contains one silo
            var connectTask = client.Connect();
            connectTask.Wait();
            ColorConsole.WithGreenText.WriteLine("Connected to silo");

            // Call kenny
            var kenny = client.GetGrain<ICharacterGrain>("Kenny");

            var askTask = kenny.AreYouAlive();
            askTask.Wait();

            ColorConsole.WithGreenText.WriteLine($"Kenny is alive, you asked {askTask.Result} time(s)");
            Console.ReadKey();
        }
    }
}
