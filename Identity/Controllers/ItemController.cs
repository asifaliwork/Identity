using Identity.Data;
using Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class ItemController : Controller
    {

        private readonly ApplicationDbContext _db;
        public ItemController(ApplicationDbContext db)
        {
            _db = db;

        }


        public IActionResult Index()
        {
            IEnumerable<Item> items = _db.items.ToList();
            return View(items);
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( Item obj)
        {
            if(ModelState.IsValid)
            {
                _db.items.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }



        public IActionResult Update(int id)
        {




            if (id == null )
            {
                return NotFound();
            }
            var item = _db.items.SingleOrDefault(x => x.Id == id);
            if(item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Item obj)
        {
            if (ModelState.IsValid)
            {
                _db.items.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(obj);
        }

        public IActionResult Delete(int id) 
        {

            if (id == null)
            {
                return NotFound();
            }
            var item = _db.items.SingleOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteById(int id)
        {
             var item = _db.items.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            
             _db.items.Remove(item);
             _db.SaveChanges();

             return RedirectToAction("Index");

        }

    }
}
