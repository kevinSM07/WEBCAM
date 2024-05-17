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
    public class ModulosController : Controller
    {

        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: Modulos
        public ActionResult Index()
        {
            return View(db.Modulo.ToList());
        }

        // GET: Modulos/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modulo modulo = db.Modulo.Find(id);
            if (modulo == null)
            {
                return HttpNotFound();
            }
            return View(modulo);
        }

        // GET: Modulos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Modulos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Descripcion")] Modulo modulo)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    modulo.Id = Guid.NewGuid();
                    db.Modulo.Add(modulo);
                    db.SaveChanges();
                    for (int i = 0; i <= 3; i++)
                    {
                        Operaciones OperacionModulo = new Operaciones();
                        switch (i)
                        {
                            case 0:
                                OperacionModulo.Nombre = "Crear";
                                break;
                            case 1:
                                OperacionModulo.Nombre = "Actualizar";
                                break;
                            case 2:
                                OperacionModulo.Nombre = "Ver";
                                break;
                            case 3:
                                OperacionModulo.Nombre = "Eliminar";
                                break;
                            default:
                                break;
                        }
                        OperacionModulo.Id = Guid.NewGuid();
                        OperacionModulo.IdModulo = modulo.Id;
                        db.Operaciones.Add(OperacionModulo);
                    }

                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue creado de manera exitosa.");
                    return RedirectToAction("Index");
                }
                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(modulo);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de resgitrar El Modulo, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: Modulos/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modulo modulo = db.Modulo.Find(id);
            if (modulo == null)
            {
                return HttpNotFound();
            }
            return View(modulo);
        }

        // POST: Modulos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Descripcion")] Modulo modulo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(modulo).State = EntityState.Modified;
                    db.SaveChanges();
                    Request.Flash("success", "El resgitro fue Editado de manera exitosa.");
                    return RedirectToAction("Index");
                }

                Request.Flash("warning", "Uno de los campos obligatorios no fue llenado correctamente ");
                return View(modulo);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de Editar El Modulo, sirvase verificar.");
                Request.Flash("danger", message: e.Message);
                return RedirectToAction("Index");
            }
        }

        // GET: Modulos/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modulo modulo = db.Modulo.Find(id);
            if (modulo == null)
            {
                return HttpNotFound();
            }
            return View(modulo);
        }

        // POST: Modulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                Modulo modulo = db.Modulo.Find(id);
                db.Modulo.Remove(modulo);
                db.SaveChanges();
                Request.Flash("success", "El resgitro fue eliminado de manera exitosa.");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Se presento un inconveniente a la hora de eliminar El Modulo, sirvase verificar.");
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
