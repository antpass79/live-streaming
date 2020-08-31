using BlazorConsumer.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorConsumer.Client.Pages
{
    public class TextStreamingConsumerDataModel : ComponentBase
    {
        #region Properties

        protected List<string> Messages { get; } = new List<string>();

        [Inject]
        protected ILiveStreamingService LiveStreamingService { get; set; }

        #endregion

        #region Protected Functions

        async protected override Task OnInitializedAsync()
        {
            //await foreach (var message in LiveStreamingService.GetMessagesAsync())
            //{
            //    Messages.Add(message);
            //    this.StateHasChanged();
            //}

            await Task.CompletedTask;
        }

        #endregion
    }
}
