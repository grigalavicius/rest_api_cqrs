using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using FluentAssertions;
using Xunit;

namespace Api.Tests.Employees
{
    public class GetEmployeeByIdTests : BaseTests
    {
        private const string Url = "/GetById?id=";

        public GetEmployeeByIdTests() : base(TestsConstants.EmployeesController)
        {
        }

        [Fact]
        public async Task GetEmployeeByIdReturn()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 6;
                var employeeDto = await SendGetRequest<EmployeeDto>($"{Url}{employeeId}");

                var expectedEmployeeResult = TestsMockData.ExpectedEmployeesResult(new List<int> { employeeId });

                employeeDto.Should().NotBeNull();
                employeeDto.Should().BeEquivalentTo(expectedEmployeeResult.FirstOrDefault());
            });
        }
    }
}