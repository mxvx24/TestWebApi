namespace TestWebAPI.LoadTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mime;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using TestWebApi.Data.Contexts;
    using TestWebApi.Domain.Entities;

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The http client.
        /// </summary>
        private static readonly HttpClient HttpClient;

        /// <summary>
        /// Initializes static members of the <see cref="Program"/> class.
        /// </summary>
        static Program()
        {
            // Test
            HttpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:5000") };
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            HttpClient.Timeout = TimeSpan.FromMinutes(10);
        }

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            Thread.Sleep(10000);

            var stopWatch = new Stopwatch();

            IEnumerable<int> runCount = Enumerable.Range(0, 100000);

            stopWatch.Start();

            /*var products = new List<Product>();
            runCount.ToList().ForEach(
                i =>
                    {
                        products.Add(new Product { Name = $"Product_{i}" });
                    }); */

            using (var db = new ProductDbContext())
            {
                List<Product> p  = db.Products.FromSql("SpUpdateAllProducts @p0, @p1", new object[] { "1,2,3", "TestWebApiLoad" }).ToList();
                Console.WriteLine($"Count: {p.Count()}");
            }

            /*Parallel.ForEach(
                    runCount,
                    new ParallelOptions { MaxDegreeOfParallelism = 3 },
                    i =>
                        {
                            Console.WriteLine($"Running {i}");

                        var request = new HttpRequestMessage()
                                          {
                                              Method = HttpMethod.Get,
                                              RequestUri = new Uri("http://localhost:5000/api/employees?nameLike=Mohammed")
                                              // Content = 
                                          };

                        HttpResponseMessage response = httpClient.SendAsync(request).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine($"Result of {i}: {result}");
                        }
                        else
                        {
                            Console.WriteLine($"Unsuccessful. Response status code: {response.StatusCode}.");
                        }
                        });*/

            stopWatch.Stop();
            Console.WriteLine($"Elapsed: {stopWatch.ElapsedMilliseconds} ms");

            Console.WriteLine("\n\nPress any key to exit.");
            Console.Read();
        }
    }
}
