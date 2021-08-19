using Application.Models;
using MediatR;

namespace Application.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommand : IRequest<SuccessfullyExecutedModel>
    {
        public DeleteEmployeeCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}