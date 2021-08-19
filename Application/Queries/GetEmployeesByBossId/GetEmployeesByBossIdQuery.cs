using System.Collections.Generic;
using Application.Models;
using MediatR;

namespace Application.Queries.GetEmployeesByBossId
{
    public class GetEmployeesByBossIdQuery: IRequest<IReadOnlyCollection<EmployeeDto>>
    {
        public GetEmployeesByBossIdQuery(int? bossId)
        {
            BossId = bossId;
        }

        public int? BossId { get; }
    }
}