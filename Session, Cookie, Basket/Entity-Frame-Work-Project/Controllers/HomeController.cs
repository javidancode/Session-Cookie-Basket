using Entity_Frame_Work_Project.Data;
using Entity_Frame_Work_Project.Models;
using Entity_Frame_Work_Project.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Entity_Frame_Work_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task <IActionResult> Index()
        {
            //HttpContext.Session.SetString("name", "Cavidan");
            //Response.Cookies.Append("surname", "Qedirli", new CookieOptions { MaxAge = TimeSpan.FromSeconds(25)});


            IEnumerable<Slider> sliders = await _context.Sliders.ToListAsync();
            SliderDetail sliderDetail = await _context.SliderDetails.FirstOrDefaultAsync();
            IEnumerable<Category> categories = await _context.Categories.Where(m => m.IsDeleted == false).ToListAsync();
            IEnumerable<Product> products = await _context.Products
                .Where(m => m.IsDeleted == false)
                .Include(m => m.Category)
                .Include(m => m.ProductImages).Take(8).ToListAsync();

            HomeVM model = new HomeVM
            {
                Sliders = sliders,
                SliderDetail = sliderDetail,
                Categories = categories,
                Products = products

            };

            return View(model);
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null) return BadRequest();

            //var dbProduct = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            var dbProduct = await _context.Products.FindAsync(id);

            if (dbProduct == null) return NotFound();


            List<BasketVM> basket;

            if (Request.Cookies["basket"] != null) 
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }

            basket.Add(new BasketVM
            {
                Id = dbProduct.Id,
                Count = 1
            });


            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));

            return RedirectToAction("Index");
        
        }













        //public IActionResult Test()
        //{
        //    var sessionData = HttpContext.Session.GetString("name");
        //    var cookieData = Request.Cookies["surname"];
        //    return Json(sessionData +"-"+ cookieData);
        
        //}
       
    }
}
