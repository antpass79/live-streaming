# Live Streaming
Streaming with Asp Net Core and SignalR

## Repository Actors

There are 2 folders inside the repository:

- Producers: there are the streaming producers. Now only a Blazor WebAssembly application.
- Consumers: there are streaming consumers. Now only a working progress Blazor WebAssembly application.

All projects are listed below:

- BlazorProducer: the Blazor WebAssembly application to generate different live data streaming (text, image, video)
- BlazorConsumer: the Blazor WebAssembly application to listen and show live data streaming (text, image, video). It's a working progress project.
- StreamingEndpoint: it provides different live streaming endpoints.

## Architecture

The below schema shows the communication among the actors.

![Live Streaming Class Diagram](assets/LiveStreamingClassDiagram.jpg)

## Run the System

In order to play with the streaming, it's necessary to run the following projects:

- StreamingEndpoint
- BlazorProducer.Server
- BlazorConsumer.Server

For the moment the consumer doesn't work, so you need third-party component to listen for live streaming.

### Text Streaming

The producer sends 5 messages every seconds for each button click.

Using *cUrl*, you have to type:

    curl https://localhost:44363/textstreaming

Using the browser, you have to put the below url:

    https://localhost:44363/textstreaming

### Image Streaming

Using *cUrl*, you have to type:

    curl https://localhost:44363/imagestreaming --output <<path_to_save_binary_data>>
    
    The output result is a concatenation of images (in the example, producer sends three images in jpg format)

### Video Streaming

Using *cUrl*, you have to type:

    curl https://localhost:44363/videostreaming --output <<path_to_save_binary_data>>

    The output result is a video in mp4 format

Using VLC Media Player, you have to put in the url for reproducing:

    https://localhost:44363/videostreaming 

## References

### SignalR

- <https://docs.microsoft.com/en-us/aspnet/core/signalr/streaming?view=aspnetcore-3.1#set-up-a-hub-for-streaming>
- <https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr-blazor-webassembly?view=aspnetcore-3.1&tabs=visual-studio>
- <https://www.strathweb.com/2013/01/asynchronously-streaming-video-with-asp-net-web-api/>
- <https://blogs.ibs.com/2017/01/24/streaming-video-content-to-a-browser-using-web-api/>
- <https://www.tpeczek.com/2017/02/server-sent-events-sse-support-for.html>
- <http://www.binaryintellect.net/articles/a77ac135-c756-4ee0-9e99-0a904959de94.aspx>
- <https://csharp.christiannagel.com/2019/10/08/signalrstreaming/>
- <https://www.aspitalia.com/script/1328/Consumare-Dati-Binari-Realtime-Lato-Client-ASP.NET-Core-SignalR.aspx>

### WebSocket

- <https://ikriv.com/blog/?p=4643>
- <https://github.com/ikriv-samples/WebSocketDemo/>
- <https://peterdaugaardrasmussen.com/2019/01/20/how-to-use-websockets-with-asp-net-core-with-an-example/>
- <https://radu-matei.com/blog/aspnet-core-websockets-middleware/>
- <https://techblog.dorogin.com/server-sent-event-aspnet-core-a42dc9b9ffa9>
- <https://www.aspitalia.com/script/1168/Streaming-Contenuti-ASP.NET-Web-API.aspx>

- <https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AspNetCoreGuidance.md#do-not-store-ihttpcontextaccessorhttpcontext-in-a-field>
- <https://blog.j2i.net/2020/05/09/pushing-data-over-a-live-connection/>
- <https://github.com/dotnet/aspnetcore/issues/12883>
- <https://codez.deedx.cz/posts/continuous-speech-to-text/>
- <https://www.c-sharpcorner.com/article/real-time-baby-monitor-chrome-extension-streaming-from-raspberry-pi-using-sign/>