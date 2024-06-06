﻿using WEBCAM.Context;
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
    public class FotosUsuariosController : Controller
    {
        private WEBCAMEntities db = new WEBCAMEntities();

        // GET: FotosUsuaios
        public ActionResult Index(string Usuarioid)
        {
            ViewBag.Usuarioid = Usuarioid;
            string ruta = "Content/FotosUsuarios/";
            if (Usuarioid != null)
            {
                TblFotosUsuario tblFotosUsuarios = db.TblFotosUsuario.FirstOrDefault(m => m.Id == new Guid(Usuarioid));
                if (tblFotosUsuarios != null)
                {
                    var usuariotemp = tblFotosUsuarios.IdUsuario;
                    List<TblFotosUsuario> ListadoUsuarios = new List<TblFotosUsuario>();
                    Session["IdUsuarioDocumento"] = usuariotemp;
                    foreach (var itemRuta in db.TblFotosUsuario.Where(m => m.IdUsuario == usuariotemp))
                    {
                        if (string.IsNullOrEmpty(itemRuta.Ruta))
                        {
                            itemRuta.Ruta = ruta + itemRuta.AspNetUsers.NroIdentificacion + "/" + itemRuta.Nombre + ".pdf";
                        }
                        else
                        {
                            itemRuta.Ruta = ruta + itemRuta.Ruta;
                        }
                        ListadoUsuarios.Add(itemRuta);
                    }
                    return View(ListadoUsuarios.ToList());
                }
                else
                {
                    List<TblFotosUsuario> ListadoUsuarios = new List<TblFotosUsuario>();
                    Session["IdUsuarioDocumento"] = Usuarioid;
                    foreach (var itemRuta in db.TblFotosUsuario.Where(m => m.IdUsuario == Usuarioid))
                    {
                        if (string.IsNullOrEmpty(itemRuta.Ruta))
                        {
                            itemRuta.Ruta = ruta + itemRuta.AspNetUsers.NroIdentificacion + "/" + itemRuta.Nombre + ".pdf";
                        }
                        else
                        {
                            itemRuta.Ruta = ruta + itemRuta.Ruta;
                        }
                        ListadoUsuarios.Add(itemRuta);
                    }
                    return View(ListadoUsuarios.ToList());
                }
            }
            else
            {
                var tblFotosUsuaios = db.TblFotosUsuario.Where(m => m.IdUsuario == Usuarioid.ToString());
                List<TblFotosUsuario> ListadoUsuarios = new List<TblFotosUsuario>();
                foreach (var item in tblFotosUsuaios)
                {
                    if (string.IsNullOrEmpty(item.Ruta))
                    {
                        item.Ruta = ruta + item.AspNetUsers.NroIdentificacion + "/" + item.Nombre + ".pdf";
                    }
                    else
                    {
                        item.Ruta = ruta + item.Ruta;
                    }
                    ListadoUsuarios.Add(item);
                }
                return View(tblFotosUsuaios.ToList());
            }
        }

        // GET: FotosUsuaios/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblFotosUsuario tblFotosUsuaios = db.TblFotosUsuario.Find(id);
            if (tblFotosUsuaios == null)
            {
                return HttpNotFound();
            }
            return View(tblFotosUsuaios);
        }

        // GET: FotosUsuaios/Create
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

        // POST: FotosUsuaios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdUsuario,Nombre,Ruta,Estado")] TblFotosUsuario tblFotosUsuaios, HttpPostedFileBase flArchivo)
        {
            tblFotosUsuaios.Estado = 0;

            if (ModelState.IsValid)
            {
                tblFotosUsuaios.Id = Guid.NewGuid();
                tblFotosUsuaios.IdUsuario = Session["IdUsuarioDocumento"].ToString();
                tblFotosUsuaios.AspNetUsers = db.AspNetUsers.FirstOrDefault(m => m.Id == tblFotosUsuaios.IdUsuario);
                tblFotosUsuaios.Ruta = tblFotosUsuaios.AspNetUsers.NroIdentificacion.Trim() + "/" + tblFotosUsuaios.Nombre + ".pdf";
                db.TblFotosUsuario.Add(tblFotosUsuaios);
                db.SaveChanges();
                if (flArchivo != null)
                {
                    SubirArchivo(flArchivo, tblFotosUsuaios);
                }
                return RedirectToAction("Index", new { Usuarioid = tblFotosUsuaios.Id });
            }
            ViewBag.Idusuario = new SelectList(db.AspNetUsers, "Id", "Email", tblFotosUsuaios.IdUsuario);
            return View(tblFotosUsuaios);
        }

        public void SubirArchivo(HttpPostedFileBase file, TblFotosUsuario tblFotosUsuaios)
        {
            SubirArchivoModelo modelo = new SubirArchivoModelo();
            AspNetUsers test = new AspNetUsers();
            if (Session["IdUsuarioDocumento"] != null)
            {
                string Idusuario = Session["IdUsuarioDocumento"].ToString();
                test = db.AspNetUsers.FirstOrDefault(m => m.Id == Idusuario);
            }
            else
            {
                TblFotosUsuario tblFotosUsuaios1 = db.TblFotosUsuario.FirstOrDefault(m => m.Id == tblFotosUsuaios.Id);
                test = db.AspNetUsers.FirstOrDefault(m => m.Id == tblFotosUsuaios1.IdUsuario);
            }
            if (file != null)
            {
                string ruta = Server.MapPath("~/Content/FotosUsuarios/" + test.NroIdentificacion.Trim() + "/");
                Directory.CreateDirectory(ruta);
                string pathArchivo = Path.Combine(ruta + tblFotosUsuaios.Nombre + ".png");
                ruta += file.FileName;
                modelo.SubirArchivo(pathArchivo, file);
            }
        }

        // GET: FotosUsuaios/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblFotosUsuario tblFotosUsuaios = db.TblFotosUsuario.Find(id);
            if (tblFotosUsuaios == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", tblFotosUsuaios.IdUsuario);
            return View(tblFotosUsuaios);
        }

        // POST: FotosUsuaios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdUsuario,Nombre,Ruta,Estado,AspNetUsers")] TblFotosUsuario tblFotosUsuaios, HttpPostedFileBase flArchivo)
        {
            if (ModelState.IsValid)
            {
                TblFotosUsuario tblFotosUsuaios1 = db.TblFotosUsuario.FirstOrDefault(m => m.Id == tblFotosUsuaios.Id);
                tblFotosUsuaios1.Nombre = tblFotosUsuaios.Nombre;
                db.SaveChanges();
                if (flArchivo != null)
                {
                    SubirArchivo(flArchivo, tblFotosUsuaios1);
                }
                return RedirectToAction("Index", new { Usuarioid = tblFotosUsuaios1.Id });
            }
            ViewBag.Idusuario = new SelectList(db.AspNetUsers, "Id", "Email", tblFotosUsuaios.IdUsuario);
            return View(tblFotosUsuaios);
        }

        // GET: FotosUsuaios/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblFotosUsuario tblFotosUsuaios = db.TblFotosUsuario.Find(id);
            if (tblFotosUsuaios == null)
            {
                return HttpNotFound();
            }
            return View(tblFotosUsuaios);
        }

        // POST: FotosUsuaios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TblFotosUsuario tblFotosUsuaios = db.TblFotosUsuario.Find(id);
            db.TblFotosUsuario.Remove(tblFotosUsuaios);
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