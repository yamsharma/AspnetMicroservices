using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class CatalogController : ControllerBase
	{
		private readonly IProductRepository _repository;
		private readonly ILogger<CatalogController> _logger;

		public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
		{
			_repository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _repository.GetProducts();

			if (products == null || !products.Any())
			{
				_logger.LogError($"No products found.");
				return NotFound();
			}

			return Ok(products);
		}

		[HttpGet("{id:length(24)}", Name = "GetProduct")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Product>> GetProduct(string id)
		{
			var product = await _repository.GetProduct(id);

			if (product == null)
			{
				_logger.LogError($"Product with id: {id} not found.");
				return NotFound();
			}

			return Ok(product);
		}

		[Route("[action]/{name}")]
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Product>> GetProductsByName(string name)
		{
			var products = await _repository.GetProductsByName(name);

			if (products == null || !products.Any())
			{
				_logger.LogError($"Products with name: {name} not found.");
				return NotFound();
			}

			return Ok(products);
		}

		[Route("[action]/{category}")]
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Product>> GetProductsByCategory(string category)
		{
			var products = await _repository.GetProductsByCategory(category);

			if (products == null || !products.Any())
			{
				_logger.LogError($"Products with category: {category} not found.");
				return NotFound();
			}

			return Ok(products);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
		public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
		{
			await _repository.CreateProduct(product);

			return Ok(CreatedAtRoute("GetProduct", new { id = product.Id }, product));
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
		public async Task<IActionResult> UpdateProduct([FromBody] Product product)
		{
			return Ok(await _repository.UpdateProduct(product));
		}

		[HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
		public async Task<IActionResult> DeleteProduct(string id)
		{
			return Ok(await _repository.DeleteProduct(id));
		}
	}
}
