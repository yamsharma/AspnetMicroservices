using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class BasketController : ControllerBase
	{
		private readonly IBasketRepository _repository;

		public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger)
		{
			_repository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
		}

		[HttpGet("{username}", Name = "GetBasket")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
		public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
		{
			var basket = await _repository.GetBasket(username);

			return Ok(basket ?? new ShoppingCart(username));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
		public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
		{
			return Ok(await _repository.UpdateBasket(basket));
		}

		[HttpDelete("{username}", Name = "DeleteBasket")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
		public async Task<IActionResult> DeleteBasket(string username)
		{
			await _repository.DeleteBasket(username);

			return Ok();
		}

		// [Route("[action]")]
		// [HttpPost]
		// [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
		// public async Task<ActionResult<Product>> Checkout([FromBody] Product product)
		// {
		// 	await _repository.CreateProduct(product);
		//
		// 	return Ok(CreatedAtRoute("GetProduct", new { id = product.Id }, product));
		// }
	}
}
