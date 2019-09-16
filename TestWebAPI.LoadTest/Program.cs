﻿namespace TestWebAPI.LoadTest
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mime;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The http client.
        /// </summary>
        private static HttpClient httpClient;

        /// <summary>
        /// Initializes static members of the <see cref="Program"/> class.
        /// </summary>
        static Program()
        {
            httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:5000") };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            httpClient.Timeout = TimeSpan.FromMinutes(10);
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

            var runCount = Enumerable.Range(0, 1000);

            stopWatch.Start();

            Parallel.ForEach(
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

                        var response = httpClient.SendAsync(request).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine($"Result of {i}: {result}");
                        }
                        else
                        {
                            Console.WriteLine($"Unsuccessful. Response status code: {response.StatusCode}.");
                        }
                    });

            stopWatch.Stop();
            Console.WriteLine($"Elapsed: {stopWatch.ElapsedMilliseconds} ms");

            Console.WriteLine("\n\nPress any key to exit.");
            Console.Read();
        }
    }
}
