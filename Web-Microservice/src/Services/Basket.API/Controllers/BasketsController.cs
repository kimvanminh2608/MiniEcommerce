using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        public BasketsController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet("username", Name = "GetBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetBasketByUsername([Required] string username)
        {
            var result =  await _basketRepository.GetBasketByUsername(username);
            return Ok(result ?? new Cart());
        }

        [HttpPost(Name = "UpdateBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart cart)
        {
            var option = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1))
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            var result = await _basketRepository.UpdateBasket(cart, option);
            return Ok(result);
        }

        [HttpDelete(Name = "DeleteBasket")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteBasket([Required] string username)
        {
            var result = await _basketRepository.DeleteBasketFromUsername(username);
            return Ok(result);
        }
    }
}
