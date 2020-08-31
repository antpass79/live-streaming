using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlazorProducer.Shared.Models;
using BlazorProducer.Shared.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlazorProducer.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VideoDownloadController : ControllerBase
    {
        private readonly ILogger<VideoDownloadController> _logger;
        private readonly IStreamingHubProxy<DataChunk<string>> _streamingHubProxy;

        public VideoDownloadController(
            ILogger<VideoDownloadController> logger,
            IStreamingHubProxy<DataChunk<string>> streamingHubProxy)
        {
            _logger = logger;
            _streamingHubProxy = streamingHubProxy;

            _streamingHubProxy.ConnectAsync("https://localhost:44363/VideoStreamingHub").Wait();
        }

        async public Task Get(string filename)
        {
            try
            {
                var buffer = new byte[4096];

                using var video = new StreamReader(System.IO.File.Open(GetFilePath(filename), FileMode.Open, FileAccess.Read));
                //using var video = new StreamReader(System.IO.File.Open(GetFilePath(filename), FileMode.Open, FileAccess.Read), Encoding.Unicode);
                //using var video = System.IO.File.Open(GetFilePath(filename), FileMode.Open, FileAccess.Read);

                var length = (int)video.BaseStream.Length;
                var bytesRead = 1;

                while (length > 0 && bytesRead > 0)
                {
                    bytesRead = await video.BaseStream.ReadAsync(buffer, 0, Math.Min(length, buffer.Length));

                    await Response.Body.WriteAsync(buffer, 0, bytesRead);
                    await _streamingHubProxy.SendAsync(ClientStreamData(buffer, bytesRead));

                    length -= bytesRead;
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
        }

        async IAsyncEnumerable<DataChunk<string>> ClientStreamData(byte[] buffer, int bytesRead)
        {
            var data = await FetchSomeData(buffer, bytesRead);
            yield return data;
        }

        async Task<DataChunk<string>> FetchSomeData(byte[] buffer, int bytesRead)
        {
            ////var copyBuffer = new byte[bytesRead];
            ////Buffer.BlockCopy(buffer, 0, copyBuffer, 0, bytesRead);
            //return await Task.FromResult(buffer);

            var dataChunk = new DataChunk<string>
            {
                Chunked = Convert.ToBase64String(buffer, 0, buffer.Length),
                Size = bytesRead
            };

            return await Task.FromResult(dataChunk);
        }


        string GetFilePath(string fileName)
        {
            return $"DataStreaming/Video/{fileName}";
        }
    }
}
