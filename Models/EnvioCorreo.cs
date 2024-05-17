using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace WEBCAM.Models
{
    public class EnvioCorreo
    {
        public void EnviarCorreo(string Para, string Asunto, string Mensaje, string Attachments)
        {
            try
            {
                MailMessage Correo = new MailMessage();
                Correo.From = new MailAddress("juan.alzate@tecnowaresolutions.com");
                Correo.To.Add(Para);
                Correo.Subject = Asunto;
                Correo.Body = Mensaje;
                Attachment data = new Attachment(Attachments, MediaTypeNames.Application.Octet);
                Correo.Attachments.Add(data);
                Correo.IsBodyHtml = true;
                Correo.Priority = MailPriority.Normal;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "tecnowaresolutions.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                string sCuentaCorreo = Correo.From.ToString();
                string sPasswordCorreo = "(P0l4r*C3n1t)";
                smtp.Credentials = new System.Net.NetworkCredential(sCuentaCorreo, sPasswordCorreo);
                Correo.BodyEncoding = UTF8Encoding.UTF8;
                Correo.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                smtp.Send(Correo);


                

            }
            catch (global::System.Exception)
            {

                throw;
            }
        }
    }
}