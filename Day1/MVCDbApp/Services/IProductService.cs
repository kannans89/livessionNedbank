using MVCDbApp.Models;

namespace MVCDbApp.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
    }
}