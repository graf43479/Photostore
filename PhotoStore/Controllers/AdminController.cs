using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain;
using Domain.Abstract;
using Domain.Entities;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using PhotoStore.Models;
using PhotoStore.Extensions;

namespace PhotoStore.Controllers
{
     [Authorize(Roles = "admin, contentManager, Admin, ContentManager, SEO")]
    public class AdminController : Controller
    {
        
        private IProductRepository repositoryProduct;
        private ICategoryRepository repositoryCategory;
        private IUserRepository repositoryUser;
        private ICommentRepository repositoryComment;
         private ICalendarRepository repositoryCalendar;
        

        public int PageSize = 5;


        //-----------------------------------------------------
         public AdminController(IProductRepository repoProduct, ICategoryRepository repoCategory,
                                IUserRepository repoUser, ICommentRepository repoComment, ICalendarRepository repoCalendar)
         {
             repositoryProduct = repoProduct;
             repositoryCategory = repoCategory;
             repositoryUser = repoUser;
             repositoryComment = repoComment;
             repositoryCalendar = repoCalendar;
         }


         public ActionResult Index()
        {
            return View();
        }

#region Category
        public ActionResult Categories(string searchWord, GridSortOptions gridSortOptions, int? page)
        {
            var productsList = repositoryProduct.Products;// GetProductListAsync();

            int pageItemsCount = 20;

         
            IEnumerable<Category> categoryType = repositoryCategory.Categories.ToList();
            
         
            var query = from a in categoryType
                        select new CategoryViewModel()
                        {
                            CategoryID = a.CategoryID,
                            Sequence = a.Sequence,
                            ShortName = a.ShortName,
                            Description = a.Description,
                            CategoryName = a.CategoryName,
                            CreateDate = a.CreateDate,
                            UpdateDate = a.UpdateDate,
                            Snippet = a.Snippet,
                            IsActive = a.IsActive,
                            KeyWords = a.KeyWords,
                            PhotoCount = productsList.Count(x => x.CategoryID==a.CategoryID),
                            ChoosenCount = productsList.Where(x=>x.IsChoosen).Count(x => x.CategoryID == a.CategoryID),
                            VisibleCount = productsList.Where(x => x.IsDisplay==false).Count(x => x.CategoryID == a.CategoryID)

                        };
         
            var pagedViewModel = new PagedViewModel<CategoryViewModel>
            {
                ViewData = ViewData,
                Query = query.AsQueryable(),//categoryType.AsQueryable(),
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "CategoryName",
                Page = page,
                PageSize = (pageItemsCount == 0) ? Domain.Constants.ADMIN_PAGE_SIZE : pageItemsCount,
            }
                .AddFilter("searchWord", searchWord,
                           a => a.CategoryName.ToLower().Contains(searchWord.ToLower()) /*|| a.ShortName.Contains(searchWord)*/)
                .Setup();


            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("CategoryGridPartial", pagedViewModel);
            }

            return View(pagedViewModel);

        }



        //[Authorize(Roles = "Admin, ContentManager")]
        //public ViewResult CreateCategory()
        //{
        //    IEnumerable<SuperCategory> superCategoryList = repositorySuperCategory.SuperCategories;
        //    return View("EditCategory", new CategoryViewModel() { SuperCategories = superCategoryList });
        //}



