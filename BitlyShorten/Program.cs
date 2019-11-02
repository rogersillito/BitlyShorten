using System;
using System.Linq;
using System.Net;
using RestSharp;

namespace BitlyShorten
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Required Url parameter not provided!");
                Environment.ExitCode = 1;
                return;
            }

            var url = args[0];

            var token = System.Configuration.ConfigurationManager.AppSettings["GenericAccessToken"];
            const string baseAddress = "https://api-ssl.bitly.com/v4/";

            var client = new RestClient(baseAddress);

            // https://dev.bitly.com/v4_documentation.html#operation/createBitlink
            var request = new RestRequest("shorten", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", token);
            request.AddJsonBody(new Link { long_url = url });

            var response = client.Execute<ShortenResponse>(request);
            if (!new[] { HttpStatusCode.OK, HttpStatusCode.Created }.Contains(response.StatusCode))
            {
                Console.Error.WriteLine($"bitly shorten not successful: {response.StatusCode} {response.Content}");
                Environment.ExitCode = 1;
                return;
            }

            Console.WriteLine(response.Data.link);
        }
    }
}
