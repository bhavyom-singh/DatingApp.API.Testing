using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DatingApp.API.Data;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace DatingApp.API.IntegrationTesting
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        private string LoginUrl = "https://localhost:44366/api/auth/login";
        private string RegisterUrl = "https://localhost:44366/api/auth/register";

        protected IntegrationTest()
        {
            var appFactory = new AppTestFixture();

            TestClient = appFactory.CreateClient();
            
        }

        protected async Task AuthenticateAsync(string username, string password)
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync(username, password));
        }

        private async Task<string> GetJwtAsync(string username, string password)
        {
            var payload = new UserForLoginDto
            {
                Username = username,
                Password = password
            };
            var response = await TestClient.PostAsJsonAsync(LoginUrl, payload);

            if (response.IsSuccessStatusCode)
            {
                var registrationResponse = await response.Content.ReadAsStringAsync();
                return registrationResponse.Split(",")[0].Split(":")[1].Trim('\"');
            }
            return string.Empty;
        }

        protected async Task<HttpResponseMessage> Register(UserForRegisterDto userForRegisterDto)
        {
            var response = await TestClient.PostAsJsonAsync(RegisterUrl, userForRegisterDto);
            var registrationResponse = await response.Content.ReadAsStringAsync();
            return response;
        }
    }

    public class AppTestFixture : WebApplicationFactory<Startup>
    {        
        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options => { 
                            options.UseInMemoryDatabase("TestDb"); 
                        });
                    });
                    webBuilder.UseStartup<Startup>().UseTestServer();
                    
                }).ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                });
            return builder;
        }
    }
}
