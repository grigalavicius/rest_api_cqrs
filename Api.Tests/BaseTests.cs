using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataStore;
using DataStore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestApiTask;

namespace Api.Tests
{

    public abstract class BaseTests : IDisposable
    {
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;
        private readonly HttpClient _testServerClient;
        private readonly string _controllerName;

        protected BaseTests(string controllerName)
        {
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                throw new ArgumentException(nameof(controllerName));
            }

            _controllerName = controllerName;

            _webApplicationFactory = CreateWebApplicationFactory();
            _testServerClient = CreateTestServerClient();
        }

        public void Dispose()
        {
            _testServerClient.Dispose();
        }

        public string GetFullUrl(string url)
        {
            return _controllerName + "/" + url.TrimStart('/');
        }

        public async Task<TResult> SendGetRequest<TResult>(string url)
        {
            var message = GetHttpRequestMessageWithHeaders(HttpMethod.Get, url);
            var response = await SendRequest(message);
            return await GetResponseContent<TResult>(response) ?? default!;
        }

        public async Task<TResult> SendPostRequest<TCommand, TResult>(string url, TCommand command)
        {
            return await SendRequestWithContent<TCommand, TResult>(HttpMethod.Post, url, command);
        }

        public async Task<TResult> SendPutRequest<TCommand, TResult>(string url, TCommand command)
        {
            return await SendRequestWithContent<TCommand, TResult>(HttpMethod.Put, url, command);
        }

        public async Task<TResult> SendDeleteRequest<TResult>(string url)
        {
            var message = GetHttpRequestMessageWithHeaders(HttpMethod.Delete, url);
            var response = await SendRequest(message);
            return await GetResponseContent<TResult>(response);
        }

        public async Task<TResult> SendRequestWithContent<TCommand, TResult>(HttpMethod httpMethod, string url, TCommand command)
        {
            var json = JsonConvert.SerializeObject(command);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var message = GetHttpRequestMessageWithHeaders(httpMethod, url);
            message.Content = httpContent;

            var response = await SendRequest(message);
            var result = await GetResponseContent<TResult>(response);
            return result;
        }

        public async Task MockDbContextAndRunTest(Func<Task> action, List<Employee>? employees = null)
        {
            var selectListContext = GetDbContext();
            ClearDbContext(selectListContext);
            MockDbContext(selectListContext, employees);

            await action();
        }

        protected IEmployeesContext GetDbContext()
        {
            if (_webApplicationFactory.Server.Services.GetService(typeof(IEmployeesContext)) is not IEmployeesContext employeesDbContext)
            {
                throw new ArgumentNullException(nameof(employeesDbContext));
            }

            return employeesDbContext;
        }

        private void ClearDbContext(IEmployeesContext employeesDbContext)
        {
            employeesDbContext.Employees.RemoveRange(employeesDbContext.Employees);

            employeesDbContext.SaveChanges();
        }

        private void MockDbContext(IEmployeesContext employeesDbContext, IReadOnlyCollection<Employee>? employees)
        {
            employees ??= TestsMockData.MockEmployees();

            employeesDbContext.Employees.AddRange(employees);

            employeesDbContext.SaveChanges();
        }

        private HttpRequestMessage GetHttpRequestMessageWithHeaders(HttpMethod httpMethod, string url)
        {
            var message = new HttpRequestMessage(httpMethod, GetFullUrl(url));

            // message.Headers.Add("correlation-id", Guid.Empty.ToString("D"));
            // message.Headers.Add("caller-id", Guid.Empty.ToString("D"));

            return message;
        }

        private WebApplicationFactory<Startup> CreateWebApplicationFactory()
        {
            var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // var dataStoreConfiguration = _configuration.GetSection("DataStore").Initialize<DataStoreConfiguration>();
                    // dataStoreConfiguration.TryValidateObject();
                    // var employeesDbContext = new DbContextOptionsBuilder<EmployeesContext>()
                    //     .UseNpgsql("ApplicationName=rest_api_test;User ID=postgres;Password=root;Host=localhost;Port=5432;Database=rest_api_test_db;Pooling=true;")
                    //     .Options;

                    var employeesDbContext = new DbContextOptionsBuilder<EmployeesContext>()
                        .UseInMemoryDatabase($"EmployeesDatabase_{Guid.NewGuid()}")
                        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                        .Options;
                    
                    services.AddSingleton(employeesDbContext);
                });
            });

            return factory;
        }

        private HttpClient CreateTestServerClient()
        {
            var client = _webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            return client;
        }

        private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
        {
            var response = await _testServerClient.SendAsync(request);
            return response;
        }

        private async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(result))
            {
                return default;
            }

            if (typeof(T) == typeof(string))
            {
                return (T) (object) result;
            }

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}