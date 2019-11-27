using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using Domain.Concrete;
using Domain.Entities;
using Microsoft.Data.OData;
using PhotoStore.Models;
using PhotoStore.Extensions;

namespace PhotoStore.Controllers
{
    public class ProductAPIController : EntitySetController<ProductAPI, int>
    {
        private PhotoDBContext db = new PhotoDBContext();


        //private ICategoryRepository categoryRepository;

        //public CategoryController(ICategoryRepository categoryRepository)
        //{

        //    this.categoryRepository = categoryRepository;

        //}
        public override IQueryable<ProductAPI> Get()
        {
            //IQueryable<ProductAPI> prodList = db.Products;

            List<Product> prodList = db.Products.ToList();
            List<ProductAPI> viewModel = new List<ProductAPI>();
            foreach (var product in prodList)
            {
                viewModel.Add(product.ProductToProductAPI());
            }

            return viewModel.AsQueryable();
            //return db.Products;
        }


        protected override ProductAPI CreateEntity(ProductAPI viewModel)
        {
            if (ModelState.IsValid)
            {
                Product product = viewModel.ProductAPIToProduct();
                //var entity = entityDto.ToEntity();
                db.Products.Add(product);
                db.SaveChanges();
                //return new ProductViewModel(db.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductID == entity.ProductID));
                return product.ProductToProductAPI();
            }
            else
            {
                HttpResponseMessage response = null;
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new ODataError
                {
                    ErrorCode = "ValidationError",
                    Message = String.Join(";", ModelState.Values.First().Errors.Select(e => e.ErrorMessage).ToArray())

                });
                throw new HttpResponseException(response);
            }
        }

        protected override ProductAPI UpdateEntity(int key, ProductAPI product)
        {

            if (!db.Products.Any(p => p.ProductID == key))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            //var update = updateDto.ToEntity();
            Product prod = product.ProductAPIToProduct();
            db.Products.Attach(prod);
            db.Entry(prod).State = EntityState.Modified;
            db.SaveChanges();

            prod = db.Products.Include(x=>x.Category).FirstOrDefault(p => p.ProductID == key);
            //return new ProductAPI(db.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductID == key));
            return prod.ProductToProductAPI();

        }
        protected override ProductAPI PatchEntity(int key, Delta<ProductAPI> patch)
        {
            Product product = db.Products.FirstOrDefault(p => p.ProductID == key);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            ProductAPI productAPI = product.ProductToProductAPI();

            patch.Patch(productAPI);
            db.Products.Attach(product);
            db.SaveChanges();
            //return new ProductAPI(productAPI);
            return productAPI;
        }
        public override void Delete([FromODataUri] int key)
        {
            Product product = db.Products.FirstOrDefault(p => p.ProductID == key);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            db.Products.Remove(product);
            db.SaveChanges();
        }

        protected override int GetKey(ProductAPI entity)
        {
            return entity.ProductID;
        }

        protected override ProductAPI GetEntityByKey(int key)
        {
            Product product = db.Products.FirstOrDefault(p => p.ProductID == key);
            return product.ProductToProductAPI();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
