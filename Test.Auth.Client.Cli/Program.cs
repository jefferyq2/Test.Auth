using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Test.Auth.Client.Cli
{
    public class Program
    {
        static IConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                ;
            Configuration = builder.Build();

            if (Configuration.GetValue<bool>("DumpConfig"))
            {
                Console.WriteLine("Dumping Configuration:");
                foreach (var kv in Configuration.AsEnumerable())
                {
                    Console.WriteLine($"{kv.Key} = {kv.Value}");
                }
            }

            using var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                UseDefaultCredentials = Configuration.GetValue<bool>(nameof(HttpClientHandler.UseDefaultCredentials)),
            };
            using var http = new HttpClient(handler, disposeHandler: false);

            var baseUri = new Uri(Configuration["BaseUrl"]);
            var url = new Uri(baseUri, "/api/who");
            var resp = await http.GetStringAsync(url);
            Console.WriteLine(resp);
        }
    }
}
