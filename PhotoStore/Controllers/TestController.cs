using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;

namespace PhotoStore.Controllers
{
    public class TestController : Controller
    {
       
        

     


        ////-----------------------------------------------------
        // public TestController(IProductRepository repoProduct, ICategoryRepository repoCategory,
        //                        IUserRepository repoUser, ICommentRepository repoComment, ICalendarRepository repoCalendar)
        // {
        //     repositoryProduct = repoProduct;
        //     repositoryCategory = repoCategory;
        //     repositoryUser = repoUser;
        //     repositoryComment = repoComment;
        //     repositoryCalendar = repoCalendar;
        // }
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View();
        }

    }
}
