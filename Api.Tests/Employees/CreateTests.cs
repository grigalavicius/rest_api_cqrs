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
    public class CreateTests: BaseTests
    {
        private const string Url = "/Create";
        
        public CreateTests() : base(TestsConstants.EmployeesController)
        {
        }

        [Fact]
        public async Task CreateEmployeeSuccessfully()
        {
            await MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                var employee = await SendPostRequest<CreateEmployeeDtoCmd, EmployeeDto>(Url, command);

                employee.Should().NotBeNull();
                employee.Id.Should().Be(11);
            });
        }
        
        [Fact]
        public async Task CreateEmployeeWithSameFirstAndLastNameFail()
        {
            await MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.FirstName = "name";
                command.LastName = "name";

                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.NotEqualFirstAndLastNamesValidationMessage);
            });
        }
        
        [Fact]
        public async Task CreateEmployeeWithBossIdNullFail()
        {
            await MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.BossId = null;

                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.BossIdMustBeNotNull);
            });
        }
        
        [Fact]
        public async Task CreateEmployeeWithCeoRoleFail()
        {
            await MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.BossId = null;
                command.Role = Role.Ceo;

                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmployeeWithCeoRoleAlreadyExist);
            });
        }
        
        [Fact]
        public async Task CreateEmployeeWithAgeValidationFail1()
        {
            await MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.BirthDate = DateTime.Today.AddYears(-18).AddDays(1);
                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmployeeMustBeAtLeast18YearsOld);
            });
        }
        
        [Fact]
        public async Task CreateEmployeeWithAgeValidationFail2()
        {
            await MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.BirthDate = DateTime.Today.AddYears(-70).AddDays(-1);
                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmployeeMustBeNotOlderThan70Years);
            });
        }
        
        [Fact]
        public async Task CreateEmployeeWithEmploymentDateValidationFail1()
        {
            await MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.EmploymentDate = DateTime.Today.AddDays(1);
                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmploymentDateCannotBeEarlierThanAndCannotBeFutureDate);
            });
        }
        
        [Fact]
        public async Task CreateEmployeeWithEmploymentDateValidationFail2()
        {
            await MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.EmploymentDate = new DateTime(2000,1,1);
                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmploymentDateCannotBeEarlierThanAndCannotBeFutureDate);
            });
        }
        
        [Fact]
        public async Task CreateEmployeeWithSalaryFail()
        {
            await MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.Salary = 0;
                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.CurrentSalaryMustBeNonNegative);
            });
        }
    }
}