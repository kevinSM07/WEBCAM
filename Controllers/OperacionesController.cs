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
    public class OperacionesController : Controller
    {


        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: Operaciones
        public ActionResult Index()
        {
            var operaciones = db.Operaciones.Include(o => o.Modulo);
            return View(operaciones.ToList());
        }

        // GET: Operaciones/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operaciones operaciones = db.Operaciones.Find(id);
            if (operaciones == null)
            {
                return HttpNotFound();
            }
            return View(operaciones);
        }

        // GET: Operaciones/Create
        public ActionResult Create()
        {
            ViewBag.IdModulo = new SelectList(db.Modulo, "Id", "Nombre");
            return View();
        }

        // POST: Operaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,IdModulo")] Operaciones operaciones)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    operaciones.Id = Guid.NewGuid();
                    db.Operaciones.Add(operaciones);
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue creado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                ViewBag.IdModulo = new SelectList(db.Modulo, "Id", "Nombre", operaciones.IdModulo);
                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(operaciones);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de resgitrar La Operacion, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: Operaciones/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operaciones operaciones = db.Operaciones.Find(id);
            if (operaciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdModulo = new SelectList(db.Modulo, "Id", "Nombre", operaciones.IdModulo);
            return View(operaciones);
        }

        // POST: Operaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,IdModulo")] Operaciones operaciones)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(operaciones).State = EntityState.Modified;
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue Editado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                ViewBag.IdModulo = new SelectList(db.Modulo, "Id", "Nombre", operaciones.IdModulo);
                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(operaciones);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de Editar La operacione, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: Operaciones/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operaciones operaciones = db.Operaciones.Find(id);
            if (operaciones == null)
            {
                return HttpNotFound();
            }
            return View(operaciones);
        }

        // POST: Operaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                Operaciones operaciones = db.Operaciones.Find(id);
                db.Operaciones.Remove(operaciones);
                db.SaveChanges();
                Request.Flash("success", "El resgitro fue eliminado de manera exitosa.");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de eliminar La Operacion, sirvase verificar.");
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
