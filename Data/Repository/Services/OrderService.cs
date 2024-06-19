
using QuickTie.Data.Models;
using QuickTie.Portal.Data.Repository.Interface;
using QuickTie.Portal.Models;
using QuickTie.Services.Interface;
using QuickTie.Services.Services;

namespace QuickTie.Portal.Data.Repository.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMongoRepository<Order> _orderRepository;
        private readonly IMongoRepository<Product> _productRepository;
        private readonly ILocalStorageRepository _localStorageService;
        private readonly EmailService _emailService;

        public OrderService(IMongoRepository<Order> orderRepository, IMongoRepository<Product> productRepository, EmailService emailSerivce, ILocalStorageRepository localStorageService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _emailService = emailSerivce;
            _localStorageService = localStorageService;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(int skip, int take)
        {
            var orderQuery = _orderRepository.GetQueryable();
            return await _orderRepository.FindByFilterAsync(orderQuery, skip, take);
        }

        public async Task<bool> CreateOrderAsync(Cart cart)
        {
            var newOrder = new Order();

            foreach (var item in cart.Items)
            {
                var newOrderItem = new OrderItem
                {
                    Item = await _productRepository.FindByIdAsync(item.ItemId)
                };
                newOrder.OrderItems.Add(newOrderItem);
            }

            // Get email
            var userInfo = await _localStorageService.GetItemAsync<UserInfo>("userInfo");
            var contactInfo = userInfo.ContactInfo;
            newOrder.Email = contactInfo != null ? contactInfo.Email : throw new Exception("Email cannot be retireved.");

            newOrder.ShippingAddress.Address1 = contactInfo.Address;
            newOrder.ShippingAddress.City = contactInfo.City;
            newOrder.ShippingAddress.State = contactInfo.State;
            newOrder.ShippingAddress.PostalCode = contactInfo.PostalCode;

            newOrder.Contact = $"{contactInfo.FirstName} {contactInfo.LastName}";

            Order Create(long nextNum)
            {
                newOrder.Number = nextNum;
                return newOrder;
            }

            var result = await _orderRepository.GetNextNumberAndInsert(newOrder.Id, "Order_Sequence", Create);

           

            if(result != null)
            {
                var config = new EmailConfiguration();
                config.AssociatedOrder = result;
                config.ReplyToAddress = "noreply@quicktie.com";
                config.ToAddress = newOrder.Email;
                await _emailService.SendEmailAsync(config);

                // Remove items in local storage
                await _localStorageService.RemoveAllItems();
            }

            return true;
        }
    }
}