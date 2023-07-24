using DotNetMastery.DataAccess.Data;
using DotNetMastery.DataAccess.Repository.IRepository;
using DotNetMastery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotNetMastery.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db): base(db) 
        { 
            _db = db;
        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.ProductId == obj.ProductId);
            if (objFromDb != null)
            {
                objFromDb.Title = objFromDb.Title;
                objFromDb.Description = objFromDb.Description;
                objFromDb.ISBN = objFromDb.ISBN;
                objFromDb.Author = objFromDb.Author;
                objFromDb.ListPrice = objFromDb.ListPrice;
                objFromDb.Price = objFromDb.Price;
                objFromDb.Price50 = objFromDb.Price50;
                objFromDb.Price100 = objFromDb.Price100;
                objFromDb.CategoryId = objFromDb.CategoryId;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
