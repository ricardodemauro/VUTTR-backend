using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using VUTTR.Backend.Models;

namespace VUTTR.Backend.Data
{
    public class MongoToolRepository : IToolsRespository
    {
        private readonly IMongoCollection<Tool> _tools;

        public MongoToolRepository(IMongoCollection<Tool> tools)
        {
            _tools = tools ?? throw new ArgumentNullException(nameof(tools));
        }

        public async Task<Tool> Create(Tool data, CancellationToken cancellationToken)
        {
            await _tools.InsertOneAsync(data, cancellationToken: cancellationToken);

            return data;
        }

        public async Task<List<Tool>> GetAll(CancellationToken cancellationToken = default)
        {
            var documents = await _tools.Find(_ => true).ToListAsync();
            return documents;
        }

        public async Task<List<Tool>> GetAllByTag(string tagFilter, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Tool>.Filter.AnyIn(x => x.Tags, new[] { tagFilter });
            var documents = await _tools.Find(filter).ToListAsync();
            return documents;
        }

        public async ValueTask<Tool> GetById(string id, CancellationToken cancellationToken)
        {
            var queryId = Builders<Tool>.Filter.Eq(x => x.Id, id);
            var tool = await _tools.Find(queryId).FirstOrDefaultAsync(cancellationToken);
            return tool;
        }
    }
}
