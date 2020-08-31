using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StreamingEndpoint.Hubs;
using BlazorProducer.Shared.Models;
using System.Collections.Generic;

namespace StreamingEndpoint
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSignalR();
                //.AddMessagePackProtocol();
                //.AddMessagePackProtocol(options =>
                //     {
                //         options.FormatterResolvers = new List<MessagePack.IFormatterResolver>()
                //            {
                //                MessagePack.Resolvers.UnsafeBinaryResolver.Instance
                //            };
                //     });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<LiveStreamingHub<string>>("/TextStreamingHub");
                endpoints.MapHub<LiveStreamingHub<byte[]>>("/ImageStreamingHub");
                endpoints.MapHub<LiveStreamingHub<DataChunk<string>>>("/VideoStreamingHub");

                endpoints.MapControllers();
            });
        }
    }
}
