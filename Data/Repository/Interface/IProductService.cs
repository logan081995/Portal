using QuickTie.Data.Models;
using QuickTie.Portal.Models;

namespace QuickTie.Portal.Data.Repository.Interface
{
    public interface IProductService
    {
        Task<(IEnumerable<Product>, long)> GetProductsAsync(int skip, int take, string searchValue , FilterParameter? filterParameter, ProductType? productType);
        Task<Dictionary<ProductType, ProductTypeInfo>> GetCategoriesAsync(string searchValue, FilterParameter filterParameter);
        Task<Product> GetProductByIdAsync(string Id);
        IEnumerable<ProductType> GetCategoryList();
        Task<bool> AddProductImages(string productId, List<string> imageUrls);
        Task<bool> DeleteProductImages(string productId, List<ProductImage> images);
        Task<bool> UpdateProductAsync(Product product);
    }
}