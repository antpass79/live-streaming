using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using BlazorProducer.Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorProducer.Client.Pages
{
    public class TextStreamingProducerDataModel : ComponentBase
    {
        #region Data Members

        private Random _random = new Random();

        #endregion

        #region Properties

        [Inject]
        protected IStreamingHubProxy<string> StreamingHubProxy { get; set; }

        #endregion

        #region Protected Functions

        async protected override Task OnInitializedAsync()
        {
            await StreamingHubProxy.ConnectAsync("https://localhost:44363/TextStreamingHub");

            await Task.CompletedTask;
        }

        protected async Task OnSendMessages()
        {
            await StreamingHubProxy.SendAsync(ClientStreamData());

            await Task.CompletedTask;
        }

        #endregion

        #region Private Functions

        async IAsyncEnumerable<string> ClientStreamData()
        {
            for (var i = 0; i < 5; i++)
            {
                await Task.Delay(1000);
                var data = await FetchSomeData();
                yield return data;
            }
        }

        async Task<string> FetchSomeData()
        {
            return await Task.FromResult($"Message at {DateTime.Now}\n");
        }

        #endregion
    }
}
