using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using WEBCAM.Models;
using System.Net.Mail;
using System.Web.Mvc;
using System.Net;

namespace WEBCAM
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Conecte el servicio de correo electrónico aquí para enviar un correo electrónico.
            SmtpClient client = new SmtpClient("alzatiyo10@gmail.com", 587);
            client.UseDefaultCredentials = false;

            client.Credentials = new System.Net.NetworkCredential("alzatiyo10@gmail.com", "(P0l4r*C3n1t)");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            client.EnableSsl = true;

            client.Timeout = 30000;
            //SmtpClient client = new SmtpClient();
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("alzatiyo10@gmail.com");
            mailMessage.Sender = new MailAddress("alzatiyo10@gmail.com");
            mailMessage.To.Add(message.Destination);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = message.Subject;
            mailMessage.Body = message.Body;
            try
            {

                client.Send(mailMessage);
            }

            catch (SmtpException)
            {

            }

            //string para = message.Destination;

            //string asunto = message.Subject;
            //string mensaje = message.Body;
            //try
            //{
            //    MailMessage correo = new MailMessage();
            //    correo.From = new MailAddress("juan.alzate@tecnowaresolutions.com");
            //    correo.To.Add(para);
            //    correo.Subject = asunto;
            //    correo.Body = mensaje;
            //    correo.IsBodyHtml = true;
            //    correo.Priority = MailPriority.Normal;

            //    //String ruta = Server.MapPath("../temporal");
            //    //fichero.SaveAs(ruta + "\\" + fichero.FileName);

            //    //Attachment adjunto = new Attachment(ruta+"\\" + fichero.FileName);
            //    //correo.Attachments.Add(adjunto);

            //    SmtpClient smtp = new SmtpClient();
            //    smtp.Host = "tecnowaresolutions.com";
            //    smtp.Timeout = 50;
            //    smtp.Port = 25;
            //    smtp.EnableSsl = false;
            //    smtp.UseDefaultCredentials = false;
            //    string CuentaCorreo = "juan.alzate@tecnowaresolutions.com";
            //    string PasswordCorreo = "B5ff4l0%";
            //    smtp.Credentials = new NetworkCredential(CuentaCorreo, PasswordCorreo);

            //    smtp.Send(correo);


            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            return Task.FromResult(0);
        }
    }

    
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Conecte el servicio SMS aquí para enviar un mensaje de texto.
            return Task.FromResult(0);
        }
    }

    // Configure el administrador de usuarios de aplicación que se usa en esta aplicación. UserManager se define en ASP.NET Identity y se usa en la aplicación.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure la lógica de validación de nombres de usuario
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure la lógica de validación de contraseñas
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configurar valores predeterminados para bloqueo de usuario
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Registre los proveedores de autenticación de dos factores. Esta aplicación usa el teléfono y el correo electrónico para recibir un código de verificación del usuario
            // Puede escribir su propio proveedor y conectarlo aquí.
            manager.RegisterTwoFactorProvider("Código telefónico", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Su código de seguridad es {0}"
            });
            manager.RegisterTwoFactorProvider("Código de correo electrónico", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Código de seguridad",
                BodyFormat = "Su código de seguridad es {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure el administrador de inicios de sesión que se usa en esta aplicación.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
