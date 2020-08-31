using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreamingEndpoint.Utils;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace StreamingEndpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageStreamingController : ControllerBase
    {
        #region Data Members

        private readonly ILogger<ImageStreamingController> _logger;
        public HubConnection _connection { get; }

        #endregion

        #region Constructors

        public ImageStreamingController(ILogger<ImageStreamingController> logger)
        {
            _logger = logger;

            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44363/ImageStreamingHub")
                //.AddMessagePackProtocol()
                .Build();

            _connection.StartAsync().Wait();
        }

        #endregion

        #region Public Functions

        [HttpGet]
        public IActionResult Get()
        {
            return new PushStreamResult(PushStreamData, "application/octet-stream");
        }

        #endregion

        #region Private Functions

        async Task PushStreamData(Stream stream, CancellationToken cancel)
        {
            Response.Headers.Add("Cache-Control", "no-cache");

            _connection.On<byte[]>("ReceiveStream", async (item) =>
            {
                await Response.Body.WriteAsync(item, 0, item.Length);
                await Response.Body.FlushAsync();
            });

            try
            {
                while (_connection.State == HubConnectionState.Connecting || _connection.State == HubConnectionState.Connected)
                {
                    if (cancel.IsCancellationRequested)
                        return;

                    //var data = _connection.StreamAsync<DataChunk>("Streaming", 10);
                    //await foreach (var item in data)
                    //{
                    //    await Response.Body.WriteAsync(item.Buffer, 0, item.Size);
                    //    await Response.Body.FlushAsync();
                    //}
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                Response.Body.Close();
            }

            await Task.CompletedTask;
        }

        //async Task PushData(Stream stream, CancellationToken cancel)
        //{
        //    var buffer = new byte[65536];

        //    using var video = System.IO.File.Open(GetFilePath("movie.mp4"), FileMode.Open, FileAccess.Read);

        //    var length = (int)video.Length;
        //    var bytesRead = 1;

        //    while (length > 0 && bytesRead > 0)
        //    {
        //        bytesRead = await video.ReadAsync(buffer, 0, Math.Min(length, buffer.Length));
        //        await stream.WriteAsync(buffer, 0, buffer.Length, cancel);
        //        await stream.FlushAsync(cancel);

        //        length -= bytesRead;
        //    }
        //}

        //string GetFilePath(string fileName)
        //{
        //    return $"DataStreaming/Video/{fileName}";
        //}

        #endregion
    }
}