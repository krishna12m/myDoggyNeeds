using MyDoggyNeeds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyDoggyNeeds.Controllers
{
    public class DogListController : Controller
    {
        Entities db = new Entities();

        // GET: DogList
        public ActionResult Index()
        {
            return View(db.Dog.ToList());
        }

        // GET: Dogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dog dog = db.Dog.Find(id);
            if (dog == null)
            {
                return HttpNotFound();
            }
            return View(dog);
        }
    }
}