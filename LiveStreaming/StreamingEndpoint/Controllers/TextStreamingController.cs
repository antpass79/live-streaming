using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreamingEndpoint.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamingEndpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextStreamingController : ControllerBase
    {
        #region Data Members

        private readonly ILogger<VideoStreamingController> _logger;
        public HubConnection _connection { get; }

        #endregion

        #region Constructors

        public TextStreamingController(ILogger<VideoStreamingController> logger)
        {
            _logger = logger;

            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44363/TextStreamingHub")
                //.AddMessagePackProtocol()
                .Build();

            _connection.StartAsync().Wait();
        }

        #endregion

        #region Public Functions

        [HttpGet]
        async public Task<IActionResult> Get()
        {
            return await Task.FromResult(new PushStreamResult(PushStreamData, "text/event-stream"));
        }

        #endregion

        #region Private Functions

        async Task PushStreamData(Stream stream, CancellationToken cancel)
        {
            _connection.On<string>("ReceiveStream", async (item) =>
            {
                var data = Encoding.UTF8.GetBytes($"{item}");
                await Response.Body.WriteAsync(data, 0, data.Length);
                await Response.Body.FlushAsync();
            });

            while (_connection.State == HubConnectionState.Connecting || _connection.State == HubConnectionState.Connected)
            {
                if (cancel.IsCancellationRequested)
                    return;

                //var cancellationTokenSource = new CancellationTokenSource();
                //var result = _connection.StreamAsync<string>("Streaming", 10, cancellationTokenSource.Token);
                //await foreach (var item in result)
                //{
                //    var data = Encoding.UTF8.GetBytes(item);
                //    await Response.Body.WriteAsync(data, 0, item.Length);
                //    await Response.Body.FlushAsync();

                //    await Task.Delay(1000, cancel);
                //}
            }

            await Task.CompletedTask;
        }

        //async Task PushData(Stream stream, CancellationToken cancel)
        //{
        //    int count = 0;
        //    while (count < 5)
        //    {
        //        count++;
        //        if (cancel.IsCancellationRequested)
        //            return;

        //        await Task.Delay(1000, cancel);
        //        var content = $"Message at {DateTime.Now}\n";
        //        var data = Encoding.UTF8.GetBytes(content);
        //        await stream.WriteAsync(data, 0, data.Length, cancel);
        //        await stream.FlushAsync(cancel);
        //    }
        //}

        #endregion
    }
}