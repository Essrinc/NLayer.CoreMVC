using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<List<Product>> GetProductsWithCategory()
        {
            //Eager Loading = ilk productları çektiğim anda categorileri de çektim.
            //önce productları çekip sonra kategorileri çekseydim : Lazy Loading
            return await _context.products.Include(x => x.Category).ToListAsync();
        }
    }
}
