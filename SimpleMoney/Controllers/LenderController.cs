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
     public class LenderController : Controller
     {
          private EntitiesContext db = new EntitiesContext();

          //
          // GET: /Lender/

          public ActionResult Index()
          {
               return View(db.Lenders.ToList());
          }

          //
          // GET: /Lender/Details/5

          public ActionResult Details(int id = 0)
          {
               Lender lender = db.Lenders.Find(id);
               if (lender == null)
               {
                    return HttpNotFound();
               }
               return View(lender);
          }

          //
          // GET: /Lender/Create

          public ActionResult Create()
          {
               return View();
          }

          //
          // POST: /Lender/Create

          [HttpPost]
          public ActionResult Create(Lender lender)
          {
               if (ModelState.IsValid)
               {
                    db.Lenders.Add(lender);
                    db.SaveChanges();
                    return RedirectToAction("Index");
               }

               return View(lender);
          }

          //
          // GET: /Lender/Edit/5

          public ActionResult Edit(int id = 0)
          {
               Lender lender = db.Lenders.Find(id);
               if (lender == null)
               {
                    return HttpNotFound();
               }
               return View(lender);
          }

          //
          // POST: /Lender/Edit/5

          [HttpPost]
          public ActionResult Edit(Lender lender)
          {
               if (ModelState.IsValid)
               {
                    db.Entry(lender).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
               }
               return View(lender);
          }

          //
          // GET: /Lender/Delete/5

          public ActionResult Delete(int id = 0)
          {
               Lender lender = db.Lenders.Find(id);
               if (lender == null)
               {
                    return HttpNotFound();
               }
               return View(lender);
          }

          //
          // POST: /Lender/Delete/5

          [HttpPost, ActionName("Delete")]
          public ActionResult DeleteConfirmed(int id)
          {
               Lender lender = db.Lenders.Find(id);
               db.Lenders.Remove(lender);
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