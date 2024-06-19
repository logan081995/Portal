using MongoDB.Driver.Linq;
using QuickTie.Data.Models;
using QuickTie.Portal.Data.Repository.Interface;
using QuickTie.Portal.Extensions;
using QuickTie.Portal.Models;
using QuickTie.Portal.Models.EqualityComparer;
using QuickTie.Services.Interface;
using QuickTie.Cloud.Helpers;


namespace QuickTie.Portal.Data.Repository.Services
{
    public class ProductService : IProductService
    {
        private readonly IMongoRepository<Product> _productsRepository;

        public ProductService(IMongoRepository<Product> productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<(IEnumerable<Product>, long)> GetProductsAsync(int skip = 0, int take = 0, string searchValue = "", FilterParameter? filterParameter = null, ProductType? productType = null)
        {
            var productQuery = _productsRepository.GetQueryable();

            if (productType != null)
            {
                productQuery = productQuery.Where(p => p.ProductType == productType);
            } else
            {
                //productQuery = productQuery.Where(p => p.ProductType != ProductType.QuickTieCable);
            }
            // Filter by search value
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                productQuery = productQuery.Where(p =>
                    p.Name.Contains(searchValue,StringComparison.CurrentCultureIgnoreCase) ||
                    p.ReferenceNumbers.Any(rn => rn.Name.Contains(searchValue, StringComparison.CurrentCultureIgnoreCase))
                );
            }

            // Filter by price range and category
            if (filterParameter != null)
            {
                productQuery = productQuery.Where(p => p.UnitCost >= filterParameter.Start && p.UnitCost <= filterParameter.End);

                if (filterParameter.SelectedCategory?.Any() ?? false)
                {
                    productQuery = productQuery.Where(p => filterParameter.SelectedCategory.Contains(p.ProductType));
                }
            }

            var products = await _productsRepository.FindByFilterAsync(productQuery, skip, take);
            var count = await _productsRepository.GetCountByFilterAsync(productQuery);

            

            var cableProducts = products.OfType<QuickTieCable>().Distinct(new DistinctProductComparer());

            // Filter out cables
            products.ToList().RemoveAll(p => p.ProductType == ProductType.QuickTieCable);


            return (products, count);
        }
        public async Task<Dictionary<ProductType, ProductTypeInfo>> GetCategoriesAsync(string searchValue, FilterParameter? filterParameter = null)
        {
            var categoryQuery = _productsRepository.GetQueryable();

            // Filter by search value
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                categoryQuery = categoryQuery.Where(p =>
                    p.Name.ToLower().Contains(searchValue.ToLower()) ||
                    p.ReferenceNumbers.Any(rn => rn.Name.ToLower().Contains(searchValue.ToLower())));
            }

            // Filter by price range
            if (filterParameter != null)
            {
                categoryQuery = categoryQuery.Where(p => p.UnitCost >= filterParameter.Start && p.UnitCost <= filterParameter.End);
            }

            var products = await _productsRepository.FindByFilterAsync(categoryQuery);
            var categories = products.GroupBy(p => p.ProductType)
                                     .OrderBy(g => g.Key)
                                     .Select(g => new KeyValuePair<ProductType, long>(g.Key, g.Count()))
                                     .ToList();
            var productTypeAndCounts = GetCategoryList()
                                       .ToDictionary(productType => productType,
                                                     productType => new ProductTypeInfo
                                                     {
                                                         Type = productType,
                                                         DisplayName = productType.GetEnumDisplayName(),
                                                         Count = 0,
                                                         ImageUrl = "https://quicktie.s3.amazonaws.com/" + ReflectionExtensions.GetEnumImageUrl(productType)
                                                     });

            foreach (var category in categories)
            {
                productTypeAndCounts[category.Key].Count = category.Value;
            }

            return productTypeAndCounts;
        }

        public IEnumerable<ProductType> GetCategoryList()
        {
            return Enum.GetValues(typeof(ProductType)).Cast<ProductType>().OrderBy(e=> e.ToString());
        }

        public async Task<Product> GetProductByIdAsync(string Id)
        {
            return await _productsRepository.FindByIdAsync(Id);
        }

        public async Task<bool> AddProductImages(string productId, List<string> imageUrls)
        {
            var product = await _productsRepository.FindByIdAsync(productId);
            if (product == null) return false;

            foreach (var item in imageUrls)
            {
                var newImage = new ProductImage
                {
                    Location = item,
                    Usage = ImageUsage.Main
                };

                product.Images.Add(newImage);
            }

            return await _productsRepository.ReplaceOneAsync(product);
        }
        public async Task<bool> DeleteProductImages(string productId, List<ProductImage> images)
        {
            var product = await _productsRepository.FindByIdAsync(productId);
            if (product == null) return false;
            foreach (var item in images)
            {
                var imageToDelete = product.Images.FirstOrDefault(img => img.Id == item.Id);
                if (imageToDelete != null)
                {
                    product.Images.Remove(imageToDelete);
                }
            }

            return await _productsRepository.ReplaceOneAsync(product);
        }
        public async Task<bool> UpdateProductAsync(Product product)
        {
            if (product == null) return false;

            return await _productsRepository.ReplaceOneAsync(product);
        }
    }
}