        public ViewResult EditCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                    return View("EditCategory", new CategoryViewModel());
            }


            Category category = repositoryCategory.Categories
                                                  .FirstOrDefault(p => p.CategoryID == categoryId);

            CategoryViewModel viewModel = category.CategoryToViewModel();
            
            

            return View(viewModel);

        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditCategory(CategoryViewModel viewModel)
        {
         //   var superCategoryListAsync = repositorySuperCategory.SuperCategories;//.GetSuperCategoryListAsync();
            Category category = viewModel.ViewModelToCategory();
            

            if ((ModelState.IsValid))//                && (repositoryCategory.Categories.FirstOrDefault(x => x.Name.TrimEnd() == category.Name.TrimEnd()) == null))
            {
                /*         if (image != null)
                                 {
                                     product.ImageMimeType = image.ContentType;
                                     product.ImageData = new byte[image.ContentLength];
                                     image.InputStream.Read(product.ImageData, 0, image.ContentLength);
         
                                 }
                                 // save the product
                         */


                if ((repositoryCategory.Categories.FirstOrDefault(x => x.CategoryName.TrimEnd() == category.CategoryName.TrimEnd()) != null) && (category.CategoryID == 0))
                {
                    TempData["message"] = string.Format("{0} уже существует в базе! Изменения не внесены", category.CategoryName);
                    TempData["messageType"] = "warning-msg";
                    return View(viewModel);
                }

                category.UpdateDate = DateTime.Now;
                if (category.CategoryID == 0)
                {
                    category.CreateDate = DateTime.Now;
                }

                repositoryCategory.SaveCategory(category);
                //repositoryCategory.SaveCategory(category);
                repositoryCategory.RefreshAllShortNames();
                // add a message to the viewbag

               
                if (category.CategoryID == 0)
                {
                    TempData["message"] = string.Format("Категория '{0}' создана", category.CategoryName);
                    TempData["messageType"] = "confirmation-msg";
                }
                else
                {
                    TempData["message"] = string.Format("Категория '{0}' изменена", category.CategoryName);
                    TempData["messageType"] = "information-msg";
                }
                // return the user to the list
                return RedirectToAction("Categories");



                //return RedirectToAction("EditCategory", new { categoryId = category.CategoryID });
            }
            else
            {
                // there is something wrong with the data values
                TempData["message"] = string.Format("Категория '{0}' уже существует в базе! Изменения не внесены", category.CategoryName);
                TempData["messageType"] = "warning-msg";
                return View(viewModel);
            }   
        }

        [HttpPost]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult DeleteCategory(int categoryId)
        {
            int[] productsIdForDelete = repositoryProduct.Products.Where(x => x.CategoryID == categoryId).Select(x => x.ProductID).ToArray();
            DeleteProductRelationships(productsIdForDelete);

            Category category = repositoryCategory.Categories.FirstOrDefault(p => p.CategoryID == categoryId);

            if (category != null)
            {

                //Product pr = repositoryProduct.Products.FirstOrDefault(x => x.CategoryID == category.CategoryID);

                
                    repositoryCategory.DeleteCategory(category);
                    TempData["message"] = string.Format("Категория '{0}' была удалена", category.CategoryName);
                    TempData["messageType"] = "warning-msg";
            }
            
           
            

            return RedirectToAction("Categories");
        }



        public ActionResult CategorySequence(int categoryId, string actionType)
        {
            try
            {
                Exception ex = new Exception();
                int[] sequence =
                    repositoryCategory.Categories.Select(x => x.Sequence).ToArray();
                Array.Sort(sequence);
                //sequence.OrderBy(x => x.ToString());

                for (int i = 0; i < sequence.Count(); i++)
                {
                    if (sequence[i] == i + 1)
                    {

                    }
                    else
                    {
                        //logger.Error(User.Identity.Name + ". Проблема обновления списка " + ex.Message);
                        throw (ex);

                    }
                }

                repositoryCategory.CategorySequence(categoryId, actionType);
            }

            catch (Exception ex)
            {
                TempData["message"] = string.Format("Нарушена последовательность! Список был пересчитан!");
                TempData["messageType"] = "error-msg";
              //  logger.Error(User.Identity.Name + ". Нарушена последовательность в категориях! Список был пересчитан!" + ex.Message);
                repositoryCategory.UpdateCategorySequence();
                repositoryCategory.CategorySequence(categoryId, actionType);
            }

            GridSortOptions gridSortOptions = new GridSortOptions()
            {
                Column = "Sequence",
                Direction = SortDirection.Ascending
            };


            var pagedViewModel = new PagedViewModel<Category>()
            {
                ViewData = ViewData,
                Query = repositoryCategory.Categories, //_service.GetAlbumsView(),
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "Sequence",
                Page = 1,
                PageSize = 100 //Domain.Constants.ADMIN_PAGE_SIZE
            }
                .Setup();

            return PartialView("CategorySequenceGridPartial", pagedViewModel);

        }



        public ActionResult CategorySequenceView()
        {

            var productListAsync = repositoryProduct.Products;//GetProductListAsync();
            IQueryable<Category> categoryType = repositoryCategory.Categories;



            GridSortOptions gridSortOptions = new GridSortOptions()
            {
                Direction = SortDirection.Ascending,
                Column = "Sequence"
            };

            var pagedViewModel = new PagedViewModel<Category>
            {
                ViewData = ViewData,
                Query = categoryType.AsQueryable(),
                //repositoryCategory.Categories.Except(categoryType), //categoryType, //repositoryCategory.Categories.Where(x=>x.CategoryID==),  //Except(categoryType), //categoryType,//repositoryCategory.Categories, //_service.GetAlbumsView(),
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "Sequence",
                Page = 1,
                PageSize = 100,
            }
                .Setup();


            /* if (Request.IsAjaxRequest())
            {
                return PartialView("CategoryGridPartial", pagedViewModel);
            }
            */


            //productListAsync.Wait();
            //IEnumerable<Product> products = productListAsync.Result.ToList();
            IEnumerable<Product> products = productListAsync;

            IEnumerable<string> productCategories = from c in repositoryCategory.Categories.ToList()
                                                    join p in products on c.CategoryID
                                                        equals
                                                        p.CategoryID
                                                    //where p.IsActive == true && p.IsDeleted == false
                                                    group c by new { c.CategoryID, c.ShortName }
                                                        into tmp
                                                        select tmp.Key.ShortName;

            IEnumerable<string> categoriesExists = from c in repositoryCategory.Categories.ToList()
                                                   select c.ShortName;


            IQueryable<string> difference = categoriesExists.Except(productCategories).AsQueryable();



            var z = from j in repositoryCategory.Categories.ToList()
                    where j.ShortName == difference.FirstOrDefault(x => x.ToString() == j.ShortName)
                    select j.CategoryID;
            TempData["EmptyCategories"] = z.ToArray();




            var categoryGroupping = from n in products
                                    group n by n.CategoryID
                                        into g
                                        select new { CategoryID = g.Key, ProductCount = g.Count() };

            var singleProductCategory = from m in categoryGroupping
                                        where m.ProductCount == 1
                                        select m.CategoryID;
            TempData["SingProductCategories"] = singleProductCategory.ToArray();

            /*
            
            IEnumerable<string> productCategories2 = from c in repositoryCategory.Categories.ToList()
                                                    join p in repositoryProduct.Products.ToList() on c.CategoryID
                                                        equals
                                                        p.CategoryID
                                           //             where 
                                                    group c by new { c.CategoryID, c.ShortName }
                                                        into tmp
                                                        select tmp.Key.ShortName;

            IEnumerable<string> categoriesExists2 = from c in repositoryCategory.Categories.ToList()
                                                   select c.ShortName;


            IQueryable<string> difference2 = categoriesExists.Except(productCategories).AsQueryable();



            var z = from j in repositoryCategory.Categories.ToList()
                    where j.ShortName == difference.FirstOrDefault(x => x.ToString() == j.ShortName)
                    select j.CategoryID;

            */




            return View("CategorySequence", pagedViewModel);
        }

     

        public ActionResult RefreshAllShortNamesInCategories()
        {
            repositoryCategory.RefreshAllShortNames();
            return RedirectToAction("Categories");
        }


