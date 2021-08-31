using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
	public class DiscountRepository : IDiscountRepository
	{
		private readonly IConfiguration _configuration;

		public DiscountRepository(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task<Coupon> GetDiscount(string productName)
		{
			await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM coupon WHERE productname = @productName", new { productName });

			if (coupon == null)
			{
				return new Coupon
				{
					ProductName = "No Discount",
					Amount = 0,
					Description = "No Discount Description"
				};
			}

			return coupon;
		}

		public async Task<bool> CreateDiscount(Coupon coupon)
		{
			await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected = await connection.ExecuteAsync(
				"INSERT INTO coupon (productname, description, amount) VALUES (@productName, @description, @amount)", new
				{
					coupon.ProductName,
					coupon.Description,
					coupon.Amount
				});

			return affected != 0;
		}

		public async Task<bool> UpdateDiscount(Coupon coupon)
		{
			await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected = await connection.ExecuteAsync(
				"UPDATE coupon SET productname = @productName, description = @description, amount = @amount WHERE id = @id", new
				{
					coupon.ProductName,
					coupon.Description,
					coupon.Amount,
					coupon.Id
				});

			return affected != 0;
		}

		public async Task<bool> DeleteDiscount(string productName)
		{
			await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected = await connection.ExecuteAsync(
				"DELETE FROM coupon WHERE productname = @productName", new { productName });

			return affected != 0;
		}
	}
}
