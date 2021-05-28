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
    public class OwnersController : Controller
    {
        private Entities db = new Entities();

        // GET: Owners
        public ActionResult Index()
        {
            var owner = db.Owner.Include(o => o.AspNetUsers);
            
            string userId = User.Identity.GetUserId();


            owner = from o in db.Owner select o;
            owner = owner.Where(o => o.Uid == userId);
           
            return View(owner.ToList());

        }

        // GET: Owners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owner.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }



        [HttpGet]
        public ActionResult GetId()
        {

            var Owner = new Owner
            {
                Uid = User.Identity.GetUserId()
            };

            ViewBag.Uid = new SelectList(db.AspNetUsers, "Id", " Email");


            return Json(Owner.Uid, JsonRequestBehavior.AllowGet);
        }

        // GET: Owners/Create
        public ActionResult Create()
        {
            var needsvm = new OwnerViewModel();

            var allNeedsList = db.Needs.ToList();

            ViewBag.AllOwnerNeeds = allNeedsList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.Id.ToString()
            });

            var owner = new Owner()
            {
                Uid = User.Identity.GetUserId()
            };

            ViewBag.Uid = new SelectList(db.AspNetUsers, "Id", "Email");

            return View();
        }

        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "Id,Fname,Lname,Dob,Line1,City,Postcode,Email,Phone,Uid")] Owner owner
        public ActionResult Create(OwnerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var needToAdd = db.Owner.Include(i => i.Needs).First();

                if (TryUpdateModel(needToAdd, "Owner", new string[] { "Id", "Fname", "Lname", "Dob", "Line1", "City", "Postcode", "Email", "Phone", "Uid"}))
                {
                    var updateNeeds = new HashSet<int>(model.SelectedOwnerNeeds);
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

                    db.Owner.Add(needToAdd);
                    db.SaveChanges();
                }

                return RedirectToAction("Create","Dogs");
            }

            ViewBag.Uid = new SelectList(db.AspNetUsers, "Id", "Email", model.Owner.Uid);

            return View(model);
        }

        // GET: Owners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var OwnerViewModel = new OwnerViewModel
            {
                Owner = db.Owner.Include(o => o.Needs).First(o => o.Id == id),
            };
            if (OwnerViewModel.Owner == null)
                return HttpNotFound();

            var allOwnerNeedList = db.Needs.ToList();

            OwnerViewModel.AllOwnerNeeds = allOwnerNeedList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.Id.ToString()

            });
            ViewBag.Uid = new SelectList(db.AspNetUsers, "Id", "Email", OwnerViewModel.Owner.Uid);


            //Owner owner = db.Owner.Find(id);
            //if (owner == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.Uid = new SelectList(db.AspNetUsers, "Id", "Email", owner.Uid);
            return View(OwnerViewModel);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "Id,Fname,Lname,Dob,Line1,City,Postcode,Email,Phone,Uid")] Owner owner
        public ActionResult Edit(OwnerViewModel model)
        {
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                var needToUpdate = db.Owner.Include(o => o.Needs).First(o => o.Id == model.Owner.Id);
                if (TryUpdateModel(needToUpdate, "Owner", new string[] { "Id","Fname","Lname", "Dob","Line1", "City", "Postcode", "Email", "Phone", "Uid" }))

                {
                    var newNeeds = db.Needs.Where(m => model.SelectedOwnerNeeds.Contains(m.Id)).ToList();
                    var updateNeeds = new HashSet<int>(model.SelectedOwnerNeeds);
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
            ViewBag.Uid = new SelectList(db.AspNetUsers, "Id", "Email", model.Owner.Uid);
            return View(model);
        }

        // GET: Owners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owner.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Owner owner = db.Owner.Find(id);
            db.Owner.Remove(owner);
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
