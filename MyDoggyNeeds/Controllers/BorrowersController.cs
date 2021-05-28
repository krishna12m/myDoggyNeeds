using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyDoggyNeeds.Models;
using MyDoggyNeeds.ViewModel;

namespace MyDoggyNeeds.Controllers
{
    public class BorrowersController : Controller
    {
        private Entities db = new Entities();

        // GET: Borrowers
        public ActionResult Index()
        {
            var borrower = db.Borrower.Include(b => b.AspNetUsers);

            string userId = User.Identity.GetUserId();


            borrower = from b in db.Borrower select b;
            borrower = borrower.Where(o => o.Uid == userId);

            return View(borrower.ToList());
        }

        // GET: Borrowers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrower borrower = db.Borrower.Find(id);
            if (borrower == null)
            {
                return HttpNotFound();
            }
            return View(borrower);
        }


        //Get: userid 
        [HttpGet]
        public ActionResult GetId()
        {

            var Borrower = new Borrower
            {
                Uid = User.Identity.GetUserId()
            };

            ViewBag.Uid = new SelectList(db.AspNetUsers, "Id", " Email");


            return Json(Borrower.Uid, JsonRequestBehavior.AllowGet);
        }



        // GET: Borrowers/Create
        public ActionResult Create()
        {

            var needsvm = new BorrowerViewModel();

            var allNeedsList = db.Needs.ToList();

            ViewBag.AllBorrowerNeeds = allNeedsList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.Id.ToString()
            });

            return View();
        }

        // POST: Borrowers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        //[Bind(Include = "Id,Fname,Lname,Dob,Line1,City,Postcode,Email,Phone,Identity,Description,Uid")] Borrower borrower
        public ActionResult Create(BorrowerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var needToAdd = db.Borrower.Include(i => i.Needs).First();

                if (TryUpdateModel(needToAdd, "Borrower", new string[] { "Id", "Fname", "Lname", "Dob", "Line1", "City", "Postcode", "Email", "Phone", "Identity", "Description", "Uid" }))
                {
                    var updateNeeds = new HashSet<int>(model.SelectedBorrowerNeeds);
                    foreach (Needs need in db.Needs)
                    {
                        if (!updateNeeds.Contains(need.Id))
                        {
                            needToAdd.Needs.Remove(need);
                        }
                        else
                        {
                            needToAdd.Needs.Add(need);
                        }
                    }

                    db.Borrower.Add(needToAdd);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
               
            }

            ViewBag.Uid = new SelectList(db.AspNetUsers, "Id", "Email", model.Borrower.Uid);
            return View(model);
        }

        // GET: Borrowers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var BorrowerViewModel = new BorrowerViewModel
            {
                Borrower = db.Borrower.Include(o => o.Needs).First(o => o.Id == id),
            };
            if (BorrowerViewModel.Borrower == null)
                return HttpNotFound();

            var allBorrowerNeedList = db.Needs.ToList();

            BorrowerViewModel.AllBorrowerNeeds = allBorrowerNeedList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.Id.ToString()

            });
            ViewBag.Uid = new SelectList(db.AspNetUsers, "Id", "Email", BorrowerViewModel.Borrower.Uid);

            //Borrower borrower = db.Borrower.Find(id);
            //if (borrower == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.Uid = new SelectList(db.AspNetUsers, "Id", "Email", borrower.Uid);
            return View(BorrowerViewModel);
        }

        // POST: Borrowers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       // [Bind(Include = "Id,Fname,Lname,Dob,Line1,City,Postcode,Email,Phone,Identity,Description,Uid")] Borrower borrower
        public ActionResult Edit(BorrowerViewModel model)
        {
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                var needToUpdate = db.Borrower.Include(o => o.Needs).First(o => o.Id == model.Borrower.Id);
                if (TryUpdateModel(needToUpdate, "Borrower", new string[] { "Id", "Fname", "Lname", "Dob", "Line1", "City", "Postcode", "Email", "Phone", "Identity", "Description", "Uid" }))

                {
                    var newNeeds = db.Needs.Where(m => model.SelectedBorrowerNeeds.Contains(m.Id)).ToList();
                    var updateNeeds = new HashSet<int>(model.SelectedBorrowerNeeds);
                    foreach (Needs need in db.Needs)
                    {
                        if (!updateNeeds.Contains(need.Id))
                        {
                            needToUpdate.Needs.Remove(need);
                        }
                        else
                        {
                            needToUpdate.Needs.Add((need));
                        }
                    }
                    db.Entry(needToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("index");
             
            }
            ViewBag.Uid = new SelectList(db.AspNetUsers, "Id", "Email", model.Borrower.Uid);
            return View(model);
        }

        // GET: Borrowers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrower borrower = db.Borrower.Find(id);
            if (borrower == null)
            {
                return HttpNotFound();
            }
            return View(borrower);
        }

        // POST: Borrowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Borrower borrower = db.Borrower.Find(id);
            db.Borrower.Remove(borrower);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
