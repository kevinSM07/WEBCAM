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
    public class RolOperacionsController : Controller
    {

        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: RolOperacions
        public ActionResult Index()
        {
            var rolOperacion = db.RolOperacion.Include(r => r.AspNetRoles).Include(r => r.Operaciones);
            return View(rolOperacion.ToList());
        }

        // GET: RolOperacions/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOperacion rolOperacion = db.RolOperacion.Find(id);
            if (rolOperacion == null)
            {
                return HttpNotFound();
            }
            return View(rolOperacion);
        }

        // GET: RolOperacions/Create
        public ActionResult Create(Guid id)
        {
            List<RolOperacion> ListarOlOperacion = new List<RolOperacion>();
            RolOperacion rolOperacion = new RolOperacion();
            rolOperacion.ListadoModulos = new List<Modulo>();
            rolOperacion.ListadoOperaciones = new List<Operaciones>();
            ViewBag.IdRol = db.AspNetRoles.FirstOrDefault(m => m.Id == id.ToString());
            ViewBag.Modulo = new SelectList(db.Modulo, "Id", "Nombre", ViewBag.SelectedModulo);
            string SelectedModulo = ViewBag.SelectedModulo;
            foreach (var item in ViewBag.Modulo)
            {
                Modulo Moduloitem = new Modulo();
                string Modulo = item.Value;
                Moduloitem = db.Modulo.FirstOrDefault(m => m.Id == new Guid(Modulo));
                rolOperacion.ListadoModulos.Add(Moduloitem);
            }

            ViewBag.IdOperacion = new SelectList(db.Operaciones, "Id", "Nombre", rolOperacion.IdOperacion);
            ViewBag.RolOperaciones = db.RolOperacion;
            foreach (var item in ViewBag.IdOperacion)
            {
                Operaciones Operacionesitem = new Operaciones();
                string Operaciones = item.Value;
                Operacionesitem = db.Operaciones.FirstOrDefault(m => m.Id == new Guid(Operaciones));
                rolOperacion.ListadoOperaciones.Add(Operacionesitem);
            }
            ListarOlOperacion.Add(rolOperacion);
            foreach (var item in db.RolOperacion)
            {
                ListarOlOperacion.Add(item);
            }
            return View(ListarOlOperacion.ToList());
        }

        // POST: RolOperacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdRol,IdOperacion")] RolOperacion rolOperacion, string SelectedModulo, FormCollection formCollection)
        {
            bool PrimerResgistro = false;
            //List<RolOperacion> listaEliminarRoles = db.RolOperacion.Where(m => m.IdRol == rolOperacion.Id.ToString()).ToList();
            //db.RolOperacion.RemoveRange(listaEliminarRoles);
            //db.SaveChanges();
            foreach (var item in formCollection)
            {
                if (PrimerResgistro == false)
                {
                    PrimerResgistro = true;

                }
                else
                {
                    string Check = item.ToString();
                    string Rol = "";
                    string Operacion = "";
                    bool Cambio = false;
                    for (int i = 0; i < Check.Length; i++)
                    {
                        if (Check[i] != '+' && !Cambio)
                        {
                            Rol = Rol + Check[i];
                        }
                        else
                        {
                            if (Check[i] == '+' && !Cambio)
                            {
                                Cambio = true;
                            }
                            else
                            {
                                Operacion = Operacion + Check[i];
                            }
                        }

                    }
                    var listadoOperaciones = db.RolOperacion.Where(m => m.IdRol == Rol);
                    if (!string.IsNullOrEmpty(Operacion))
                    {
                        if (listadoOperaciones.Count(m => m.IdOperacion == new Guid(Operacion)) == 0)
                        {
                            RolOperacion ChechBox = new RolOperacion();
                            ChechBox.Id = Guid.NewGuid();
                            ChechBox.IdRol = Rol;
                            ChechBox.IdOperacion = new Guid(Operacion);
                            db.RolOperacion.Add(ChechBox);
                        }
                    }

                }

            }
            db.SaveChanges();

            return RedirectToAction("Index", "AspNetRoles", new { id = rolOperacion.Id });
        }

        // GET: RolOperacions/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOperacion rolOperacion = db.RolOperacion.Find(id);
            if (rolOperacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdRol = new SelectList(db.AspNetRoles, "Id", "Name", rolOperacion.IdRol);
            ViewBag.IdOperacion = new SelectList(db.Operaciones, "Id", "Nombre", rolOperacion.IdOperacion);
            return View(rolOperacion);
        }

        // POST: RolOperacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdRol,IdOperacion")] RolOperacion rolOperacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolOperacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdRol = new SelectList(db.AspNetRoles, "Id", "Name", rolOperacion.IdRol);
            ViewBag.IdOperacion = new SelectList(db.Operaciones, "Id", "Nombre", rolOperacion.IdOperacion);
            return View(rolOperacion);
        }

        // GET: RolOperacions/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOperacion rolOperacion = db.RolOperacion.Find(id);
            if (rolOperacion == null)
            {
                return HttpNotFound();
            }
            return View(rolOperacion);
        }

        // POST: RolOperacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RolOperacion rolOperacion = db.RolOperacion.Find(id);
            db.RolOperacion.Remove(rolOperacion);
            db.SaveChanges();
            return RedirectToAction("Index");
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
