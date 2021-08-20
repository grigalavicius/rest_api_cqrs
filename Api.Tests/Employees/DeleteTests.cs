using System.Threading.Tasks;
using Application.Models;
using FluentAssertions;
using Xunit;

namespace Api.Tests.Employees
{
    public class DeleteTests : BaseTests
    {
        private const string Url = "/Delete/";
        
        public DeleteTests() : base(TestsConstants.EmployeesController)
        {
        }
        
        [Fact]
        public async Task DeleteEmployeesSuccessfully()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var response = await SendDeleteRequest<SuccessfullyExecutedModel>(Url + employeeId);

                response.Should().NotBeNull();
                response.ExecutedSuccessfully.Should().BeTrue();
            });
        }
    }
}