#endregion Category


        #region Product
        public ActionResult Products(string searchWord, GridSortOptions gridSortOptions, int? categoryId, int? page)
        {
            
            int pageItemsCount = Constants.ADMIN_PAGE_SIZE;

            List<ProductViewModel> products = new List<ProductViewModel>();
            var plist = repositoryProduct.Products.ToList();
            foreach (var product in plist)      
            {

                products.Add(product.ProductToViewModel());
            }



            var pagedViewModel = new PagedViewModel<ProductViewModel>
            {
                ViewData = ViewData,
                Query = products.AsQueryable(),//query, //repositoryProduct.Products, //repositoryProduct.Products,
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "UpdateDate",
                Page = page,
                PageSize = (pageItemsCount == 0) ? Domain.Constants.ADMIN_PAGE_SIZE : pageItemsCount,
            };
            //int s = 0;
            //if (quantity == "В асортименте")
            //{
            //    pagedViewModel.Query = pagedViewModel.Query.Where(x => x.Quantity > 20);
            //}
            //else if (quantity == "Мало")
            //{
            //    pagedViewModel.Query = pagedViewModel.Query.Where(x => x.Quantity > 0 & x.Quantity <= 20);
            //}
            //else if (quantity == "Нет")
            //{
            //    pagedViewModel.Query = pagedViewModel.Query.Where(x => x.Quantity == 0);
            //}

            pagedViewModel
                //.AddFilter("searchWord", searchWord,
                //           a =>
                //           a.Name.Contains(searchWord)
                //           || a.ShortName.Contains(searchWord)
                //           || a.CategoryName.Contains(searchWord)
                //           || a.ArticleNumber.Contains(searchWord)
                //           || a.Description.Contains(searchWord))
                .AddFilter("categoryId", categoryId, a => a.SelectedCategoryID == categoryId,
                           repositoryCategory.Categories.OrderBy(x => x.CategoryName), "CategoryName")
                .Setup();

            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("ProductGridPartial", pagedViewModel);
            }

            return View(pagedViewModel);
            //return View(repositoryProduct.Products);

        }


        public ViewResult EditProduct(int productId)
        {
            IEnumerable<Category> categoryList = repositoryCategory.Categories.OrderBy(x => x.CategoryName);
            //IEnumerable<ProductImage> productImagesList = repositoryProductImages.ProductImages;
            Product product = repositoryProduct.Products.FirstOrDefault(p => p.ProductID == productId);

            ProductViewModel productViewModel = product.ProductToViewModel();
            productViewModel.Categories = categoryList;

            //List<ProductViewModel> products = new List<ProductViewModel>();
            //var plist = repositoryProduct.Products.ToList();
            //foreach (var product in plist)
            //{

            //    products.Add(product.ProductToViewModel());
            //}



            return View(productViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditProduct(ProductViewModel productViewModel)
        {
            int selectedCategory = productViewModel.SelectedCategoryID;

            /*  Product product = new Product()
                  {
                      ProductID = productViewModel.ProductID,
                      Name = productViewModel.Name,
                      Price = productViewModel.Price,
                      Quantity = productViewModel.Quantity,
                      Description = productViewModel.Description,
                      ShortName = productViewModel.ShortName,
                      StartDate = productViewModel.StartDate,
                      UpdateDate = productViewModel.UpdateDate,
                      Sequence = productViewModel.Sequence,
                      IsActive = productViewModel.IsActive,
                      IsDeleted = productViewModel.IsDeleted,
                      CategoryID = productViewModel.SelectedCategoryID
                  };*/


            //int tmp3 = repositoryProduct.Products.Count();


            if (ModelState.IsValid)
            {
                productViewModel.UpdateDate = DateTime.Now;
                //productViewModel.CategoryName = productViewModel.CategoryName.Trim();

                if (productViewModel.ProductID == 0)
                {
                    productViewModel.StartDate = DateTime.Now;
                    repositoryProduct.SaveProduct(productViewModel.ViewModelToProduct());


                    TempData["message"] = string.Format("{0}-{1}.{2} сохранен", productViewModel.CategoryName, productViewModel.ProductID, productViewModel.ImgExt);
                    TempData["messageType"] = "confirmation-msg";
                    
                    return RedirectToAction("EditProduct", "Admin", new { productId = repositoryProduct.Products.Max(x => x.ProductID) });
                }
                else
                {
                    ///проверка, если у товара изменилась категория, то пересчитать величину Sequence для обеих категорий
                    /*   int oldProdCategoryId =
                       repositoryProduct.Products.FirstOrDefault(x => x.ProductID == productViewModel.ProductID).CategoryID;*/


                    Product product = repositoryProduct.Products.FirstOrDefault(x => x.ProductID == productViewModel.ProductID);
                    Product productOriginal = repositoryProduct.GetProductOrigin(product);

                    productViewModel.Sequence = 10000;
                    
                    repositoryProduct.SaveProduct(productViewModel.ViewModelToProduct());

                    if (productOriginal.CategoryID != productViewModel.SelectedCategoryID)
                    {
                        int[] categoryIdArray = new int[2];
                        categoryIdArray[0] = productOriginal.CategoryID;
                        categoryIdArray[1] = selectedCategory;
                        repositoryProduct.RefreshEveryProductSequence(categoryIdArray);
                    }

                    //if (productOriginal.IsActive != productViewModel.IsActive)
                    //{
                    //    int[] categoryIdArray = new int[1];
                    //    categoryIdArray[0] = productViewModel.SelectedCategoryID;
                    //    repositoryProduct.RefreshEveryProductSequence(categoryIdArray);
                    //}
                    return RedirectToAction("EditProduct", "Admin", new { productId = productViewModel.ProductID });
                }
            }
            else
            {
                IEnumerable<Category> categoryList = repositoryCategory.Categories;
                productViewModel.Categories = categoryList;
                // there is something wrong with the data values
                return View(productViewModel);
            }
        }

        //[Authorize(Roles = "Admin, ContentManager")]
        //public ViewResult CreateProduct()
        //{
        //    IEnumerable<Category> categoryList = repositoryCategory.Categories;
        //    return View("EditProduct", new ProductEditViewModel() { Categories = categoryList });
        //}

        [HttpPost]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult DeleteProduct(int[] resubmit)
        {
            DeleteProductRelationships(resubmit);
           
            return RedirectToAction("Products");
        }




        public void DeleteProductRelationships(int[] resubmit)
        {
            //if ((productId == null) && (resubmit!=null)) 
            if (resubmit != null)
            {
                try
                {
                    foreach (var p in resubmit)
                    {

                        //var productAsync = repositoryProduct.GetProductByIDAsync(p);
                        var product = repositoryProduct.Products.FirstOrDefault(x => x.ProductID == p);//GetProductByIDAsync(p);


                        if (product != null)
                        {
                            //productImage.Product = null;

                            string strSaveFileName = "Image" + product.ProductID.ToString() + product.ImgExt;
                            //repositoryProductImages.DeleteProductImage(productImage);
                            //  TempData["Message"] = string.Format("Изображение {0}_{1}.{2} было удалено", productImage.ProductID, productImage.ProductImageID, productImage.ImageExt);
                            //  TempData["messageType"] = "warning-msg";
                            string strSaveFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                                            Constants.PRODUCT_IMAGE_FOLDER,
                                                                            strSaveFileName);
                            string strSavePreviewFullPath =
                                System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                       Constants.PRODUCT_IMAGE_FOLDER,
                                                       Constants.PRODUCT_IMAGE_PREVIEW_FOLDER,
                                                       strSaveFileName);
                            if (System.IO.File.Exists(strSaveFullPath))
                            {
                                System.IO.File.Delete(strSaveFullPath);
                            }
                            else
                            {
                                Exception ex;
                            }
                            if (System.IO.File.Exists(strSavePreviewFullPath))
                            {
                                System.IO.File.Delete(strSavePreviewFullPath);
                            }

                            repositoryProduct.DeleteProduct(product);

                        }

                    }
                    TempData["message"] = string.Format("Фотографии были удалены");
                    TempData["messageType"] = "warning-msg";
                }
                catch (Exception)
                {
                    TempData["message"] = string.Format("Что-то пошло не так при удалении файлов");
                    TempData["messageType"] = "warning-msg";    
                }
                //IEnumerable<Product> productList = repositoryProduct.Products.Where(x=>x.ProductID==)
    
                TempData["message"] = string.Format("Фотографии были удалены");
                TempData["messageType"] = "warning-msg";
            }
            
            
        }

        public ActionResult ProductSequenceView(int categoryId)
        {
            //IQueryable<Category> categoryType = repositoryCategory.Categories;

            GridSortOptions gridSortOptions = new GridSortOptions()
            {
                Direction = SortDirection.Ascending,
                Column = "Sequence"
            };

            var pagedViewModel = new PagedViewModel<Product>
            {
                ViewData = ViewData,
                Query = repositoryProduct.Products.Where(x => x.CategoryID == categoryId),
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "Sequence",
                Page = 1,
                PageSize = 100,
            }
                .Setup();

            return View("ProductSequence", pagedViewModel);
        }


        public ActionResult ProductSequence(int productId, string actionType)
        {
            Product product = repositoryProduct.Products.FirstOrDefault(x => x.ProductID == productId);

            try
            {
                Exception ex = new Exception();
                int[] sequence =
                    repositoryProduct.Products.Where(x => x.CategoryID == product.CategoryID)
                                     .Select(x => x.Sequence)
                                     .ToArray();
                Array.Sort(sequence);
                //sequence.OrderBy(x => x.ToString());

                for (int i = 0; i < sequence.Count(); i++)
                {
                    if (sequence[i] == i + 1)
                    {

                    }
                    else
                    {
                        //logger.Error(User.Identity.Name + ". Ошибка при переборе последовательности товаров " + ex.Message);
                        throw (ex);
                    }
                }

                repositoryProduct.ProductSequence(productId, actionType);
            }

            catch (Exception ex)
            {
                TempData["message"] = string.Format("Нарушена последовательность! Список был пересчитан!");
                TempData["messageType"] = "error-msg";
                repositoryProduct.UpdateProductSequence(product.CategoryID, false);
                repositoryProduct.ProductSequence(productId, actionType);
                //logger.Error(User.Identity.Name + ". Нарушена последовательность! Приоритетность товаров пересчитана!" + ex.Message);
            }

            //---Определение пустых категорий 
            /*    IEnumerable<string> productCategories = from c in repositoryCategory.Categories.ToList()
                                                    join p in repositoryProduct.Products.ToList() on c.CategoryID
                                                        equals
                                                        p.CategoryID
                                                    group c by new { c.CategoryID, c.ShortName }
                                                        into tmp
                                                        select tmp.Key.ShortName;

            IEnumerable<string> categoriesExists = from c in repositoryCategory.Categories.ToList()
                                                   select c.ShortName;


            IQueryable<string> difference = categoriesExists.Except(productCategories).AsQueryable();



            var z = from j in repositoryCategory.Categories.ToList()
                    where j.ShortName == difference.FirstOrDefault(x => x.ToString() == j.ShortName)
                    select j.CategoryID;

            ViewBag.EmptyCategories = z.ToArray();  */
            //--------------------------


            GridSortOptions gridSortOptions = new GridSortOptions()
            {
                Column = "Sequence",
                Direction = SortDirection.Ascending
            };


            var pagedViewModel = new PagedViewModel<Product>
            {
                ViewData = ViewData,
                Query = repositoryProduct.Products.Where(x => x.CategoryID == product.CategoryID),
                //_service.GetAlbumsView(),
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "Sequence",
                Page = 1,
                PageSize = 100 //Domain.Constants.ADMIN_PAGE_SIZE
            }
                .Setup();

            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("ProductSequenceGridPartial", pagedViewModel);
            }

            return PartialView("ProductSequenceGridPartial", pagedViewModel);

        }

         public ActionResult UploadPhoto(int categoryId)
         {
             Category category = repositoryCategory.Categories.FirstOrDefault(x => x.CategoryID == categoryId); 
             return View(category);
         }

             public ActionResult Upload(int categoryId)
         {
             for (int i = 0; i < Request.Files.Count; i++)
             {
                 
                 
                 var file = Request.Files[i];
                 //file.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "Uploads/" + file.FileName);

                 //int productId;
                 //try
                 //{
                 //    productId = repositoryProduct.Products.Max(x => x.ProductID) + 1;
                 //}
                 //catch (Exception)
                 //{
                 //    productId = 1;
                 //} 

                
                 
                 int sequence;
                 try
                 {
                     sequence = ((repositoryProduct.Products.Where(x => x.CategoryID == categoryId)
                                                  .Select(x => x.Sequence)
                                                  .Max()) == 0
                              ? 1
                              : repositoryProduct.Products.Where(x => x.CategoryID == categoryId)
                                                       .Select(x => x.Sequence)
                                                       .Max() + 1);
                 }
                 catch (Exception)
                 {
                     
                     sequence = 1;
                 }

                 string strExtension = System.IO.Path.GetExtension(file.FileName);
                 string path = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                      "img");
                 Product product = new Product()
                     {
                         CategoryID = categoryId,
                         Path = path,
                         ImgExt = strExtension,
                         IsChoosen = false,
                         IsDisplay = true,
                         StartDate = DateTime.Now,
                         UpdateDate = DateTime.Now,
                         Sequence = sequence
                     };
                 repositoryProduct.SaveProduct(product);


                 
                 string strSaveFileName = "Image" + product.ProductID + strExtension;
                 
                 string strSaveFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                                 "img",
                                                                 strSaveFileName);

                 string strSavePreviewFullPath = System.IO.Path.Combine(
                            Server.MapPath(Url.Content("~/Content")), "img",
                            "cache", strSaveFileName);


                 // Если файл с таким названием имеется, удаляем его.
                 if (System.IO.File.Exists(strSaveFullPath)) System.IO.File.Delete(strSaveFullPath);
                 if (System.IO.File.Exists(strSavePreviewFullPath)) System.IO.File.Delete(strSavePreviewFullPath);
                 file.ResizeImage(Constants.PRODUCT_IMAGE_HEIGHT, Constants.PRODUCT_IMAGE_WIDTH, strSaveFullPath);
                 file.ResizeImage(Constants.PRODUCT_IMAGE_PREVIEW_HEIGHT, Constants.PRODUCT_IMAGE_PREVIEW_WIDTH, strSavePreviewFullPath);

             }
             return Json(new { success = true }, JsonRequestBehavior.AllowGet);
         }

         

