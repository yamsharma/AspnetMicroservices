using Catalog.API.Entities;
using Catalog.API.Persistence;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ICatalogDbContext _context;

		public ProductRepository(ICatalogDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IEnumerable<Product>> GetProducts()
		{
			return await _context.Products
				.Find(product => true)
				.ToListAsync();
		}

		public async Task<Product> GetProduct(string id)
		{
			return await _context.Products
				.Find(product => product.Id == id)
				.SingleOrDefaultAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByName(string name)
		{
			return await _context.Products
				.Find(product => product.Name == name)
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
		{
			return await _context.Products
				.Find(product => product.Category == category)
				.ToListAsync();
		}

		public async Task CreateProduct(Product product)
		{
			await _context.Products.InsertOneAsync(product);
		}

		public async Task<bool> UpdateProduct(Product product)
		{
			var updateResult = await _context.Products
				.ReplaceOneAsync(p => p.Id == product.Id, product);

			return updateResult.IsAcknowledged && updateResult.ModifiedCount == 1;
		}

		public async Task<bool> DeleteProduct(string id)
		{
			var deleteResult = await _context.Products
				.DeleteOneAsync(product => product.Id == id);

			return deleteResult.IsAcknowledged && deleteResult.DeletedCount == 1;
		}
	}
}
