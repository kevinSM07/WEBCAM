using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WEBCAM.Context;
using WEBCAM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AForge.Video.DirectShow;

namespace WEBCAM.Controllers
{
    [Authorize]
    public class AspNetUsersController : Controller
    {
        //private bool HayDispositivos;
        //private FilterInfoCollection MisDispositivos;
        //private VideoCaptureDevice MiWebcam;
        private WEBCAMEntities db = new WEBCAMEntities();
        private ApplicationDbContext defaultdb = new ApplicationDbContext();

        // GET: AspNetUsers
        public ActionResult Index(AspNetUsers model, string txtUsuario, string BtnAsignar, string BtnRemover, string SelectedRol, string IdUsuarioPendientes, HttpPostedFileBase flUsuarios)
        {
            if (IdUsuarioPendientes != null)
            {
                List<AspNetUsers> listadoPendiente = new List<AspNetUsers>();
                AspNetUsers Pendientes = new AspNetUsers();
                foreach (var item in db.AspNetUsers.OrderBy(m => m.NroIdentificacion))
                {
                    if (IdUsuarioPendientes != null)
                    {
                        if (item.Status == "0         " || item.Status == "0" || item.Status == null)
                        {
                            Pendientes = db.AspNetUsers.FirstOrDefault(m => m.Id == item.Id);
                            Pendientes.Nombre = Pendientes.NroIdentificacion + " / " + Pendientes.Nombre;
                            listadoPendiente.Add(Pendientes);
                        }
                    }
                }

                foreach (var item in listadoPendiente)
                {
                    //foreach (var itemRuta in item.TblDocumentosUsuarios.Where(m => m.Idusuario == item.Id))
                    //{
                    //    if (string.IsNullOrEmpty(itemRuta.Ruta))
                    //    {
                    //        itemRuta.Ruta = "Content/DocumentosUsuarios/" + item.NroIdentificacion + "/" + itemRuta.Nombre + ".pdf";
                    //    }
                    //    else
                    //    {
                    //        itemRuta.Ruta = "Content/DocumentosUsuarios/" + itemRuta.Ruta;
                    //    }
                    //}
                }
                ViewBag.Rol = new SelectList(db.AspNetRoles, "Name", "Name");

                return View(listadoPendiente);
            }
            else
            {
                var aspNetUsers = db.AspNetUsers.Where(m => m.Id != null);
                ViewBag.Rol = new SelectList(db.AspNetRoles, "Name", "Name");
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(defaultdb));

                foreach (var item in db.AspNetUsers)
                {
                    //foreach (var itemRuta in item.TblDocumentosUsuarios.Where(m => m.Idusuario == item.Id))
                    //{
                    //    if (string.IsNullOrEmpty(itemRuta.Ruta))
                    //    {
                    //        itemRuta.Ruta = "Content/DocumentosUsuarios/" + item.NroIdentificacion + "/" + itemRuta.Nombre + ".pdf";
                    //    }
                    //    else
                    //    {
                    //        itemRuta.Ruta = "Content/DocumentosUsuarios/" + itemRuta.Ruta;
                    //    }
                    //}
                }

                if (!string.IsNullOrEmpty(BtnAsignar))
                {
                    AsignarRol(txtUsuario, SelectedRol);
                }
                if (!string.IsNullOrEmpty(BtnRemover))
                {
                    RemoverRol(txtUsuario, SelectedRol);
                }
                return View(aspNetUsers.ToList());
            }            
        }
        public void SubirArchivo(HttpPostedFileBase file)
        {
            SubirArchivoModelo modelo = new SubirArchivoModelo();
            if (file != null)
            {
                string ruta = Server.MapPath("~/Content/Plantillas/");
                string pathArchivo = Path.Combine(ruta + Session["Id"] + "Usuarios.xls");
                ruta += file.FileName;
                modelo.SubirArchivo(pathArchivo, file);
            }
            //return RedirectToAction("Index", new { Vehiculoid = Vehiculo.Id });
        }

