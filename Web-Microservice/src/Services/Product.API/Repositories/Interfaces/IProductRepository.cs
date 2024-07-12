using Contracts.Commons.Interfaces;
using Product.API.Persistence;

namespace Product.API.Repositories.Interfaces
{
    public interface IProductRepository : IRepositoryBaseAsync<Entities.Product, long, ProductContext>
    {
        Task<IEnumerable<Entities.Product>> GetProducts();

        Task<Entities.Product> GetProduct(long productId);

        Task<Entities.Product> GetProductByNo(string productNo);

        Task CreateProduct(Entities.Product product);

        Task UpdateProduct(Entities.Product product);

        Task DeleteProduct(long productId);
    }
}
