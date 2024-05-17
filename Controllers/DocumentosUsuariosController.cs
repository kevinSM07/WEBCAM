using WEBCAM.Context;
using WEBCAM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using SpreadsheetLight;

namespace WEBCAM.Controllers
{
    public class DocumentosUsuariosController : Controller
    {
        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: DocumentosUsuarios
        public ActionResult Index(string Usuarioid)
        {
            ViewBag.Usuarioid = Usuarioid;
            if (Usuarioid != null)
            {                
                TblDocumentosUsuarios tblDocumentosUsuarios = db.TblDocumentosUsuarios.FirstOrDefault(m => m.Id == new Guid( Usuarioid));
                if (tblDocumentosUsuarios != null )
                {
                    var usuariotemp = tblDocumentosUsuarios.Idusuario;
                    List<TblDocumentosUsuarios> ListadoUsuarios = new List<TblDocumentosUsuarios>();
                    Session["IdUsuarioDocumento"] = usuariotemp;
                    foreach (var itemRuta in db.TblDocumentosUsuarios.Where(m => m.Idusuario == usuariotemp))
                    {
                        //if (string.IsNullOrEmpty(itemRuta.Ruta))
                        //{
                        //    itemRuta.Ruta = ruta + itemRuta.AspNetUsers.NroIdentificacion + "/" + itemRuta.Nombre + ".pdf";
                        //}
                        //else
                        //{
                        //    itemRuta.Ruta = ruta + itemRuta.Ruta;
                        //}
                        ListadoUsuarios.Add(itemRuta);
                    }
                    return View(ListadoUsuarios.ToList());
                }
                else
                {
                    List<TblDocumentosUsuarios> ListadoUsuarios = new List<TblDocumentosUsuarios>();
                    Session["IdUsuarioDocumento"] = Usuarioid;
                    foreach (var itemRuta in db.TblDocumentosUsuarios.Where(m => m.Idusuario == Usuarioid))
                    {
                        //if (string.IsNullOrEmpty(itemRuta.Ruta))
                        //{
                        //    itemRuta.Ruta = ruta + itemRuta.AspNetUsers.NroIdentificacion + "/" + itemRuta.Nombre + ".pdf";
                        //}
                        //else
                        //{
                        //    itemRuta.Ruta = ruta + itemRuta.Ruta;
                        //}
                        ListadoUsuarios.Add(itemRuta);
                    }
                    return View(ListadoUsuarios.ToList());
                }
            }
            else
            {               
                var tblDocumentosUsuaios = db.TblDocumentosUsuarios.Where(m => m.Idusuario == Usuarioid.ToString());
                List<TblDocumentosUsuarios> ListadoUsuarios = new List<TblDocumentosUsuarios>();
                foreach (var item in tblDocumentosUsuaios)
                {
                    //if (string.IsNullOrEmpty(item.Ruta))
                    //{
                    //    item.Ruta = ruta + item.AspNetUsers.NroIdentificacion + "/" + item.Nombre + ".pdf";
                    //}
                    //else
                    //{
                    //    item.Ruta = ruta + item.Ruta;
                    //}
                    ListadoUsuarios.Add(item);
                }
                return View(tblDocumentosUsuaios.ToList());               
            }
        }

        // GET: DocumentosUsuarios/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblDocumentosUsuarios tblDocumentosUsuarios = db.TblDocumentosUsuarios.Find(id);
            if (tblDocumentosUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(tblDocumentosUsuarios);
        }

        // GET: DocumentosUsuarios/Create
        public ActionResult Create(Guid? Usuarioid)
        {
            Usuarioid = new Guid(Session["IdUsuarioDocumento"].ToString());
            if (Usuarioid != null)
            {
                ViewBag.IdUsuario = new SelectList(db.AspNetUsers.Where(m => m.Id == Usuarioid.ToString()), "Id", "Email");

                return View();
            }
            else
            {
                ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email");
                return View();
            }
        }

