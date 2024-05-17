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
    public class GenerosController : Controller
    {

        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: Generos
        public ActionResult Index()
        {
            return View(db.TblGeneros.ToList());
        }

        // GET: Generos/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblGeneros tblGeneros = db.TblGeneros.Find(id);
            if (tblGeneros == null)
            {
                return HttpNotFound();
            }
            return View(tblGeneros);
        }

        // GET: Generos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Generos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Observacion")] TblGeneros tblGeneros)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tblGeneros.Id = Guid.NewGuid();
                    db.TblGeneros.Add(tblGeneros);
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue creado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(tblGeneros);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de resgitrar el genero, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }
        }

        // GET: Generos/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblGeneros tblGeneros = db.TblGeneros.Find(id);
            if (tblGeneros == null)
            {
                return HttpNotFound();
            }
            return View(tblGeneros);
        }

        // POST: Generos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Observacion")] TblGeneros tblGeneros)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tblGeneros).State = EntityState.Modified;
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue Editado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(tblGeneros);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de Editar el genero, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }


        }

        // GET: Generos/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblGeneros tblGeneros = db.TblGeneros.Find(id);
            if (tblGeneros == null)
            {
                return HttpNotFound();
            }
            return View(tblGeneros);
        }

        // POST: Generos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                TblGeneros tblGeneros = db.TblGeneros.Find(id);
                db.TblGeneros.Remove(tblGeneros);
                db.SaveChanges();
                Request.Flash("success", "El resgitro fue Eliminado de manera exitosa.");
                return RedirectToAction("Index");


            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de Eliminar el Genero, sirvase verificar.");
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
