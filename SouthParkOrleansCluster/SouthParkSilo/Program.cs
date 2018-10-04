using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Data;
using System.Data.SqlClient;

namespace SouthParkSilo
{
    class Program
    {
        static void Main(string[] args)
        {
            //request node number            
            int siloPort = 0;
            int gatewayPort = 0;
            string serviceId = "";

            //Get config from command line
            if (args.Length == 3)
            {
                int.TryParse(args[0], out siloPort);
                int.TryParse(args[1], out gatewayPort);
                serviceId = args[2];
            }

            //IF not provided by command line, ask keuboard input
            if (siloPort == 0)
            {
                Console.WriteLine("Enter the node number:");
                switch (Console.ReadKey().KeyChar)
                {
                    case '1': siloPort = 10000; gatewayPort = 10002; serviceId = "SouthPark1"; break;
                    case '2': siloPort = 20000; gatewayPort = 20002; serviceId = "SouthPark2"; break;
                    case '3': siloPort = 30000; gatewayPort = 30002; serviceId = "SouthPark3"; break;
                    default: return;

                }
            }

            //Write out configuration information
            FluentColorConsole.ColorConsole.WithYellowBackground.AndBlackText.WriteLine($"Silo port: {siloPort}, Gateway port {gatewayPort}, Sile name {serviceId}");

            //Build connection string
            SqlConnectionStringBuilder connstring = new SqlConnectionStringBuilder();
            connstring.DataSource = @".\MSSQLSERVER2017";
            connstring.InitialCatalog = "SouthPark";
            connstring.IntegratedSecurity = true;
            string invariant = "System.Data.SqlClient";

            //define the cluster configuration
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "SouthParkCluster";
                    options.ServiceId = serviceId;
                })
                .UseAdoNetClustering(options =>
                {
                    options.ConnectionString = connstring.ConnectionString;
                    options.Invariant = invariant;
                })
                .AddAdoNetGrainStorage("LocalSql", options =>
                {
                    options.ConnectionString = connstring.ConnectionString;
                    options.Invariant = invariant;
                    options.UseJsonFormat = true;
                })
                .Configure<EndpointOptions>(options =>
                {
                    options.SiloPort = siloPort;
                    options.GatewayPort = gatewayPort;
                    options.AdvertisedIPAddress = IPAddress.Loopback;
                })
                .ConfigureLogging(logging => logging.AddConsole());
            
            //build the silo
            var host = builder.Build();

            //start the silo
            var startTask = host.StartAsync();

            Console.ReadKey();
            
        }
    }
}
