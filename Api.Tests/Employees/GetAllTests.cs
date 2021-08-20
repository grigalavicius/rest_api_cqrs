using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models;
using FluentAssertions;
using Xunit;

namespace Api.Tests.Employees
{
    public class GetAllTests : BaseTests
    {
        private const string Url = "/GetAll";

        public GetAllTests() : base(TestsConstants.EmployeesController)
        {
        }
        
        [Fact]
        public async Task GetAllEmployeesReturn()
        {
            await MockDbContextAndRunTest(async () =>
            {
                var employees = await SendGetRequest<IReadOnlyCollection<EmployeeDto>>(Url);

                var expectedEmployeesResult = TestsMockData.ExpectedEmployeesResult(new List<int> { 1,2,3,4,5,6,7,8,9,10 });

                employees.Should().NotBeNull();
                employees.Should().BeEquivalentTo(expectedEmployeesResult);
            });
        }
    }
}