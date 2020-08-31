using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace BlazorConsumer.Client.Services
{
    public class LiveStreamingService : ILiveStreamingService
    {
        const string ENDPOINT_TEXT_STREAMING = "https://localhost:44363/textstreaming";
        private readonly HttpClient _httpClient;

        public LiveStreamingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async public IAsyncEnumerable<string> GetMessagesAsync()
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ENDPOINT_TEXT_STREAMING))
            {
                request.SetBrowserResponseStreamingEnabled(true);
                using var result = await _httpClient.SendAsync(request);
                Console.WriteLine("RESULT READY");
                var stream = await result.Content.ReadAsStreamAsync();
                Console.WriteLine("STREAM READY");
                using (var rdr = new StreamReader(stream))
                {
                    Console.WriteLine("START TO READ");
                    while (!rdr.EndOfStream)
                    {
                        var data = await rdr.ReadLineAsync();
                        Console.WriteLine(data);

                        yield return data;
                    }
                }
            }
        }
    }
}
