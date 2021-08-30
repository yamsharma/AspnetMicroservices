using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Persistence
{
	public interface ICatalogDbContext
	{
		IMongoCollection<Product> Products { get; }
	}
}
