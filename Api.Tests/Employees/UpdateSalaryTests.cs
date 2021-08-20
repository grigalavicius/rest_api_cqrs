using Api.Tests.Models;
using Application;
using Application.Models;
using FluentAssertions;
using RestApiTask.Commands;
using Xunit;

namespace Api.Tests.Employees
{
    public class UpdateSalaryTests : BaseTests
    {
        private const string Url = "/UpdateSalary?employeeId=";
        
        public UpdateSalaryTests() : base(TestsConstants.EmployeesController)
        {
        }
        
        [Fact]
        public void UpdateEmployeesSalarySuccessfully()
        {
            MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeesSalaryDtoCmd((decimal)1111.1);
                var response = await SendPutRequest<UpdateEmployeeSalaryDtoCmd, SuccessfullyExecutedModel>(Url + employeeId, command);

                response.Should().NotBeNull();
                response.ExecutedSuccessfully.Should().BeTrue();
            });
        }
        
        [Fact]
        public void UpdateEmployeesSalaryWithNegativeNumberFail()
        {
            MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeesSalaryDtoCmd(-10);
                var error = await SendPutRequest<UpdateEmployeeSalaryDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull(); 
                error.Message.Should().Contain(ValidationMessages.CurrentSalaryMustBeNonNegative);
            });
        }
        
        [Fact]
        public void UpdateEmployeesSalaryWithZeroFail()
        {
            MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeesSalaryDtoCmd(0);
                var error = await SendPutRequest<UpdateEmployeeSalaryDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull(); 
                error.Message.Should().Contain(ValidationMessages.CurrentSalaryMustBeNonNegative);
            });
        }
    }
}