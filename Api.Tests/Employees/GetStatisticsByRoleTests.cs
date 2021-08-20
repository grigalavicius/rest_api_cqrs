using Application.Models;
using DataStore.Models;
using FluentAssertions;
using Xunit;

namespace Api.Tests.Employees
{
    public class GetStatisticsByRoleTests : BaseTests
    {
        private const string Url = "/GetStatisticsByRole?role=";

        public GetStatisticsByRoleTests() : base(TestsConstants.EmployeesController)
        {
        }

        [Fact]
        public void GetStatisticsByRoleReturn()
        {
            MockDbContextAndRunTest(async () =>
            {
                var role = Role.Waiter;
                var employeeStatisticsByRole = await SendGetRequest<EmployeesStatisticsByRoleDto>($"{Url}{role}");

                var expectedResult = TestsMockData.ExpectedEmployeesStatisticsByRoleResult(role);

                employeeStatisticsByRole.Should().NotBeNull();
                employeeStatisticsByRole.Should().BeEquivalentTo(expectedResult);
                
            });
        }
    }
}