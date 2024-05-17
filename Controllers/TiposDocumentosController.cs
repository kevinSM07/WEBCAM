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
    public class TiposDocumentosController : Controller
    {

        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: TiposDocumentos
        public ActionResult Index()
        {
            return View(db.TblTiposDocumentos.ToList());
        }

        // GET: TiposDocumentos/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblTiposDocumentos tblTiposDocumentos = db.TblTiposDocumentos.Find(id);
            if (tblTiposDocumentos == null)
            {
                return HttpNotFound();
            }
            return View(tblTiposDocumentos);
        }

        // GET: TiposDocumentos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposDocumentos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre")] TblTiposDocumentos tblTiposDocumentos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tblTiposDocumentos.Id = Guid.NewGuid();
                    db.TblTiposDocumentos.Add(tblTiposDocumentos);
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue creado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(tblTiposDocumentos);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de resgitrar el nuevo Documento, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }
        }

        // GET: TiposDocumentos/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblTiposDocumentos tblTiposDocumentos = db.TblTiposDocumentos.Find(id);
            if (tblTiposDocumentos == null)
            {
                return HttpNotFound();
            }
            return View(tblTiposDocumentos);
        }

        // POST: TiposDocumentos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre")] TblTiposDocumentos tblTiposDocumentos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tblTiposDocumentos).State = EntityState.Modified;
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue editado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(tblTiposDocumentos);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de editar el Documento, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: TiposDocumentos/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblTiposDocumentos tblTiposDocumentos = db.TblTiposDocumentos.Find(id);
            if (tblTiposDocumentos == null)
            {
                return HttpNotFound();
            }
            return View(tblTiposDocumentos);
        }

        // POST: TiposDocumentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                TblTiposDocumentos tblTiposDocumentos = db.TblTiposDocumentos.Find(id);
                db.TblTiposDocumentos.Remove(tblTiposDocumentos);
                db.SaveChanges();
                Request.Flash("success", "El resgitro fue eliminado de manera exitosa.");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de eliminar el Documento, sirvase verificar.");
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
