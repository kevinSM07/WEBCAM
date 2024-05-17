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
    public class PaisesController : Controller
    {

        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: Paises
        public ActionResult Index()
        {
            return View(db.TblPaises.ToList());
        }

        // GET: Paises/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblPaises tblPaises = db.TblPaises.Find(id);
            if (tblPaises == null)
            {
                return HttpNotFound();
            }
            return View(tblPaises);
        }

        // GET: Paises/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Paises/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre")] TblPaises tblPaises)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tblPaises.Id = Guid.NewGuid();
                    db.TblPaises.Add(tblPaises);
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue creado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(tblPaises);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de resgitrar el Pais, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: Paises/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblPaises tblPaises = db.TblPaises.Find(id);
            if (tblPaises == null)
            {
                return HttpNotFound();
            }
            return View(tblPaises);
        }

        // POST: Paises/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre")] TblPaises tblPaises)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tblPaises).State = EntityState.Modified;
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue editado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(tblPaises);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de editar el Pais, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: Paises/Delete/5
        public ActionResult Delete(Guid? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblPaises tblPaises = db.TblPaises.Find(id);
            if (tblPaises == null)
            {
                return HttpNotFound();
            }
            return View(tblPaises);
        }

        // POST: Paises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                TblPaises tblPaises = db.TblPaises.Find(id);
                db.TblPaises.Remove(tblPaises);
                db.SaveChanges();
                Request.Flash("success", "El resgitro fue eliminado de manera exitosa.");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de eliminar el Pais, sirvase verificar.");
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
