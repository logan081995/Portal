using QuickTie.Data.Models;
using QuickTie.Portal.Models;

namespace QuickTie.Portal.Data.Repository.Interface
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync(int skip, int take);
        Task<bool> CreateOrderAsync(Cart cart);
    }
}
