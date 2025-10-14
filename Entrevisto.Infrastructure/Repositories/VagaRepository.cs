
using Entrevisto.Domain.Entities;
using Entrevisto.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Entrevisto.Infrastructure.Repositories
{
    public class VagaRepository : IVagaRepository
    {
        private readonly IMongoCollection<Vaga> _vagasCollection;

        public VagaRepository(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDbSettings:ConnectionString"];
            var databaseName = configuration["MongoDbSettings:DatabaseName"];
            var collectionName = configuration["MongoDbSettings:CollectionName"];

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _vagasCollection = database.GetCollection<Vaga>(collectionName);
        }

        public async Task<Vaga> AddAsync(Vaga vaga)
        {
            await _vagasCollection.InsertOneAsync(vaga);
            return vaga;
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var result = await _vagasCollection.DeleteOneAsync(v => v.Id == id && v.UserId == userId);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Vaga>> GetAllByUserIdAsync(string userId)
        {
            return await _vagasCollection.Find(v => v.UserId == userId).ToListAsync();
        }

        public async Task<Vaga> GetByIdAsync(string id, string userId)
        {
            return await _vagasCollection.Find(v => v.Id == id && v.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(string id, Vaga vaga)
        {
            var result = await _vagasCollection.ReplaceOneAsync(v => v.Id == id && v.UserId == vaga.UserId, vaga);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
