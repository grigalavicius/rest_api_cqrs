using System;
using System.Threading.Tasks;
using Api.Tests.Models;
using Application;
using Application.Models;
using DataStore.Models;
using FluentAssertions;
using RestApiTask.Commands;
using Xunit;

namespace Api.Tests.Employees
{
    public class UpdateTests : BaseTests
    {
        private const string Url = "/Update?employeeId=";
        
        public UpdateTests() : base(TestsConstants.EmployeesController)
        {
        }
        
        [Fact]
        public async Task UpdateEmployeeSuccessfully()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeeDtoCmd();
                var employee = await SendPutRequest<UpdateEmployeeDtoCmd, EmployeeDto>(Url + employeeId, command);

                employee.Should().NotBeNull();
                employee.Id.Should().Be(5);
                employee.FirstName.Should().Be(command.FirstName);
                employee.LastName.Should().Be(command.LastName);
                employee.BirthDate.Should().Be(command.BirthDate);
                employee.EmploymentDate.Should().Be(command.EmploymentDate);
                employee.BossId.Should().Be(command.BossId);
                employee.HomeAddress.Should().Be(command.HomeAddress);
                employee.Role.Should().Be(command.Role.ToString());
            });
        }
        
        [Fact]
        public async Task UpdateEmployeeDoesntExistFail()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 666;
                var command = TestsMockData.MockUpdateEmployeeDtoCmd();
                var error = await SendPutRequest<UpdateEmployeeDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(string.Format(ValidationMessages.EmployeeDoesNotExistMessage, employeeId));
            });
        }
        
        [Fact]
        public async Task UpdateEmployeeIdMustBeGreaterThanZeroFail()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = -1;
                var command = TestsMockData.MockUpdateEmployeeDtoCmd();
                var error = await SendPutRequest<UpdateEmployeeDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.IdMustBeGreaterThanZero);
            });
        }
        
        [Fact]
        public async Task UpdateEmployeeWithCeoRoleFail()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeeDtoCmd();
                command.BossId = null;
                command.Role = Role.Ceo;
                var error = await SendPutRequest<UpdateEmployeeDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmployeeWithCeoRoleAlreadyExist);
            });
        }
        
        [Fact]
        public async Task UpdateEmployeeWithFirstAndLastNameLengthFail()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeeDtoCmd();
                command.FirstName = string.Empty.PadLeft(51, 'a');
                command.LastName = string.Empty.PadLeft(51, 'b');
                var error = await SendPutRequest<UpdateEmployeeDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain("50");
            });
        }
        
        [Fact]
        public async Task UpdateEmployeeWithEqualFirstAndLastNameFail()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeeDtoCmd();
                command.LastName = command.FirstName;
                var error = await SendPutRequest<UpdateEmployeeDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.NotEqualFirstAndLastNamesValidationMessage);
            });
        }
        
        [Fact]
        public async Task UpdateEmployeeWithAgeFail1()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeeDtoCmd();
                command.BirthDate = DateTime.Today.AddYears(-18).AddDays(1);
                var error = await SendPutRequest<UpdateEmployeeDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmployeeMustBeAtLeast18YearsOld);
            });
        }
        
        [Fact]
        public async Task UpdateEmployeeWithAgeFail2()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeeDtoCmd();
                command.BirthDate = DateTime.Today.AddYears(-70).AddDays(-1);
                var error = await SendPutRequest<UpdateEmployeeDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmployeeMustBeNotOlderThan70Years);
            });
        }
        
        [Fact]
        public async Task UpdateEmployeeWithEmploymentDateValidationFail1()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeeDtoCmd();
                command.EmploymentDate = DateTime.Today.AddDays(1);
                var error = await SendPutRequest<UpdateEmployeeDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmploymentDateCannotBeEarlierThanAndCannotBeFutureDate);
            });
        }
        
        [Fact]
        public async Task UpdateEmployeeWithEmploymentDateValidationFail2()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeeDtoCmd();
                command.EmploymentDate = new DateTime(2000,1,1);
                var error = await SendPutRequest<UpdateEmployeeDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmploymentDateCannotBeEarlierThanAndCannotBeFutureDate);
            });
        }
        
        [Fact]
        public async Task UpdateEmployeeWithSalaryFail()
        {
            await MockDbContextAndRunTest(async () =>
            {
                const int employeeId = 5;
                var command = TestsMockData.MockUpdateEmployeeDtoCmd();
                command.Salary = 0;
                var error = await SendPutRequest<UpdateEmployeeDtoCmd, ErrorModel>(Url + employeeId, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.CurrentSalaryMustBeNonNegative);
            });
        }
    }
}