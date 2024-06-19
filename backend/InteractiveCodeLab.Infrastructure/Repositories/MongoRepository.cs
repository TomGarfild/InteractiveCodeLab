using InteractiveCodeLab.Infrastructure.Attributes;
using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace InteractiveCodeLab.Infrastructure.Repositories;

public class MongoRepository<TDocument> : IRepository<TDocument> where TDocument : IData
{
    private readonly IMongoCollection<TDocument> _collection;

    public MongoRepository(IMongoDatabase mongoDatabase)
    {
        _collection = mongoDatabase.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
    }

    private protected string? GetCollectionName(Type documentType)
    {
        return ((MongoCollectionAttribute?)documentType.GetCustomAttributes(
                typeof(MongoCollectionAttribute),
                true)
            .FirstOrDefault())?.CollectionName;
    }

    public async Task<IList<TDocument>> GetAll()
    {
        var res = await _collection.FindAsync(_ => true);
        return res.ToList();
    }

    public async Task<TDocument> GetById(string id)
    {
        var res = await _collection.FindAsync(document => document.Id == id);
        return res.FirstOrDefault();
    }

    public async Task Upsert(TDocument document)
    {
        // TODO: Version increment
        await _collection.ReplaceOneAsync(doc => doc.Id == document.Id, document, new ReplaceOptions() {IsUpsert = true});
    }

    public async Task<IList<TDocument>> Get(ISpecification<TDocument> spec)
    {
        var query = _collection.AsQueryable().Where(spec.Build());
        return await query.ToListAsync();
    }

    public async Task<TDocument?> GetOne(ISpecification<TDocument> spec)
    {
        var res = await Get(spec);
        return res.FirstOrDefault();
    }
}