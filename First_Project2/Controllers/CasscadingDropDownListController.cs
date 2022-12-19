using First_Project2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace First_Project2.Controllers
{
    public class CasscadingDropDownListController : Controller
    {

        private readonly ModelContext _context;

        public CasscadingDropDownListController(ModelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// For casscading DropDown List 
        /// </summary>

        public JsonResult Category()
        {
            var cat = _context.CategoryHalls.ToList();
            return new JsonResult(cat);
        }

        public JsonResult Hall(int id)
        {
            var hal = _context.Halls.Where(x => x.CategoryId == id).ToList();
            return new JsonResult(hal);
        }

    }
}
