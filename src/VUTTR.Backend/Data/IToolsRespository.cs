using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VUTTR.Backend.Models;

namespace VUTTR.Backend.Data
{
    public interface IToolsRespository
    {
        Task<List<Tool>> GetAll(CancellationToken cancellationToken = default);

        Task<List<Tool>> GetAllByTag(string tagFilter, CancellationToken cancellationToken = default);

        ValueTask<Tool> GetById(string id, CancellationToken cancellationToken);

        Task<Tool> Create(Tool data, CancellationToken cancellationToken);
    }
}
