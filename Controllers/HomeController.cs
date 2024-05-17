using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WEBCAM.Context;
using WEBCAM.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WEBCAM.Controllers
{
    // Autorize, Evita que los usuario que no esten autenticados ingresen a las pantallas de la plataforma
    [Authorize]
    public class HomeController : Controller
    {
        private WEBCAMEntities db = new WEBCAMEntities();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManagers;
        // Autorize, Evita que los usuario que no esten autenticados ingresen a las pantallas de la plataforma

        public ActionResult Index()
        {
            Session["UsuariosPendientes"] = 0;
            
            Guid IdUsuario = new Guid(HttpContext.User.Identity.GetUserId());
            AspNetUsers Usuarioactual = db.AspNetUsers.FirstOrDefault(u => u.Id == IdUsuario.ToString());

            List<AspNetRoles> listadoRol = new List<AspNetRoles>();
            List<RolOperacion> ListadoOperacionesRol = new List<RolOperacion>();
            AspNetRoles Rol = new AspNetRoles();
            string roles = "";
            using (ApplicationDbContext defaultdb = new ApplicationDbContext())
            {
                var UserManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(new UserStore<ApplicationUser>(defaultdb));
                var listadoUsarioRol = UserManager.GetRoles(Usuarioactual.Id);
                foreach (var item in listadoUsarioRol)
                {
                    Rol = db.AspNetRoles.FirstOrDefault(m => m.Name == item);
                    roles = roles + "," + Rol.Name;
                    listadoRol.Add(Rol);
                }
            }
            Session["RolesUsuario"] = listadoRol;
            Session["Nombres"] = Usuarioactual.Nombre;
            Session["Apellidos"] = Usuarioactual.Apellido;
            Session["Id"] = Usuarioactual.Id;
            Session["PromedioCamposUsuario"] = PromedioCamposUsuario(Usuarioactual);

            List<TblDocumentosUsuarios> DocumentoUsuario = new List<TblDocumentosUsuarios>();
            DocumentoUsuario = db.TblDocumentosUsuarios.Where(m=>m.Estado != null).ToList();
            foreach (var item in DocumentoUsuario)
            {
                TblDocumentosUsuarios itemdocumento = new TblDocumentosUsuarios();
                itemdocumento = db.TblDocumentosUsuarios.FirstOrDefault(m => m.Id == item.Id);
                DateTime fehcaActual = DateTime.Now.Date;
                DateTime fecha = (DateTime)itemdocumento.FechaFinal;
                TimeSpan diferenciafecha = fecha - fehcaActual;
                int days = (int)diferenciafecha.TotalDays;
                if (days <= 30)
                {
                    if (days <= 10)
                    {
                        if (days <= 0)
                        {
                            itemdocumento.Estado = 3;
                        }
                        else
                        {
                            itemdocumento.Estado = 2;
                        }
                    }
                    else
                    {
                        itemdocumento.Estado = 1;
                    }
                }
                else
                {
                    itemdocumento.Estado = 0;
                }
                db.SaveChanges();
            }
            ViewBag.DocumentosUsuarios = DocumentoUsuario;

            foreach (var itemRol in listadoRol)
            {
                ListadoOperacionesRol = db.RolOperacion.Where(m => m.IdRol == itemRol.Id).ToList();
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
                ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
                Session["status"] = ListadoOperacionesRol; //HttpContext.User.Identity.Name;

                AspNetUsers usuario = db.AspNetUsers.FirstOrDefault(m => m.Id == user.Id);
                if (usuario.AspNetRoles.Count(m => m.Id == "7f452e0e-e916-4142-9e6f-1cd048ed3a4a") > 0)
                {
                    Session["RolUsuario"] = "7f452e0e-e916-4142-9e6f-1cd048ed3a4a";
                }
                else
                {
                    if (usuario.AspNetRoles.Count(m => m.Id == "7c88f22f-fea4-4091-97b4-aa1092c74476") > 0)
                    {
                        Session["RolUsuario"] = "7c88f22f-fea4-4091-97b4-aa1092c74476";
                    }
                }
                foreach (var item in usuario.AspNetRoles)
                {
                    Session["RolUsuario"] = item.Name;
                }
                Session["IdUsuario"] = usuario.Id;
                Session["UserId"] = new Guid();

                if (itemRol.Name == "Administrador")
                {
                    Session["UsuariosPendientes"] = db.AspNetUsers.Count(m => m.Status == "0");

                }
            }
            return View();
        }
        public int PromedioCamposUsuario(AspNetUsers Usuarioactual)
        {
            int Promedio = 0;
            if (Usuarioactual.IdGenero != null)
            {
                Promedio = Promedio + 1;
            }
            if (!string.IsNullOrEmpty(Usuarioactual.Nombre))
            {
                Promedio = Promedio + 1;
            }
            if (!string.IsNullOrEmpty(Usuarioactual.Apellido))
            {
                Promedio = Promedio + 1;
            }
            if (!string.IsNullOrEmpty(Usuarioactual.Celular))
            {
                Promedio = Promedio + 1;
            }
            if (!string.IsNullOrEmpty(Usuarioactual.Direccion))
            {
                Promedio = Promedio + 1;
            }
            if (!string.IsNullOrEmpty(Usuarioactual.Email))
            {
                Promedio = Promedio + 1;
            }
            if (Usuarioactual.IdTipoDocumento != null)
            {
                Promedio = Promedio + 1;
            }
            if (Usuarioactual.IdCiudad != null)
            {
                Promedio = Promedio + 1;
            }
            Session["PromedioCamposUsuario"] = Promedio;
            return Promedio = Promedio * 100 / 8;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManagers ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManagers = value;
            }
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult _SideBar()
        {
            TempData["status"] = TempData["status"];
            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize]
        public ActionResult InfoUser()
        {
            string IdUser = String.Empty;
            //var Usuarioactual = db.AspNetUsers.Where(u => u.UserName == HttpContext.User.Identity.Name);
            //foreach (var item in Usuarioactual)
            //{
            //    IdUser = item.Id;
            //}
            return RedirectToAction("Edit", "AspNetUsers", new { id = IdUser });

        }

    }
}