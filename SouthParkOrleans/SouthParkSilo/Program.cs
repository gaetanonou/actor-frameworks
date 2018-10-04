using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;

namespace SouthParkSilo
{
    class Program
    {
        static void Main(string[] args)
        {
            //define the cluster configuration
            var builder = new SiloHostBuilder()                
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "SoutPark1";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureLogging(logging => logging.AddConsole());
            
            //build the silo
            var host = builder.Build();

            //start the silo
            var startTask = host.StartAsync();

            Console.ReadKey();
            
        }
    }
}
