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
    public class CiudadesController : Controller
    {
        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: Ciudades
        public ActionResult Index()
        {
            var tblCiudades = db.TblCiudades.Include(t => t.TblDepartamentos);
            return View(tblCiudades.ToList());
        }

        // GET: Ciudades/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblCiudades tblCiudades = db.TblCiudades.Find(id);
            if (tblCiudades == null)
            {
                return HttpNotFound();
            }
            return View(tblCiudades);
        }

        // GET: Ciudades/Create
        public ActionResult Create()
        {
            ViewBag.IdDepartamento = new SelectList(db.TblDepartamentos, "Id", "Nombre");
            return View();
        }

        // POST: Ciudades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,IdDepartamento,Codigo")] TblCiudades tblCiudades)
        {
            try
            {
                List<TblCiudades> tblCiudad = db.TblCiudades.ToList();
                if (ModelState.IsValid)
                {
                    tblCiudades.Id = Guid.NewGuid();
                    db.TblCiudades.Add(tblCiudades);
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue creado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                ViewBag.IdDepartamento = new SelectList(db.TblDepartamentos, "Id", "Nombre", tblCiudades.IdDepartamento);
                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(tblCiudades);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de resgitrar la Ciudad, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: Ciudades/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblCiudades tblCiudades = db.TblCiudades.Find(id);
            if (tblCiudades == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDepartamento = new SelectList(db.TblDepartamentos, "Id", "Nombre", tblCiudades.IdDepartamento);
            return View(tblCiudades);
        }

        // POST: Ciudades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,IdDepartamento,Codigo")] TblCiudades tblCiudades)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tblCiudades).State = EntityState.Modified;
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue editado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                ViewBag.IdDepartamento = new SelectList(db.TblDepartamentos, "Id", "Nombre", tblCiudades.IdDepartamento);
                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(tblCiudades);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de editar la Ciudad, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: Ciudades/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblCiudades tblCiudades = db.TblCiudades.Find(id);
            if (tblCiudades == null)
            {
                return HttpNotFound();
            }
            return View(tblCiudades);
        }

        // POST: Ciudades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                TblCiudades tblCiudades = db.TblCiudades.Find(id);
                db.TblCiudades.Remove(tblCiudades);
                db.SaveChanges();
                Request.Flash("success", "El resgitro fue eliminado de manera exitosa.");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de eliminar La Ciudad, sirvase verificar.");
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
