using Application.Commands.CreateEmployee;

namespace Application.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommand : CreateEmployeeCommand
    {
        public int Id { get; init; }
    }
}