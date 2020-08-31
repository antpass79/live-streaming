using System.Collections.Generic;

namespace BlazorConsumer.Client.Services
{
    public interface ILiveStreamingService
    {
        IAsyncEnumerable<string> GetMessagesAsync();
    }
}
