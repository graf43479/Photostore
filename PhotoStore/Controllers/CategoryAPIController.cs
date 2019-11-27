using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using PhotoStore.Extensions;
using PhotoStore.Models;

namespace PhotoStore.Controllers
{
    public class CategoryAPIController :  EntitySetController<CategoryViewModel, int>
    {
        private PhotoDBContext db = new PhotoDBContext();


        //private ICategoryRepository categoryRepository;

        //public CategoryController(ICategoryRepository categoryRepository)
        //{
           
        //    this.categoryRepository = categoryRepository;
          
        //}
        public override IQueryable<CategoryViewModel> Get()
        {
            List<Category> catList = db.Categories.ToList();
            List<CategoryViewModel> viewModel = new List<CategoryViewModel>();
            foreach (var cat in catList)
            {
                viewModel.Add(cat.CategoryToViewModel());
            }
            //return db.Categories.Include("Products"); 
            //return db.Categories.Include("Products"); 
            return viewModel.AsQueryable();


        }

        //protected override Category GetEntityByKey(int key)
        //{
        //    return db.Categories.FirstOrDefault(c => c.CategoryID == key);
        //}


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
                                                                       