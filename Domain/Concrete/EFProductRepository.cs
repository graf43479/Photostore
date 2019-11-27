using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private PhotoDBContext context;

        public EFProductRepository(PhotoDBContext context)
        {
            this.context = context;
        }

        public IQueryable<Product> Products {
            get {  return  context.Products.Include("Category");} 
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                try
                {
                    product.Sequence = context.Products.Where(x => x.CategoryID == product.CategoryID).Select(x => x.Sequence).Max() + 1;
                }
                catch (Exception)
                {
                    product.Sequence = 1;
                }
                //product.ShortName = GetShortName(product.Name, context.Products.Max(x => x.ProductID) + 1);
                context.Products.Add(product);
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
                try
                {
                    var productList =
                        context.Products.Where(x => x.CategoryID == product.CategoryID)
                               .OrderBy(x => x.Sequence)
                               .ToList();
                    
                    foreach (var p in productList)
                    {
                        if (p.Sequence <= product.Sequence)
                        {

                        }
                        else
                        {
                            --p.Sequence;
                            context.Entry(p).State = EntityState.Modified;
                        }
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    context.Products.Remove(product);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            
            
        }

        public void UpdateProductSequence(int categoryId, bool every)
        {
            int i = 1;
            if (every)
            {
                int[] z =
                context.Products.Select(x => x.CategoryID).Distinct().ToArray();

                foreach (var category in z)
                {
                    foreach (var product in context.Products.Where(x => x.CategoryID == category))
                    {
                        product.Sequence = i;
                        context.Entry(product).State = EntityState.Modified;
                        i++;
                    }
                }
            }
            else
            {
                foreach (var product in context.Products.Where(x => x.CategoryID == categoryId))
                {
                    product.Sequence = i;
                    context.Entry(product).State = EntityState.Modified;
                    i++;
                }

            }
            context.SaveChanges();
        }

        public void ProductSequence(int productId, string actionType)
        {
            Product product1 =
                context.Products.FirstOrDefault(x => x.ProductID == productId);

            if (actionType == "Up")
            {
                if (product1.Sequence == 1)
                {
                    return;
                }
                else
                {
                    Product product2 =
                    context.Products.Where(x => x.CategoryID == product1.CategoryID).FirstOrDefault(x => x.Sequence == product1.Sequence - 1);
                    product2.Sequence = product2.Sequence + 1;
                    product1.Sequence = product1.Sequence - 1;
                    context.Entry(product1).State = EntityState.Modified;
                    context.Entry(product2).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            else if (actionType == "Down")
            {
                if (product1.Sequence == context.Products.Where(x => x.CategoryID == product1.CategoryID).Max(x => x.Sequence))
                {
                    return;
                }
                else
                {
                    Product product2 =
                context.Products.Where(x => x.CategoryID == product1.CategoryID).FirstOrDefault(x => x.Sequence == product1.Sequence + 1);
                    product2.Sequence = product2.Sequence - 1;
                    product1.Sequence = product1.Sequence + 1;
                    context.Entry(product1).State = EntityState.Modified;
                    context.Entry(product2).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        public void RefreshEveryProductSequence(int[] categoryIdArray)
        {
            //foreach (var i in categoryIdArray)
            //{
            //    IEnumerable<Product> productListNotActive = context.Products.Where(x => x.CategoryID == i).Where(z => z.IsActive != true || z.IsDeleted == true);
            //    foreach (var product in productListNotActive)
            //    {
            //        product.Sequence = 10000;
            //        context.Entry(product).State = EntityState.Modified;

            //    }
            //    context.SaveChanges();
            //}



            foreach (var i in categoryIdArray)
            {
                int currentSequensNum = 1;

                //выставляем максимум тем товарам, у которых статус неактивен или удален

                IEnumerable<Product> productList = context.Products.Where(x => x.CategoryID == i).OrderBy(f => f.Sequence);

                foreach (var product in productList)
                {
                    /*     if (product.Sequence == currentSequensNum)
                         {
                             currentSequensNum++;    
                         }
                         else*/

                    product.Sequence = currentSequensNum;
                    //SaveProduct(product);
                    context.Entry(product).State = EntityState.Modified;

                    currentSequensNum++;
                }

                context.SaveChanges();



            }

        }

        public Product GetProductOrigin(Product product)
        {
            context.Entry(product).State = EntityState.Detached;
            return product;
        }
    }
}
