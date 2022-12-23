using DotNet_MinimalAPI_Example.Models;

namespace DotNet_MinimalAPI_Example.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<ICollection<Product>> GetAllAsync();
        Task<Product> GetAsync(int id);
        Task<Product> GetAsync(string productName);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task SaveAsync();
    }
}
