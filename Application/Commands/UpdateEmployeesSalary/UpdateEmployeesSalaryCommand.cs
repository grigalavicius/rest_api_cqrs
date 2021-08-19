using Application.Models;
using MediatR;

namespace Application.Commands.UpdateEmployeesSalary
{
    public class UpdateEmployeesSalaryCommand: IRequest<SuccessfullyExecutedModel>
    {
        public UpdateEmployeesSalaryCommand(int id, decimal salary)
        {
            Id = id;
            Salary = salary;
        }

        public int Id { get; }
        public decimal Salary { get; }
    }
}