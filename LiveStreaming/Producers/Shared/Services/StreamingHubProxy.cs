using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorProducer.Shared.Services
{
    public class StreamingHubProxy<T> : IStreamingHubProxy<T>
    {
        #region Data Members

        public HubConnection _connection;

        #endregion

        #region Public Functions

        async public Task ConnectAsync(string endpoint)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(endpoint)
                //.AddMessagePackProtocol()
                .Build();
            await _connection.StartAsync();
        }

        async virtual public Task SendAsync(object data)
        {
            await _connection.SendAsync("UploadStream", data);
        }

        public IAsyncEnumerable<TData> Streaming<TData>(int delay)
        {
            return _connection.StreamAsync<TData>("Streaming", delay);
        }

        #endregion
    }
}
