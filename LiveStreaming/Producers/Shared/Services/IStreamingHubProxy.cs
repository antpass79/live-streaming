using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorProducer.Shared.Services
{
    public interface IStreamingHubProxy<T>
    {
        Task ConnectAsync(string endpoint);
        Task SendAsync(object data);
        IAsyncEnumerable<TData> Streaming<TData>(int delay);
    }
}
