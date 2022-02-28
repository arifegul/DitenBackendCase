using DataAccessLayer.Concrete;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Context context;

        public ProductRepository(Context context)
        {
            this.context = context;
        }

        public async Task<Products> AddProducts(Products products)
        {
            var result = await context.Products.AddAsync(products);
            await context.SaveChangesAsync();
            return result.Entity;
        }


        public async Task DeleteProducts(int id)
        {
            var result = await context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            if (result != null)
            {
                result.DeleteStatus = true;
                context.Products.Remove(result);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Products>> GetProducts()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Products> GetProductsById(int id)
        {
            return await context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

        }

        public async Task<IEnumerable<Products>> SearchProductName(string name)
        {
            IQueryable<Products> query = context.Products;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.ProductName.Contains(name));
            }

            return await query.ToListAsync();
        }

        //public async Task<IEnumerable<Products>> SearchStock(int stock)
        //{
        //    var data = from Products in context.Products where Products.Stock == stock select new { stock = Products.Stock };
        //    return (IEnumerable<Products>)await data.ToListAsync();
        //}

        public async Task<Products> UpdateProducts(Products products)
        {
            var result = await context.Products.FirstOrDefaultAsync(x => x.ProductId == products.ProductId);

            if (result != null)
            {
                result.ProductName = products.ProductName;
                result.Stock = products.Stock;
                result.UpdatedUser = products.UpdatedUser;
                result.UpdatedDate = DateTime.Now;

                await context.SaveChangesAsync();

                return result;
            }

            return null;
        }
    }
}
