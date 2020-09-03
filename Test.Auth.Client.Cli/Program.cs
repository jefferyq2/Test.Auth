using System;
using System.IO;
using System.Net;
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
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("_IGNORE/appsettings.local.json", optional: true, reloadOnChange: true)
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
            };

            handler.UseDefaultCredentials = Configuration.GetValue<bool>(nameof(HttpClientHandler.UseDefaultCredentials));
            if (Configuration.GetValue<bool>("Set" + nameof(CredentialCache.DefaultCredentials)))
                handler.Credentials = CredentialCache.DefaultCredentials;
            else if (Configuration.GetValue<bool>("Set" + nameof(CredentialCache.DefaultNetworkCredentials)))
                handler.Credentials = CredentialCache.DefaultNetworkCredentials;

            using var http = new HttpClient(handler, disposeHandler: false);

            var baseUrl = Configuration["BaseUrl"];
            var url = Flurl.Url.Combine(baseUrl, "api/who");

            Console.WriteLine($"Url: {url}");
            var resp = await http.GetStringAsync(url);
            Console.WriteLine(resp);
        }
    }
}
