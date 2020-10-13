using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;

namespace dmc_auth
{
  public class Program
  {
    public static void Main(string[] args)
    {
      // Check auth server health
      var httpClientHandler = new HttpClientHandler();
      httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; // DEBUGGING ONLY
      var url = $"{Constant.GetAuthURL()}/health/ready";
      var httpClient = new HttpClient(httpClientHandler);
      var responseMessage = httpClient.GetAsync(url).Result;
      if (responseMessage.StatusCode != HttpStatusCode.OK)
      {
        Environment.Exit(1);
        return;
      }
      CreateHostBuilder(args).Build().Run();
    }

    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
            .Build();

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
          logging.ClearProviders();
          logging.AddConsole();
        })
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
