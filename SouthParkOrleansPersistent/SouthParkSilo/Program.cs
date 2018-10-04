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
            //Build connection string
            SqlConnectionStringBuilder connstring = new SqlConnectionStringBuilder();
            connstring.DataSource = ".";
            connstring.InitialCatalog = "SouthPark";
            connstring.UserID = "SouthPark";
            connstring.Password = "SouthPark";

            //define the cluster configuration
            var builder = new SiloHostBuilder()                
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "SoutPark1";
                })
                .AddAdoNetGrainStorage("LocalSql", options =>
                {
                    options.ConnectionString = connstring.ConnectionString;
                    options.UseJsonFormat = true;
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
