namespace Application
{
    public static class ValidationMessages
    {
        public const string NotEqualFirstAndLastNamesValidationMessage = "FirstName can not coincide with the LastName";
        public const string EmployeeMustBeAtLeast18YearsOld = "Employee must be at least 18 years old";
        public const string EmployeeMustBeNotOlderThan70Years = "Employee must be not older than 70 years";
        public const string EmploymentDateCannotBeEarlierThanAndCannotBeFutureDate = "Employment date cannot be earlier than 2000-01-01 and cannot be future date";
        public const string CurrentSalaryMustBeNonNegative = "Current salary must be non-negative";
        public const string BossIdMustBeNull = "BossId must be null";
        public const string BossIdMustBeNotNull = "BossId must be not null";
        public const string IdMustBeGreaterThanZero = "Id must be greater than zero";
        public const string DateFromMustBeLessOrEqualToDateTo = "Date From must be less or equal to date To";
        public const string EmployeeWithCeoRoleAlreadyExist = "Employee with CEO role already exist";
        public const string EmployeeDoesNotExistMessage = "Could not find employee by id: {0}";
    }
}