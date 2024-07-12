namespace Product.API.Persistence
{
    public class ProductContextSeed
    {
        public static async Task SeedProductAsync(ProductContext productContext, Serilog.ILogger logger)
        {
            if (!productContext.Products.Any())
            {
                productContext.AddRange(GetProducts);
                await productContext.SaveChangesAsync();
                logger.Information("Seeded data for ProductDb associated with context {DbContextName}", nameof(ProductContext));
            }
        }

        private static IEnumerable<Entities.Product> GetProducts()
        {
            return new List<Entities.Product>()
            {
                new()
                {
                    No = "01",
                    Name = "SP 01",
                    Summary = "summary 1",
                    Decription = "des 1",
                    Price = 10000
                },
                new()
                {
                    No = "02",
                    Name = "SP 02",
                    Summary = "summary 2",
                    Decription = "des 2",
                    Price = 20000
                }
            };
        }
    }
}
