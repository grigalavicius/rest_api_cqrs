using System.Collections.Generic;
using Application.Models;
using FluentAssertions;
using Xunit;

namespace Api.Tests.Employees
{
    public class GetEmployeesByBossIdTests : BaseTests
    {
        
        private const string Url = "/GetByBossId?bossId=";
        
        public GetEmployeesByBossIdTests() : base(TestsConstants.EmployeesController)
        {
        }
        
        [Fact]
        public void GetEmployeesByBossIdReturn()
        {
            MockDbContextAndRunTest(async () =>
            {
                var bossId = 1;
                var employees = await SendGetRequest<IReadOnlyCollection<EmployeeDto>>($"{Url}{bossId}");

                var expectedEmployeesResult = TestsMockData.ExpectedEmployeesByBossIdResult(bossId);

                employees.Should().NotBeNull();
                employees.Should().BeEquivalentTo(expectedEmployeesResult);
            });
        }
    }
}