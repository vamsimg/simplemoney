using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleMoney.Models;

namespace SimpleMoney.Controllers
{
     [Authorize(Roles = "admin")]
    public class LenderEmployeeController : Controller
    {
        private EntitiesContext db = new EntitiesContext();

        //
        // GET: /LenderEmployee/

        public ActionResult Index()
        {
            var lenderemployees = db.LenderEmployees.Include(l => l.Employee).Include(l => l.Employer);
            return View(lenderemployees.ToList());
        }

        //
        // GET: /LenderEmployee/Details/5

        public ActionResult Details(int id = 0)
        {
            LenderEmployee lenderemployee = db.LenderEmployees.Find(id);
            if (lenderemployee == null)
            {
                return HttpNotFound();
            }
            return View(lenderemployee);
        }

        //
        // GET: /LenderEmployee/Create

        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.UserProfiles, "UserId", "Email");
            ViewBag.LenderID = new SelectList(db.Lenders, "LenderID", "Name");
            return View();
        }

        //
        // POST: /LenderEmployee/Create

        [HttpPost]
        public ActionResult Create(LenderEmployee lenderemployee)
        {
            if (ModelState.IsValid)
            {
                db.LenderEmployees.Add(lenderemployee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.UserProfiles, "UserId", "Email", lenderemployee.UserID);
            ViewBag.LenderID = new SelectList(db.Lenders, "LenderID", "Name", lenderemployee.LenderID);
            return View(lenderemployee);
        }

        //
        // GET: /LenderEmployee/Edit/5

        public ActionResult Edit(int id = 0)
        {
            LenderEmployee lenderemployee = db.LenderEmployees.Find(id);
            if (lenderemployee == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.UserProfiles, "UserId", "Email", lenderemployee.UserID);
            ViewBag.LenderID = new SelectList(db.Lenders, "LenderID", "Name", lenderemployee.LenderID);
            return View(lenderemployee);
        }

        //
        // POST: /LenderEmployee/Edit/5

        [HttpPost]
        public ActionResult Edit(LenderEmployee lenderemployee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lenderemployee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.UserProfiles, "UserId", "Email", lenderemployee.UserID);
            ViewBag.LenderID = new SelectList(db.Lenders, "LenderID", "Name", lenderemployee.LenderID);
            return View(lenderemployee);
        }

        //
        // GET: /LenderEmployee/Delete/5

        public ActionResult Delete(int id = 0)
        {
            LenderEmployee lenderemployee = db.LenderEmployees.Find(id);
            if (lenderemployee == null)
            {
                return HttpNotFound();
            }
            return View(lenderemployee);
        }

        //
        // POST: /LenderEmployee/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            LenderEmployee lenderemployee = db.LenderEmployees.Find(id);
            db.LenderEmployees.Remove(lenderemployee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}