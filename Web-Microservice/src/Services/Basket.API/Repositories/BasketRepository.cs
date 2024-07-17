using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Contracts.Commons.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Ilogger = Serilog.ILogger;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;
        private readonly ISerializeService _serializeService;
        private readonly Ilogger _logger;
        public BasketRepository(IDistributedCache redisCache, ISerializeService serializeService, Ilogger logger)
        {
            _redisCache = redisCache;
            _serializeService = serializeService;
            _logger = logger;
        }
        public async Task<bool> DeleteBasketFromUsername(string userName)
        {
            try
            {
                await _redisCache.RemoveAsync(userName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("DeleteBasketFromUsername: " + ex);
                return false;
            }
            
        }

        public async Task<Cart?> GetBasketByUsername(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            return string.IsNullOrEmpty(basket) ? null : _serializeService.Deserialize<Cart>(basket);
        }

        public async Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
        {
            if (options != null)
            {
                await _redisCache.SetStringAsync(cart.UserName, _serializeService.Serialize(cart), options);
            }
            else
            {
                await _redisCache.SetStringAsync(cart.UserName, _serializeService.Serialize(cart));
            }
            return await GetBasketByUsername(cart.UserName);
        }
    }
}
