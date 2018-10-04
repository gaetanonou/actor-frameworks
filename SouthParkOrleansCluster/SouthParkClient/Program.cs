using FluentColorConsole;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using SouthParkInterfaces;
using System;
using System.Data.SqlClient;
using System.Net;

namespace SouthParkClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Build connection string
            SqlConnectionStringBuilder connstring = new SqlConnectionStringBuilder();
            connstring.DataSource = @".\MSSQLSERVER2017";
            connstring.InitialCatalog = "SouthPark";
            connstring.IntegratedSecurity = true;
            string invariant = "System.Data.SqlClient";


            //configure the client with proper cluster options, logging and clustering
            var client = new ClientBuilder()
               .UseAdoNetClustering(options =>
               {
                   options.ConnectionString = connstring.ConnectionString;
                   options.Invariant = invariant;
               })
              .Configure<ClusterOptions>(options =>
              {            
                  options.ClusterId = "SouthParkCluster";
                  options.ServiceId = "SouthParkClient";
              })
              .ConfigureLogging(logging => logging.AddConsole())
              .Build();

            //connect the client to the cluster, in this case, which only contains one silo
            var connectTask = client.Connect();
            connectTask.Wait();
            ColorConsole.WithGreenText.WriteLine("Connected to silo");

            do
            {
                // Call kenny
                var kenny = client.GetGrain<ICharacterGrain>("Kenny");

                var askTask = kenny.AreYouAlive();
                askTask.Wait();

                ColorConsole.WithGreenText.WriteLine($"Kenny is alive, you asked {askTask.Result} time(s)");
            }
            while (Console.ReadKey().KeyChar != 'x');
        }
    }
}
