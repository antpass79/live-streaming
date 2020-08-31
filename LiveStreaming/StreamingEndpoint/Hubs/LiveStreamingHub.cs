using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace StreamingEndpoint.Hubs
{
    public class LiveStreamingHub<T> : Hub
    {
        static ConcurrentQueue<T> _stream = new ConcurrentQueue<T>();

        public async Task UploadStream(IAsyncEnumerable<T> stream)
        {            
            await foreach (var item in stream)
            {
                _stream.Enqueue(item);
                await Clients.All.SendAsync("ReceiveStream", item);
            }
        }

        public async IAsyncEnumerable<T> Streaming(int delay, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            while (true)
            {
                // Check the cancellation token regularly so that the server will stop
                // producing items if the client disconnects.
                cancellationToken.ThrowIfCancellationRequested();

                T data;
                if (!_stream.IsEmpty && _stream.TryDequeue(out data))
                    yield return data;

                // Use the cancellationToken in other APIs that accept cancellation
                // tokens so the cancellation can flow down to them.
                await Task.Delay(delay, cancellationToken);
            }
        }
    }
}
