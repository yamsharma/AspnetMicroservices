using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Persistence
{
	public class CatalogDbContext : ICatalogDbContext
	{
		public IMongoCollection<Product> Products { get; }

		public CatalogDbContext(IConfiguration configuration)
		{
			var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
			var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

			Products = database.GetCollection<Product>(
				configuration.GetValue<string>("DatabaseSettings:CollectionName"));

			CatalogDbContextSeed.SeedData(Products);
		}
	}
}
