using DotNet_MinimalAPI_Example.Data;
using DotNet_MinimalAPI_Example.Models;
using DotNet_MinimalAPI_Example.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DotNet_MinimalAPI_Example.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly ApplicationDBContext _db;
        public ProductRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Product product)
        {
            _db.Add(product);
        }

        public async Task DeleteAsync(Product product)
        {
            _db.Products.Remove(product);
        }

        public async Task<ICollection<Product>> GetAllAsync()
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<Product> GetAsync(int id)
        {
            return await _db.Products.FirstOrDefaultAsync(u=>u.ProductId== id);
        }

        public async Task<Product> GetAsync(string productName)
        {
            return await _db.Products.FirstOrDefaultAsync(u => u.Name.ToLower() == productName.ToLower());
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _db.Products.Update(product);
        }
    }
}
