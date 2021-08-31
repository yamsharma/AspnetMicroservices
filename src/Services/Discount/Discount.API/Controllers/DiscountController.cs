using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class DiscountController : ControllerBase
	{
		private readonly IDiscountRepository _repository;

		public DiscountController(IDiscountRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		[HttpGet("{productName}", Name = "GetDiscount")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Coupon))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Coupon>> GetDiscount(string productName)
		{
			var coupon = await _repository.GetDiscount(productName);

			if (coupon == null)
				return NotFound();

			return Ok(coupon);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Coupon))]
		public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
		{
			await _repository.CreateDiscount(coupon);

			return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Coupon))]
		public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
		{
			return Ok(await _repository.UpdateDiscount(coupon));
		}

		[HttpDelete("{productName}", Name = "DeleteDiscount")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
		public async Task<ActionResult<bool>> DeleteDiscount(string productName)
		{
			return Ok(await _repository.DeleteDiscount(productName));
		}
	}
}