        // POST: DocumentosUsuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdUsuario,Nombre,Ruta,Estado,Observacion")] TblDocumentosUsuarios tblDocumentosUsuarios, HttpPostedFileBase flArchivo, string DtpFechaInicial, string DtpFechaFinal)
        {
            tblDocumentosUsuarios.Estado = 0;

            if (ModelState.IsValid)
            {
                tblDocumentosUsuarios.Id = Guid.NewGuid();
                tblDocumentosUsuarios.Idusuario = Session["IdUsuarioDocumento"].ToString();
                tblDocumentosUsuarios.AspNetUsers = db.AspNetUsers.FirstOrDefault(m => m.Id == tblDocumentosUsuarios.Idusuario);
                //tblDocumentosUsuarios.Ruta = tblDocumentosUsuarios.AspNetUsers.NroIdentificacion.Trim() + "/" + tblDocumentosUsuarios.Nombre + ".pdf";
                tblDocumentosUsuarios.FechaInicio = DateTime.Parse(DtpFechaInicial);
                tblDocumentosUsuarios.FechaFinal = DateTime.Parse(DtpFechaFinal);
                tblDocumentosUsuarios.Observaciones = tblDocumentosUsuarios.Observaciones;
                db.TblDocumentosUsuarios.Add(tblDocumentosUsuarios);
                db.SaveChanges();
                if (flArchivo != null)
                {
                    SubirArchivo(flArchivo, tblDocumentosUsuarios);
                }
                return RedirectToAction("Index", new { Usuarioid = tblDocumentosUsuarios.Id });
            }
            ViewBag.Idusuario = new SelectList(db.AspNetUsers, "Id", "Email", tblDocumentosUsuarios.Idusuario);
            return View(tblDocumentosUsuarios);
        }

        public void SubirArchivo(HttpPostedFileBase file, TblDocumentosUsuarios tblDocumentosUsuarios)
        {           
            //SubirArchivoModelo modelo = new SubirArchivoModelo();
            //AspNetUsers test = new AspNetUsers();
            //if (Session["IdUsuarioDocumento"] != null)
            //{
            //    string Idusuario = Session["IdUsuarioDocumento"].ToString();
            //    test = db.AspNetUsers.FirstOrDefault(m => m.Id == Idusuario);
            //}
            //else
            //{
            //    TblDocumentosUsuarios tblDocumentosUsuarios1 = db.TblDocumentosUsuarios.FirstOrDefault(m=>m.Id == tblDocumentosUsuarios.Id);
            //    test = db.AspNetUsers.FirstOrDefault(m => m.Id == tblDocumentosUsuarios1.Idusuario);
            //}
            //if (file != null)
            //{                
            //    string ruta = Server.MapPath("~/Content/DocumentosUsuarios/" + test.NroIdentificacion.Trim() + "/");
            //    Directory.CreateDirectory(ruta);
            //    string pathArchivo = Path.Combine(ruta + tblDocumentosUsuarios.Nombre + ".pdf");
            //    ruta += file.FileName;
            //    modelo.SubirArchivo(pathArchivo, file);
            //}
        }

        // GET: DocumentosUsuarios/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblDocumentosUsuarios tblDocumentosUsuarios = db.TblDocumentosUsuarios.Find(id);
            if (tblDocumentosUsuarios == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", tblDocumentosUsuarios.Idusuario);
            return View(tblDocumentosUsuarios);
        }

        // POST: DocumentosUsuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdUsuario,Nombre,Ruta,Estado,Observacion,AspNetUsers")] TblDocumentosUsuarios tblDocumentosUsuarios, HttpPostedFileBase flArchivo, string DtpFechaInicial, string DtpFechaFinal)
        {
            if (ModelState.IsValid)
            {
                TblDocumentosUsuarios tblDocumentosUsuarios1 = db.TblDocumentosUsuarios.FirstOrDefault(m => m.Id == tblDocumentosUsuarios.Id);
                tblDocumentosUsuarios1.Nombre = tblDocumentosUsuarios.Nombre;
                tblDocumentosUsuarios1.FechaInicio = DateTime.Parse(DtpFechaInicial);
                tblDocumentosUsuarios1.FechaFinal = DateTime.Parse(DtpFechaFinal);
                db.SaveChanges();
                if (flArchivo != null)
                {
                    SubirArchivo(flArchivo, tblDocumentosUsuarios1);
                }
                return RedirectToAction("Index", new { Usuarioid = tblDocumentosUsuarios1.Id });
            }
            ViewBag.Idusuario = new SelectList(db.AspNetUsers, "Id", "Email", tblDocumentosUsuarios.Idusuario);
            return View(tblDocumentosUsuarios);
        }

        // GET: DocumentosUsuarios/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblDocumentosUsuarios tblDocumentosUsuarios = db.TblDocumentosUsuarios.Find(id);
            if (tblDocumentosUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(tblDocumentosUsuarios);
        }

        // POST: DocumentosUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TblDocumentosUsuarios tblDocumentosUsuarios = db.TblDocumentosUsuarios.Find(id);
            db.TblDocumentosUsuarios.Remove(tblDocumentosUsuarios);
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