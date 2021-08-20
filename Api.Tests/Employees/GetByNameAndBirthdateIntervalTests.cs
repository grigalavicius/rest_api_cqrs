using System;
using System.Collections.Generic;
using Application.Models;
using FluentAssertions;
using Xunit;

namespace Api.Tests.Employees
{
    public class GetByNameAndBirthdateIntervalTests : BaseTests
    {
        
        private const string Url = "/GetByNameAndBirthdateInterval?name={0}&from={1}&to={2}";
        
        public GetByNameAndBirthdateIntervalTests() : base(TestsConstants.EmployeesController)
        {
        }
        
        
        [Fact]
        public void GetEmployeeByNameAndBirthdateRangeReturn()
        {
            MockDbContextAndRunTest(async () =>
            {
                var name = "a";
                var from = new DateTime(1980, 1, 1);
                var to = new DateTime(2015, 1, 1);
                var employeeDto = await SendGetRequest<IReadOnlyCollection<EmployeeDto>>(string.Format(Url, name, from, to));

                var expectedEmployeeResult = TestsMockData.ExpectedEmployeesByNameAndBirthdateRangeResult(name, from, to);

                employeeDto.Should().NotBeNull();
                employeeDto.Should().BeEquivalentTo(expectedEmployeeResult);
            });
        }
    }
}