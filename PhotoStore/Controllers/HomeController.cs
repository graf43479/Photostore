using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Domain;
using Domain.Abstract;
using Domain.Entities;
using PhotoStore.Models;

namespace PhotoStore.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;
        private ICommentRepository commentRepository;
        private ICalendarRepository calendarRepository;
        private IDeliveryProcessor deliveryProcessor;

        public HomeController(IProductRepository productRepository, ICategoryRepository categoryRepository, 
            ICommentRepository commentRepository, ICalendarRepository calendarRepository,
            IDeliveryProcessor deliveryProcessor)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.commentRepository = commentRepository;
            this.deliveryProcessor = deliveryProcessor;
            this.calendarRepository = calendarRepository;
        }

        public ActionResult Index()
        {
            //throw new HttpException(404, "облом");
            
         //   ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            TempData["nav-message"] = "About";
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            TempData["nav-message"] = "Contact";
            return View(new Message());
        }

        [HttpPost]
        public ActionResult Contact(Message message)
      //  public ActionResult Contact(string Name, string Email, string Text)
        {
          
            TempData["nav-message"] = "Contact";
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    try
                    {
                        deliveryProcessor.FeedBackRequest(message);
                        return Content("true");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Возникла ошибка при отправке сообщения!");
                        return PartialView("ContactPartialView", message);   
                    }

                }
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Некорректно заполнение полей!");
                return PartialView("ContactPartialView",message);    
            }
            
        }

        public ActionResult Price()
        {
            TempData["nav-message"] = "Price";
            return View();
        }

        public ActionResult Comments()
        {
            TempData["nav-message"] = "Comments";

            IEnumerable<Comment> comments = commentRepository.Comments.Where(x=>x.IsAccept==true).OrderByDescending(x=>x.CreateDate).ToList();

            return View(comments);
        }


        public ActionResult Calendar()
        {
            TempData["nav-message"] = "Calendar";
            return View();
        }

        [HttpPost]
        public ActionResult BookDate(int year, int month, int day)
        {
            DateTime dt = new DateTime(year, month, day);
            
                var choosenDate =
                    calendarRepository.Calendars.FirstOrDefault(
                        x => x.CalendarDate.Year == dt.Year & x.CalendarDate.Month == dt.Month & x.CalendarDate.Day == dt.Day);
                if (choosenDate==null)
                {
                    TempData["BookDate"] = dt;
                    return Json(new { success = "true" });        
                }
            return Json(new { success = "false" });        
        }

        [HttpPost]
        public ActionResult GetBookedDates()
        {
            List<DateParts> dt = (from dates in calendarRepository.Calendars.Where(x=>x.CalendarDate>DateTime.Now).ToList()
                     select new DateParts
                         {
                             Day = dates.CalendarDate.Day,
                             Month = dates.CalendarDate.Month,
                             Year = dates.CalendarDate.Year
                         }).ToList();
   
            
            //List<DateParts> dt = new List<DateParts>()
            //    {
            //        new DateParts() {Day = 5, Month = 10, Year = 2015},
            //        new DateParts() {Day = 8, Month = 10, Year = 2015},
            //        new DateParts() {Day = 3, Month = 10, Year = 2015},
            //        new DateParts() {Day = 1, Month = 10, Year = 2015},
            //    };
        



            //List<string> dt = new List<string>();
            //dt.Add(DateTime.Now.AddDays(-1).ToShortDateString());
            //dt.Add(DateTime.Now.AddDays(-7).ToShortDateString());
            //dt.Add(DateTime.Now.AddDays(-8).ToShortDateString());
            //dt.Add(DateTime.Now.AddDays(-5).ToShortDateString());
            
            
            //TempData["BookDate"] = dt;
            return Json(new { success = dt});
        }

        



        public ActionResult Portfolio(string category, bool? ajax, int page = 1 ) //(int? categoryId)
        {
            
            TempData["nav-message"] = "Portfolio";
            int pageSize = Constants.PRODUCT_PAGE_SIZE;

            IEnumerable<Product> products = productRepository.Products.ToList();
            if (!String.IsNullOrEmpty(category))
            {
                Category cat = categoryRepository.GetCategoryByShortName(category);
                if (cat==null)
                {
                    {
                        throw new HttpException(404, "Указанная страница не существует!");
                    }
                }
                products = products.Where(x => x.CategoryID == cat.CategoryID && x.IsDisplay);

             //   return View(products);    
            }
            else
            {
                products = products.Where(x=>x.IsDisplay);

               // return View(products);
            }
            ProductPagedListViewModel viewModel = new ProductPagedListViewModel
                {
                    PagingInfo = new PagingInfo
                        {
                            CurrentPage = page,
                            ItemsPerPage = pageSize,
                            TotalItems = products.Count()
                        },
                    CurrentCategory = category,
                    Products =  products.OrderByDescending(p => p.StartDate).Skip((page - 1) * pageSize).Take(pageSize)
                };

            if (Request.IsAjaxRequest())
            {
                // Thread.Sleep(10000);

               


                if (ajax==true)
                {
                   // Thread.Sleep(2000);
                    JsonModel jsonModel = new JsonModel();
                    jsonModel.NoMoreData = viewModel.Products.Count() < pageSize;
                    jsonModel.HTMLString = RenderPartialViewToString("PortfolioAjaxPartialView", viewModel);

                    return Json(jsonModel);    
                }
                else
                {
                  //  Thread.Sleep(2000);
                    Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                    return PartialView("PortfolioPartialView", viewModel);
                }
                
            }

            return View(viewModel);
            
        }

        [HttpGet]
        public PartialViewResult SendComment()
        {
            //CommentViewModel viewModel = new CommentViewModel();
            return PartialView("SendCommentPartialView",new Comment());
        }

        [HttpPost]
        public ActionResult SendComment(Comment comment)
        {
            //Thread.Sleep(2000);
            if (ModelState.IsValid)
            {
                comment.IsAccept = false;
                comment.CreateDate = DateTime.Now;
                commentRepository.SaveComment(comment);

                
                //return Content("success");
                return Json(new {success = "true"});
                //return PartialView(new CommentViewModel());
            }
            else
            {
                var errors = ModelState.Select(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage));

                return Json(new {success = "false", message = errors}); //Content(errors);
            }
            //CommentViewModel viewModel = new CommentViewModel();
           // return null;  //PartialView("SendCommentPartialView",viewModel);
        }

        [HttpPost]
        public ActionResult ImageGalery()
        {
            IEnumerable<Product> imageList = productRepository.Products.Where(x => x.IsChoosen);
            List<string> list = new List<string>();
            foreach (var image in imageList)
            {
                list.Add("Content/img/Image" + image.ProductID + "" + image.ImgExt);
            }
            return Json(list);    
        }






        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }


        public ActionResult Robots()
        {
            Response.ContentType = "text/plain";
            return View();
        }


        public ActionResult Sitemap()
        {
            string host = Request.Url.Host;
            string port = Request.Url.Port.ToString();
            Response.ContentType = "text/xml";
            IEnumerable<Category> categoryList = categoryRepository.Categories.OrderBy(x => x.ShortName).Where(x => x.IsActive);
            //IEnumerable<Product> productList = dataManager.Products.Products.OrderBy(x => x.Category.ShortName).Where(x => x.IsDeleted != true);
            
            List<SitemapAttributes> nodes = new List<SitemapAttributes>();

            //url.Add();

            string path = "http://" + host + ":" + port + "/";

            foreach (var category in categoryList)
            {
                nodes.Add(new SitemapAttributes
                {
                    Loc = path + category.ShortName + "\n",
                    Lastmod = category.UpdateDate.ToString("yyyy-MM-dd"),
                    Changefreq = "daily",
                    Priority = "0.8"
                });
            }
            

            nodes.Add(new SitemapAttributes
            {
                Loc = path + "Contact" + "\n",
                Lastmod = DateTime.Now.ToString("yyyy-MM-dd"),
                Changefreq = "monthly",
                Priority = "0.5"
            });

            nodes.Add(new SitemapAttributes
            {
                Loc = path + "Portfolio" + "\n",
                Lastmod = DateTime.Now.ToString("yyyy-MM-dd"),
                Changefreq = "monthly",
                Priority = "0.3"
            });

            nodes.Add(new SitemapAttributes
            {
                Loc = path + "Price" + "\n",
                Lastmod = DateTime.Now.ToString("yyyy-MM-dd"),
                Changefreq = "daily",
                Priority = "0.5"
            });
            
            nodes.Add(new SitemapAttributes
            {
                Loc = path + "About" + "\n",
                Lastmod = DateTime.Now.ToString("yyyy-MM-dd"),
                Changefreq = "daily",
                Priority = "0.5"
            });

            nodes.Add(new SitemapAttributes
            {
                Loc = path + "Comments" + "\n",
                Lastmod = DateTime.Now.ToString("yyyy-MM-dd"),
                Changefreq = "daily",
                Priority = "0.5"
            });

            XElement data = new XElement("urlset", nodes.Select(x => new XElement("url",
                                                                                  new XElement("loc", x.Loc),
                                                                                  new XElement("lastmod", x.Lastmod),
                                                                                  new XElement("changefreq", x.Changefreq),
                                                                                  new XElement("priority", x.Priority))));
            data.Add(new XAttribute("id", "1"));
            return Content(data.ToString(), "text/xml");
        }

        
        public class JsonModel
        {
            public string HTMLString { get; set; }
            public bool NoMoreData { get; set; }
        }




        public class SitemapAttributes
        {
            public string Loc { get; set; }
            public string Lastmod { get; set; }
            public string Changefreq { get; set; }
            public string Priority { get; set; }

        }
    }
}
