using Contracts.Commons.Interfaces;
using Infrastructure.Commons;
using Microsoft.EntityFrameworkCore;
using Product.API.Persistence;
using Product.API.Repositories.Interfaces;

namespace Product.API.Repositories
{
    public class ProductRepository : RepositoryBaseAsync<Entities.Product, long, ProductContext>, IProductRepository
    {
        public ProductRepository(ProductContext dbContext, IUnitOfWork<ProductContext> unitOfWork) : base(dbContext, unitOfWork)
        {
            
        }

        public async Task<IEnumerable<Entities.Product>> GetProducts() => await FindAll().ToListAsync();

        public Task<Entities.Product> GetProduct(long productId) => GetByIdAsync(productId);

        public Task<Entities.Product> GetProductByNo(string productNo) => FindCondition(x => x.No.Equals(productNo))
            .FirstOrDefaultAsync();

        public Task CreateProduct(Entities.Product product) => CreateAsync(product);

        public Task UpdateProduct(Entities.Product product) => UpdateAsync(product);

        public async Task DeleteProduct(long productId)
        {
            var product = await GetProduct(productId);
            if (product != null) DeleteAsync(product);
        }
    }
}
