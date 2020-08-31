using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorConsumer.Client.Services
{
    public class LocalLiveStreamingService : ILiveStreamingService
    {
        async public IAsyncEnumerable<string> GetMessagesAsync()
        {
            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(1000);
                yield return $"Message: {i}";
            }
        }
    }
}
