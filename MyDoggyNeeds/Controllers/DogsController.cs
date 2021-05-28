using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyDoggyNeeds.Models;

namespace MyDoggyNeeds.Controllers
{
    public class DogsController : Controller
    {
        private Entities db = new Entities();

        // GET: Dogs
        public ActionResult Index()
        {

            var dog = db.Dog.Include(d => d.Owner);

            string userId = User.Identity.GetUserId();

           
           var ownerID = (from o in db.Owner where o.Uid == userId select o.Id).FirstOrDefault();

            dog = from d in db.Dog select d;
            dog = dog.Where(d => d.Oid == ownerID);

            return View(dog.ToList());
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

        // GET: Dogs/Create
        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();
            var dog = new Dog()
            {
               Oid = (from o in db.Owner where o.Uid == userId select o.Id).FirstOrDefault()
            };

            ViewBag.Oid = new SelectList(db.Owner, "Id", "Fname");
            return View(dog);
        }

        // POST: Dogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DOB,Size,Breed,Image,Description,Oid,ImageFile")] Dog dog)
        {
            if (ModelState.IsValid)
            {
                if (dog.Image == null)
                {
                    dog.Image = "~/Content/Images/dog.jpg";
                }
                string imagename = Path.GetFileNameWithoutExtension(dog.ImageFile.FileName); //image ex imagename
                string extension = Path.GetExtension(dog.ImageFile.FileName); //extention ex .jpg
                imagename += extension; //combine both image and extention
                dog.Image = imagename; //that goes to gatabase;

                imagename = Path.Combine(Server.MapPath("~/Content/Images/"), imagename);

                dog.ImageFile.SaveAs(imagename);
                db.Dog.Add(dog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Oid = new SelectList(db.Owner, "Id", "Fname", dog.Oid);
            return View(dog);
        }

        // GET: Dogs/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Oid = new SelectList(db.Owner, "Id", "Fname", dog.Oid);
            return View(dog);
        }

        // POST: Dogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DOB,Size,Breed,Image,Description,ImageFile,Oid")] Dog dog)
        {
            if (ModelState.IsValid)
            {
                string imagename = Path.GetFileNameWithoutExtension(dog.ImageFile.FileName); //image ex imagename
                string extension = Path.GetExtension(dog.ImageFile.FileName); //extention ex .jpg
                imagename += extension; //combine both image and extention
                dog.Image = imagename; //that goes to gatabase;

                imagename = Path.Combine(Server.MapPath("~/Content/Images/"), imagename);

                dog.ImageFile.SaveAs(imagename);

                db.Entry(dog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Oid = new SelectList(db.Owner, "Id", "Fname", dog.Oid);
            return View(dog);
        }

        // GET: Dogs/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Dogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dog dog = db.Dog.Find(id);
            db.Dog.Remove(dog);
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
