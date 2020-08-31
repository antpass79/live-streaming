using Microsoft.AspNetCore.Http;
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
    public class ImageDownloadController : ControllerBase
    {
        private readonly ILogger<ImageDownloadController> _logger;
        private readonly IStreamingHubProxy<byte[]> _streamingHubProxy;

        public ImageDownloadController(
            ILogger<ImageDownloadController> logger,
            IStreamingHubProxy<byte[]> streamingHubProxy)
        {
            _logger = logger;
            _streamingHubProxy = streamingHubProxy;

            _streamingHubProxy.ConnectAsync("https://localhost:44363/ImageStreamingHub").Wait();
        }

        async public Task Get()
        {
            Response.ContentType = "image/jpg";

            try
            {
                var files = Directory.GetFiles(@"DataStreaming/Images");
                foreach (var file in files)
                {
                    var buffer = System.IO.File.ReadAllBytes(file);

                    await Response.Body.WriteAsync(buffer, 0, buffer.Length);
                    await WriteSeparatorAsync();
                    await Response.Body.FlushAsync();

                    await Task.Delay(1000);

                    await _streamingHubProxy.SendAsync(ClientStreamData(buffer, buffer.Length));
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

        async Task WriteSeparatorAsync()
        {
            string separator = "#####";
            byte[] bytes = Encoding.UTF8.GetBytes(separator);
            await Response.WriteAsync(separator);

            //await Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }

        async IAsyncEnumerable<byte[]> ClientStreamData(byte[] buffer, int bytesRead)
        {
            var data = await FetchSomeData(buffer, bytesRead);
            yield return data;
        }

        async Task<byte[]> FetchSomeData(byte[] buffer, int bytesRead)
        {
            var copyBuffer = new byte[bytesRead];
            Buffer.BlockCopy(buffer, 0, copyBuffer, 0, bytesRead);
            return await Task.FromResult(copyBuffer);

            //var dataChunk = new DataChunk
            //{
            //    Buffer = buffer,
            //    Size = bytesRead
            //};

            //string value = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);

            //return await Task.FromResult(dataChunk);
        }
    }
}
