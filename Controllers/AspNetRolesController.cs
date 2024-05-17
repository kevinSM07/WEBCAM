using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WEBCAM.Context;
using WEBCAM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Xml.Linq;

namespace WEBCAM.Controllers
{
    [Authorize]
    public class AspNetRolesController : Controller
    {
        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: AspNetRoles
        public ActionResult Index()
        {
            return View(db.AspNetRoles.ToList());
        }

        // GET: AspNetRoles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRoles);
        }

        // GET: AspNetRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetRoles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] AspNetRoles aspNetRoles)
        {
            using (ApplicationDbContext defaultdb = new ApplicationDbContext())
            {
                var rolmanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(defaultdb));
                var resultado = rolmanager.Create(new IdentityRole(aspNetRoles.Name));
            }


            return View(aspNetRoles);
        }

        // GET: AspNetRoles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRoles);
        }

        // POST: AspNetRoles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AspNetRoles aspNetRoles)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aspNetRoles).State = EntityState.Modified;
                    db.SaveChanges();
                    Request.Flash("success", "El registro se edito exitosamente");
                    return RedirectToAction("Index");
                }
                return View(aspNetRoles);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Hubo un error editando el registro, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return View("Index");
            }

        }

        // GET: AspNetRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRoles);
        }

        // POST: AspNetRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
                db.AspNetRoles.Remove(aspNetRoles);
                db.SaveChanges();
                Request.Flash("success", "El registro se elimino exitosamente");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Hubo un error al eliminando el registro.");
                Request.Flash("danger", message: e.Message);
                return View("Index");
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