#endregion Product


#region User
        public ActionResult UsersView(string searchWord, GridSortOptions gridSortOptions, int? page,int? roleId,
                                     string userActivity = "Активные")
        {
            var userList = repositoryUser.UsersInfo.ToList();
            bool IsActive;

            int pageItemsCount = Domain.Constants.ADMIN_PAGE_SIZE;

            List<UserViewModel> users = new List<UserViewModel>();

            foreach (var user in userList)
            {
                users.Add(user.UserToViewModel());
            }

            var pagedViewModel = new PagedViewModel<UserViewModel>
            {
                ViewData = ViewData,
                Query = users.AsQueryable(), 
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "Login",
                Page = page,
                PageSize = pageItemsCount
            }
                .AddFilter("searchWord", searchWord,
                           a =>
                           a.Login.ToLower().Contains(searchWord.ToLower()) || a.Email.ToLower().Contains(searchWord.ToLower()))
                
                .AddFilter("userActivity", (userActivity == "Активные") ? IsActive = true : IsActive = false,
                           a => a.IsActivated == IsActive) //,  _service.GetGenres(), "Name")
                 .AddFilter("roleId", roleId, a => a.SelectedRoleID == roleId,
                           repositoryUser.Roles.OrderBy(x => x.RoleName), "RoleName")
                .Setup();
            //  ViewBag.ServName = Server.MachineName;

            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("UserGridPartial", pagedViewModel);
            }

            return View(pagedViewModel);
            //return View(repositoryUser.UsersInfo);
        }



        [Authorize(Roles = "Admin, ContentManager")]
        public ViewResult EditUser(int userId)
        {
            /*
             IEnumerable<Category> categoryList = repositoryCategory.Categories.OrderBy(x => x.CategoryName);
            //IEnumerable<ProductImage> productImagesList = repositoryProductImages.ProductImages;
            Product product = repositoryProduct.Products.FirstOrDefault(p => p.ProductID == productId);

            ProductViewModel productViewModel = product.ProductToViewModel();
            productViewModel.Categories = categoryList;

            //List<ProductViewModel> products = new List<ProductViewModel>();
            //var plist = repositoryProduct.Products.ToList();
            //foreach (var product in plist)
            //{

            //    products.Add(product.ProductToViewModel());
            //}



            return View(productViewModel);
             */
            IEnumerable<Role> roleList = repositoryUser.Roles.OrderBy(x => x.RoleName);
            User user = repositoryUser.UsersInfo.FirstOrDefault(p => p.UserID == userId);
            UserViewModel userViewModel = user.UserToViewModel();
            userViewModel.Roles = roleList;
            return View(userViewModel);

        }


        [HttpPost]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult EditUser(UserViewModel viewModel)
        {

            viewModel.Email = viewModel.Email.TrimEnd();
            viewModel.Login = viewModel.Login.TrimEnd();
            viewModel.Password = viewModel.Password.TrimEnd();
            

            if (ModelState.IsValid)
            {
                repositoryUser.SaveUser(viewModel.ViewModelToUser());

                // return the user to the list

                if (viewModel.UserID == 0)
                {
                    // add a message to the viewbag
                    TempData["Message"] = string.Format("Пользователь '{0}' создан", viewModel.Login);
                    TempData["messageType"] = "confirmation-msg";
                }
                else
                {
                    TempData["Message"] = string.Format("Пользователь '{0}' изменен", viewModel.Login);
                    TempData["messageType"] = "information-msg";
                }

                return RedirectToAction("UsersView");
            }
            else
            {
                // there is something wrong with the data values
                return View(viewModel);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult DeleteUser(int?[] resubmit)
        {
            
            if (resubmit != null)
            {
                foreach (var p in resubmit)
                {
                    User user = repositoryUser.UsersInfo.FirstOrDefault(x => x.UserID == p);
                    if (user != null)
                    {
                        try
                        {
                            repositoryUser.DeleteUser(user);
                            TempData["Message"] = string.Format("Пользователь '{0}' был удален", user.Login);
                            TempData["messageType"] = "warning-msg";
                        }
                        catch (Exception)
                        {
                            
                            //repositoryUser.SaveUser(user);
                            TempData["Message"] = string.Format("Произошла ошибка удаления '{0}' ", user.Login);
                            TempData["messageType"] = "warning-msg";
                            //logger.Warn(User.Identity.Name + ". Пользователь " + user.Login + " деактивирован ");
                        }

                    }
                }
            }

            return RedirectToAction("UsersView");
        }

#endregion User

        #region Comment

        public ActionResult Comments(string searchWord, GridSortOptions gridSortOptions, int? categoryId, int? page)
         {
             //IEnumerable<Comment> comments = repositoryComment.Comments.ToList();


             int pageItemsCount = Constants.ADMIN_PAGE_SIZE;

             List<Comment> comments = new List<Comment>();
             var clist = repositoryComment.Comments.OrderByDescending(x=>x.CreateDate).ToList();
             foreach (var comment in clist)
             {

                 comments.Add(comment);
             }

             var pagedViewModel = new PagedViewModel<Comment>
             {
                 ViewData = ViewData,
                 Query = comments.AsQueryable(),//query, //repositoryProduct.Products, //repositoryProduct.Products,
                 GridSortOptions = gridSortOptions,
                 DefaultSortColumn = "CreateDate",
                 Page = page,
                 PageSize = (pageItemsCount == 0) ? Domain.Constants.ADMIN_PAGE_SIZE : pageItemsCount,
             };

             pagedViewModel
                 .AddFilter("searchWord", searchWord,
                            a =>
                            a.Text.Contains(searchWord)
                            || a.Tittle.Contains(searchWord))
                 .Setup();

             if (Request.IsAjaxRequest())
             {
                 Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                 return PartialView("CommentGridPartial", pagedViewModel);
             }

             return View(pagedViewModel);
         }

        public ViewResult EditComment(int commentId)
        {
            Comment comment = repositoryComment.Comments.FirstOrDefault(x=>x.CommentID==commentId);
            //IEnumerable<ProductImage> productImagesList = repositoryProductImages.ProductImages;
            return View(comment);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
             
                    repositoryComment.SaveComment(comment);
                    
                    return RedirectToAction("Comments", "Admin");
            }
            else
            {
                return View(comment);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin, ContentManager")]
        public ActionResult DeleteComment(int[] resubmit)
        {
             if (resubmit != null)
               {
                foreach (var p in resubmit)
                {
                    var comment = repositoryComment.Comments.FirstOrDefault(x => x.CommentID == p);//GetProductByIDAsync(p);

                    
                    if (comment != null){
                        repositoryComment.DeleteComment(comment);
                    }

                }
                TempData["message"] = string.Format("Нежелательные комментарии были удалены");
                TempData["messageType"] = "warning-msg";
            }
            

            return RedirectToAction("Comments");
        }
        

         #endregion Comment


        #region Calendar

        public ActionResult Calendar(string searchWord, GridSortOptions gridSortOptions, int? page, string startDate, string endDate)
        {
            //IEnumerable<Comment> comments = repositoryComment.Comments.ToList();
            var dt1 = "";
            var dt2 = "";
            
            if (startDate == null)
            {
              dt1 = String.Format("{0:dd.mm.yyyy}", (DateTime.Now.Date).ToShortDateString());
            }
            else
            {
              dt1 = startDate = String.Format("{0:dd.mm.yyyy}", startDate);
            }

            if (endDate == null)
            {
                dt2 = (DateTime.Now.Date.AddDays(14).ToShortDateString());
            }
            else
            {
                dt2 = String.Format("{0:dd.mm.yyyy}", endDate);
            }

            //var dt1 = startDate ?? DateTime.Now.ToShortDateString();
            TempData["startDate"] = dt1;

            //var dt2 = endDate ?? DateTime.Now.AddDays(14).ToShortDateString();
            TempData["endDate"] = dt2;


            DateTime dStart = Convert.ToDateTime(dt1);
            DateTime dEnd = Convert.ToDateTime(dt2);

            //var dt2 = (TempData["endDate"] == null) ? DateTime.Now.ToShortDateString() : (string)TempData["endDate"];

            int pageItemsCount = Constants.ADMIN_PAGE_SIZE;

            List<Calendar> calendars = new List<Calendar>();
            var dtlist = repositoryCalendar.Calendars.Where(a => a.CalendarDate >= dStart && a.CalendarDate <= dEnd).OrderByDescending(x => x.CalendarDate).ToList();
            foreach (var date in dtlist)
            {

                calendars.Add(date);
            }

            var pagedViewModel = new PagedViewModel<Calendar>
            {
                ViewData = ViewData,
                Query = calendars.AsQueryable(),//query, //repositoryProduct.Products, //repositoryProduct.Products,
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "CalendarDate",
                Page = page,
                PageSize = (pageItemsCount == 0) ? Domain.Constants.ADMIN_PAGE_SIZE : pageItemsCount,
            };

            pagedViewModel
                .AddFilter("searchWord", searchWord,
                           a =>
                           a.CalendarDescription.ToLower().Contains(searchWord.ToLower()))
                //.AddFilter("startDate", startDate, a =>)
                //.AddFilter("endDate", endDate, a => a.CalendarDate <= dEnd)
                //.AddFilter("startDate", startDate, a => a.CalendarDate >= dStart)
                //.AddFilter("endDate", endDate, a => a.CalendarDate <= dEnd)
                .Setup();

            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("CalendarGridPartial", pagedViewModel);
            }

            return View(pagedViewModel);
        }



        public ViewResult EditCalendar(int calendarId)
        {
            Calendar calendar = repositoryCalendar.Calendars.FirstOrDefault(x => x.CalendarID == calendarId);
            //IEnumerable<ProductImage> productImagesList = repositoryProductImages.ProductImages;
            return View(calendar);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditCalendar(Calendar calendar)
        {
            if (ModelState.IsValid)
            {

                repositoryCalendar.SaveCalendar(calendar);

                return RedirectToAction("Calendar", "Admin");
            }
            else
            {
                return View(calendar);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin, ContentManager")]
        public ActionResult DeleteCalendar(int[] resubmit)
        {
            if (resubmit != null)
            {
                foreach (var p in resubmit)
                {
                    var calendar = repositoryCalendar.Calendars.FirstOrDefault(x => x.CalendarID == p);//GetProductByIDAsync(p);


                    if (calendar != null)
                    {
                        repositoryCalendar.DeleteCalendar(calendar);
                    }

                }
                TempData["message"] = string.Format("Нежелательные даты  были удалены");
                TempData["messageType"] = "warning-msg";
            }


            return RedirectToAction("Calendar");
        }
        

        public ActionResult Calendar2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BookDate(int year, int month, int day)
        {
            DateTime dt = new DateTime(year, month, day);
            
                var choosenDate =
                    repositoryCalendar.Calendars.FirstOrDefault(
                        x => x.CalendarDate.Year == dt.Year & x.CalendarDate.Month == dt.Month & x.CalendarDate.Day == dt.Day);
                if (choosenDate==null)
                {
                    Calendar calendar = new Calendar() {CalendarDate = dt};
                    repositoryCalendar.SaveCalendar(calendar);
                    TempData["BookDate"] = dt;
                    return Json(new { success = "true", id = calendar.CalendarID });        
                }
                else
                {
                    repositoryCalendar.DeleteCalendar(choosenDate);
                    return Json(new { success = "false" });                
                }

            
        }

         [HttpPost]
         public ActionResult GetBookedDates(int? year)
         {
             year = (year == null) ? DateTime.Now.Year : year;
             List<DateParts> dt =
                 (from dates in repositoryCalendar.Calendars.Where(x => x.CalendarDate.Year ==year).ToList()
                  select new DateParts
                      {
                          Day = dates.CalendarDate.Day,
                          Month = dates.CalendarDate.Month,
                          Year = dates.CalendarDate.Year
                      }).ToList();
             return Json(new { success = dt });
         }

         #endregion Calendar

    }

}
