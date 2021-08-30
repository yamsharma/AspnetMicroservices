﻿using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDistributedCache _redisCache;

		public BasketRepository(IDistributedCache redisCache)
		{
			_redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
		}

		public async Task<ShoppingCart> GetBasket(string username)
		{
			var basket = await _redisCache.GetStringAsync(username);

			if (string.IsNullOrEmpty(basket))
				return null;

			return JsonConvert.DeserializeObject<ShoppingCart>(basket);
		}

		public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
		{
			await _redisCache.SetStringAsync(basket.Username, JsonConvert.SerializeObject(basket));

			return await GetBasket(basket.Username);
		}

		public Task DeleteBasket(string username)
		{
			return _redisCache.RemoveAsync(username);
		}
	}
}