        //public void CargaDispositivos()
        //{
        //    MisDispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);

        //    if (MisDispositivos.Count > 0)
        //    {
        //        HayDispositivos = true;
        //        for (int i = 0; i < MisDispositivos.Count; i++)
        //        {
                    
        //        }
        //    }
        //    else
        //    {
        //        HayDispositivos = false;
        //    }

        //}

        public void SubirArchivo2(HttpPostedFileBase file)
        {
            SubirArchivoModelo modelo = new SubirArchivoModelo();
            if (file != null)
            {
                string ruta = Server.MapPath("~/Content/Plantillas/");
                string pathArchivo = Path.Combine(ruta + Session["Id"] + "Usuarios.xls");
                ruta += file.FileName;
                modelo.SubirArchivo(pathArchivo, file);
            }
            //return RedirectToAction("Index", new { Vehiculoid = Vehiculo.Id });
        }
        public ActionResult Confirmar(string IdUsuario)
        {
            AspNetUsers UsuarioActual = new AspNetUsers();
            UsuarioActual = db.AspNetUsers.FirstOrDefault(m => m.Id == IdUsuario);
            UsuarioActual.EmailConfirmed = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult VerRoles(string id)
        {

            List<AspNetRoles> listadoRol = new List<AspNetRoles>();
            List<RolOperacion> ListadoOperacionesRol = new List<RolOperacion>();
            AspNetRoles Rol = new AspNetRoles();
            string roles = "";
            using (ApplicationDbContext defaultdb = new ApplicationDbContext())
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(defaultdb));
                var listadoUsarioRol = UserManager.GetRoles(id);
                foreach (var item in listadoUsarioRol)
                {
                    Rol = db.AspNetRoles.FirstOrDefault(m => m.Name == item);
                    roles = roles + "," + Rol.Name;
                    listadoRol.Add(Rol);
                }

            }
            Request.Flash("success", "Los roles del usuario son:" + roles);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AsignarRol(string IdUsuario, string Rol)
        {
            try
            {
                using (ApplicationDbContext defaultdb = new ApplicationDbContext())
                {
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(defaultdb));
                    var resultado = UserManager.AddToRole(IdUsuario, Rol);
                    AspNetUsers usuarioactual = new AspNetUsers();
                    usuarioactual = db.AspNetUsers.FirstOrDefault(m => m.Id == IdUsuario);
                    usuarioactual.Status = "1";
                    db.SaveChanges();
                    Request.Flash("success", "Rol Asignado Exitosamente");
                }
            }
            catch (Exception e)
            {

                Request.Flash("danger", "Hubo un error al ASignado la Rol del usuario.");
                Request.Flash("danger", message: e.Message);
                return View("Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult RemoverRol(string IdUsuario, string Rol)
        {
            try
            {
                using (ApplicationDbContext defaultdb = new ApplicationDbContext())
                {
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(defaultdb));
                    var resultado = UserManager.RemoveFromRole(IdUsuario, Rol);
                    Request.Flash("success", "Rol Removido Exitosamente");
                }
            }
            catch (Exception e)
            {

                Request.Flash("danger", "Hubo un error al Remover la Rol del usuario.");
                Request.Flash("danger", message: e.Message);
                return View("Index");
            }
            return View();
        }

        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Create
        public ActionResult Create(string Rol)
        {
            if (!string.IsNullOrEmpty(Rol))
            {
                Session["Rol"] = Rol;
            }
            ViewBag.IdCiudad = new SelectList(db.TblCiudades, "Id", "Nombre");
            ViewBag.IdGenero = new SelectList(db.TblGeneros, "Id", "Nombre");
            ViewBag.IdTipoDocumento = new SelectList(db.TblTiposDocumentos, "Id", "Nombre");
            return View();
        }

        // POST: AspNetUsers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,PasswordHash,SecurityStamp,PhoneNumber,LockoutEndDateUtc,Status,IdGenero,IdCiudad,IdTipoDocumento,NroIdentificacion,Nombre,Apellido,Celular,Direccion,RazonSocial")] AspNetUsers aspNetUsers)
        {
            try
            {
                aspNetUsers.Id = Guid.NewGuid().ToString();
                aspNetUsers.EmailConfirmed = true;
                aspNetUsers.PhoneNumberConfirmed = false;
                aspNetUsers.TwoFactorEnabled = false;
                aspNetUsers.LockoutEnabled = true;
                aspNetUsers.AccessFailedCount = 0;
                aspNetUsers.UserName = aspNetUsers.Email;
                if (ModelState.IsValid)
                {
                    db.AspNetUsers.Add(aspNetUsers);
                    db.SaveChanges();
                    if (Session["Rol"] != null)
                        AsignarRol(aspNetUsers.Id, Session["Rol"].ToString());
                    Request.Flash("success", "El registro se creo exitosamente");
                    return RedirectToAction("Index");
                }

                ViewBag.IdCiudad = new SelectList(db.TblCiudades, "Id", "Nombre", aspNetUsers.IdCiudad);
                ViewBag.IdGenero = new SelectList(db.TblGeneros, "Id", "Nombre", aspNetUsers.IdGenero);
                ViewBag.IdTipoDocumento = new SelectList(db.TblTiposDocumentos, "Id", "Nombre", aspNetUsers.IdTipoDocumento);
                Request.Flash("warning", "Se presento un inconveniente, sirvase revisar la información ingresada");
                return View();
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Hubo un error al crear el registro de Usuario.");
                Request.Flash("danger", message: e.Message);
                return View("Index");
            }

        }

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCiudad = new SelectList(db.TblCiudades, "Id", "Nombre", aspNetUsers.IdCiudad);
            ViewBag.IdGenero = new SelectList(db.TblGeneros, "Id", "Nombre", aspNetUsers.IdGenero);
            ViewBag.IdTipoDocumento = new SelectList(db.TblTiposDocumentos, "Id", "Nombre", aspNetUsers.IdTipoDocumento);
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Status,IdGenero,IdCiudad,IdTipoDocumento,NroIdentificacion,Nombre,Apellido,Celular,Direccion,RazonSocial")] AspNetUsers aspNetUsers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aspNetUsers).State = EntityState.Modified;
                    db.SaveChanges();
                    Request.Flash("success", "El registro se edito exitosamente");
                    return RedirectToAction("Index", "aspNetUsers", null);
                }
                ViewBag.IdCiudad = new SelectList(db.TblCiudades, "Id", "Nombre", aspNetUsers.IdCiudad);
                ViewBag.IdGenero = new SelectList(db.TblGeneros, "Id", "Nombre", aspNetUsers.IdGenero);
                ViewBag.IdTipoDocumento = new SelectList(db.TblTiposDocumentos, "Id", "Nombre", aspNetUsers.IdTipoDocumento);
                Request.Flash("warning", "Se presento un inconveniente, sirvase revisar la información ingresada");
                return View(aspNetUsers);
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Hubo un error al Editar el registro de Usuario.");
                Request.Flash("danger", message: e.Message);
                return View(aspNetUsers);
            }

        }

        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string IdUsuario)
        {
            using (ApplicationDbContext defaultdb = new ApplicationDbContext())
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(defaultdb));
                var resultado = UserManager.GetRoles(IdUsuario);

                return View(resultado);
            }
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
                db.AspNetUsers.Remove(aspNetUsers);
                db.SaveChanges();
                Request.Flash("danger", "Hubo un error al Eliminar el registro de Usuario.");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Request.Flash("danger", "Hubo un error al Eliminar el registro de Usuario.");
                Request.Flash("danger", message: e.Message);
                return View("Index");
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
