using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetProducts();
        Task<IEnumerable<Products>> SearchProductName(string name);
        //Task<IEnumerable<Products>> SearchStock(int stock);
        Task<Products> GetProductsById(int id);
        Task<Products> AddProducts(Products products);
        Task<Products> UpdateProducts(Products products);
        Task DeleteProducts(int id);
    }
}
