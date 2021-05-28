using MyDoggyNeeds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyDoggyNeeds.Controllers
{
    public class BorrowerListController : Controller
    {
        Entities db = new Entities();

        // GET: BorrowerList
        public ActionResult Index()
        {
            return View(db.Borrower.ToList());
        }
    }
}