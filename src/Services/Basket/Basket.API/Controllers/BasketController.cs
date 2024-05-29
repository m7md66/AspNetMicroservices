using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketRepository repository
            , DiscountGrpcService discountGrpcService,
            IPublishEndpoint publishEndpoint,
            IMapper mapper,
            ILogger<BasketController> logger 
            )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException( nameof(logger));
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // TODO : Communicate with Discount.Grpc
            // and Calculate latest prices of product into shopping cart
            // consume Discount Grpc


            foreach (var item in basket.Items)
            {
                _logger.LogError($"Product with id >>>>>>>>>>><<<<<<<<<<<<<, not found.");
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                _logger.LogError($"Product with id >>>>>>>>>>><<<<<<<<<<<<<{coupon}, not found.");
                item.Price -= coupon.Amount;
            }

            return Ok(await _repository.UpdateBasket(basket));

//            {
//                "userName": "swn",
//  "items": [
//    {
//                    "quantity": 3,
//      "color": "string",
//      "price": 333,
//      "productId": "602d2149e773f2a3990b47f6",
//      "productName": "Samsung 10"
//    }
//  ]
//}
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            //    // get existing basket with total price 
            //    // Create basketCheckoutEvent -- Set TotalPrice on basketCheckout eventMessage
            //    // send checkout event to rabbitmq
            //    // remove the basket

            //    //1- get existing basket with total price
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            //    //2- send checkout event to rabbitmq
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            //    //3- remove the basket
            await _repository.DeleteBasket(basket.UserName);

            return Accepted();
        }
}
}
