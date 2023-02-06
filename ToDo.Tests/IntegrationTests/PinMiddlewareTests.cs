using AutoMapper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Todo.Api;
using Todo.Core.Models;
using Xunit;

namespace ToDo.Tests.IntegrationTests
{
    public class PinMiddlewareTests
    {
        [Fact]
        public async Task CreatingStoryWithoutPin_ShouldReturnUnauthorized()
        {
            // Arrange
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    // Add TestServer
                    webHost.UseTestServer();
                    webHost.UseStartup<Startup>();
                });

            // Create and start up the host
            var host = await hostBuilder.StartAsync();

            // Create an HttpClient which is setup for the test host
            var client = host.GetTestClient();
            var jsonStory = JsonConvert.SerializeObject(new StoryDto { Id = 1,Name = "test" });
            var content = new StringContent(jsonStory, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("api/stories", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        }
    }
}
