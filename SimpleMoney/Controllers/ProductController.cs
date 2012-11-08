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
     public class ProductController : Controller
     {
         
          private EntitiesContext db = new EntitiesContext();

          //
          // GET: /Product/

          public ActionResult Index()
          {
               var products = db.Products.Include(p => p.Lender);
               return View(products.ToList());
          }

          //
          // GET: /Product/Details/5

          public ActionResult Details(int id = 0)
          {
               Product product = db.Products.Find(id);
               if (product == null)
               {
                    return HttpNotFound();
               }
               return View(product);
          }

          //
          // GET: /Product/Create

          public ActionResult Create()
          {
               ViewBag.LenderID = new SelectList(db.Lenders, "LenderID", "Name");
               return View();
          }

          //
          // POST: /Product/Create

          [HttpPost]
          public ActionResult Create(Product product)
          {
               if (ModelState.IsValid)
               {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
               }

               ViewBag.LenderID = new SelectList(db.Lenders, "LenderID", "Name", product.LenderID);
               return View(product);
          }

          //
          // GET: /Product/Edit/5

          public ActionResult Edit(int id = 0)
          {
               Product product = db.Products.Find(id);
               if (product == null)
               {
                    return HttpNotFound();
               }
               ViewBag.LenderID = new SelectList(db.Lenders, "LenderID", "Name", product.LenderID);
               return View(product);
          }

          //
          // POST: /Product/Edit/5

          [HttpPost]
          public ActionResult Edit(Product product)
          {
               if (ModelState.IsValid)
               {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
               }
               ViewBag.LenderID = new SelectList(db.Lenders, "LenderID", "Name", product.LenderID);
               return View(product);
          }

          //
          // GET: /Product/Delete/5

          public ActionResult Delete(int id = 0)
          {
               Product product = db.Products.Find(id);
               if (product == null)
               {
                    return HttpNotFound();
               }
               return View(product);
          }

          //
          // POST: /Product/Delete/5

          [HttpPost, ActionName("Delete")]
          public ActionResult DeleteConfirmed(int id)
          {
               Product product = db.Products.Find(id);
               db.Products.Remove(product);
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