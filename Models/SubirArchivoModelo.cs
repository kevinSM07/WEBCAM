using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace WEBCAM.Models
{
    public class SubirArchivoModelo
    {
        public string Confirmacion { get; set; }
        public Exception error { get; set; }
        public void SubirArchivo(string ruta, HttpPostedFileBase file)
        {
            try
            {
                file.SaveAs(ruta);
                this.Confirmacion = "Fichero Guardado";
            }
            catch (Exception e)
            {
                this.error = e;
            }
        }

        public string BuscarExtencion(string FileName)
        {
            string nuevo = "";
            bool EncuentroPunto = false;
            for (int i = 0; i < FileName.Length; i++)
            {
                if (FileName[i].ToString() == "." || EncuentroPunto == true)
                {
                    if (FileName[i].ToString() == ".")
                    {
                        nuevo = "";
                    }
                    nuevo = nuevo + FileName[i];
                    EncuentroPunto = true;
                }
            }
            
            return nuevo;
        }

        internal void SubirArchivo(string v, ImageFormat png)
        {
            throw new NotImplementedException();
        }
    }
}