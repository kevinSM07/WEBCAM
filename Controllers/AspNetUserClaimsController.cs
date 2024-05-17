using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WEBCAM.Context;

namespace WEBCAM.Controllers
{
    [Authorize]
    public class AspNetUserClaimsController : Controller
    {

        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: AspNetUserClaims
        public ActionResult Index()
        {
            var aspNetUserClaims = db.AspNetUserClaims.Include(a => a.AspNetUsers);
            return View(aspNetUserClaims.ToList());
        }

        // GET: AspNetUserClaims/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserClaims aspNetUserClaims = db.AspNetUserClaims.Find(id);
            if (aspNetUserClaims == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserClaims);
        }

        // GET: AspNetUserClaims/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AspNetUserClaims/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,ClaimType,ClaimValue")] AspNetUserClaims aspNetUserClaims)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.AspNetUserClaims.Add(aspNetUserClaims);
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue creado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUserClaims.UserId);
                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(aspNetUserClaims);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de resgitrar el nuevo producto, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: AspNetUserClaims/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserClaims aspNetUserClaims = db.AspNetUserClaims.Find(id);
            if (aspNetUserClaims == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUserClaims.UserId);
            return View(aspNetUserClaims);
        }

        // POST: AspNetUserClaims/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,ClaimType,ClaimValue")] AspNetUserClaims aspNetUserClaims)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aspNetUserClaims).State = EntityState.Modified;
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue Editado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUserClaims.UserId);
                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(aspNetUserClaims);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de Editar el producto, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: AspNetUserClaims/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserClaims aspNetUserClaims = db.AspNetUserClaims.Find(id);
            if (aspNetUserClaims == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserClaims);
        }

        // POST: AspNetUserClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {
                AspNetUserClaims aspNetUserClaims = db.AspNetUserClaims.Find(id);
                db.AspNetUserClaims.Remove(aspNetUserClaims);
                db.SaveChanges();
                Request.Flash("success", "El resgitro fue eliminado de manera exitosa.");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de eliminar el producto, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }
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
