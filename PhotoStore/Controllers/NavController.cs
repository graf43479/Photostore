using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;

namespace PhotoStore.Controllers
{
    public class NavController : Controller
    {

        
        private ICategoryRepository categoryRepository;

        public NavController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        //
        // GET: /Nav/

        public ActionResult Menu(string type)
        {
         
            TempData["mobile"] = type;
            

            return View();
        }

        public ActionResult PortfolioMenu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            var categoryList = categoryRepository.Categories.Where(x=>x.IsActive).OrderBy(x=>x.Sequence).ToList();

            return View(categoryList);
        }




    }
}
