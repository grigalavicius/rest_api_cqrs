using System;
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
        public void CreateEmployeeSuccessfully()
        {
            MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                var employee = await SendPostRequest<CreateEmployeeDtoCmd, EmployeeDto>(Url, command);

                employee.Should().NotBeNull();
                employee.Id.Should().Be(11);
            });
        }
        
        [Fact]
        public void CreateEmployeeWithSameFirstAndLastNameFail()
        {
            MockDbContextAndRunTest(async () =>
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
        public void CreateEmployeeWithBossIdNullFail()
        {
            MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.BossId = null;

                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.BossIdMustBeNotNull);
            });
        }
        
        [Fact]
        public void CreateEmployeeWithCeoRoleFail()
        {
            MockDbContextAndRunTest(async () =>
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
        public void CreateEmployeeWithAgeValidationFail1()
        {
            MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.BirthDate = DateTime.Today.AddYears(-18).AddDays(1);
                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmployeeMustBeAtLeast18YearsOld);
            });
        }
        
        [Fact]
        public void CreateEmployeeWithAgeValidationFail2()
        {
            MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.BirthDate = DateTime.Today.AddYears(-70).AddDays(-1);
                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmployeeMustBeNotOlderThan70Years);
            });
        }
        
        [Fact]
        public void CreateEmployeeWithEmploymentDateValidationFail1()
        {
            MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.EmploymentDate = DateTime.Today.AddDays(1);
                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmploymentDateCannotBeEarlierThanAndCannotBeFutureDate);
            });
        }
        
        [Fact]
        public void CreateEmployeeWithEmploymentDateValidationFail2()
        {
            MockDbContextAndRunTest(async () =>
            {
                var command = TestsMockData.MockCreateEmployeeDtoCmd();
                command.EmploymentDate = new DateTime(2000,1,1);
                var error = await SendPostRequest<CreateEmployeeDtoCmd, ErrorModel>(Url, command);

                error.Should().NotBeNull();
                error.Message.Should().Contain(ValidationMessages.EmploymentDateCannotBeEarlierThanAndCannotBeFutureDate);
            });
        }
        
        [Fact]
        public void CreateEmployeeWithSalaryFail()
        {
            MockDbContextAndRunTest(async () =>
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