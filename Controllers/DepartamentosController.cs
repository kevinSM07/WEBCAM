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
    public class DepartamentosController : Controller
    {

        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: Departamentos
        public ActionResult Index()
        {
            var tblDepartamentos = db.TblDepartamentos.Include(t => t.TblPaises);
            return View(tblDepartamentos.ToList());
        }

        // GET: Departamentos/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblDepartamentos tblDepartamentos = db.TblDepartamentos.Find(id);
            if (tblDepartamentos == null)
            {
                return HttpNotFound();
            }
            return View(tblDepartamentos);
        }

        // GET: Departamentos/Create
        public ActionResult Create()
        {
            ViewBag.IdPais = new SelectList(db.TblPaises, "Id", "Nombre");
            return View();
        }

        // POST: Departamentos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,IdPais")] TblDepartamentos tblDepartamentos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tblDepartamentos.Id = Guid.NewGuid();
                    db.TblDepartamentos.Add(tblDepartamentos);
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue creado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                ViewBag.IdPais = new SelectList(db.TblPaises, "Id", "Nombre", tblDepartamentos.IdPais);
                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(tblDepartamentos);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de resgitrar el Departamento , sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: Departamentos/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblDepartamentos tblDepartamentos = db.TblDepartamentos.Find(id);
            if (tblDepartamentos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPais = new SelectList(db.TblPaises, "Id", "Nombre", tblDepartamentos.IdPais);
            return View(tblDepartamentos);
        }

        // POST: Departamentos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,IdPais")] TblDepartamentos tblDepartamentos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tblDepartamentos).State = EntityState.Modified;
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue editado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                ViewBag.IdPais = new SelectList(db.TblPaises, "Id", "Nombre", tblDepartamentos.IdPais);
                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(tblDepartamentos);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de editar el Departamento, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: Departamentos/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblDepartamentos tblDepartamentos = db.TblDepartamentos.Find(id);
            if (tblDepartamentos == null)
            {
                return HttpNotFound();
            }
            return View(tblDepartamentos);
        }

        // POST: Departamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                TblDepartamentos tblDepartamentos = db.TblDepartamentos.Find(id);
                db.TblDepartamentos.Remove(tblDepartamentos);
                db.SaveChanges();
                Request.Flash("success", "El resgitro fue eliminado de manera exitosa.");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de eliminar el Departamento, sirvase verificar.");
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
