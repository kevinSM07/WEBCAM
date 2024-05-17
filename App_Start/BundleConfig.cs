//using System.Web;
//using System.Web.Optimization;

//namespace WEBCAM
//{
//    public class BundleConfig
//    {
//        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
//        public static void RegisterBundles(BundleCollection bundles)
//        {
//            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
//                        "~/Scripts/jquery-{version}.js"));

//            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
//                        "~/Scripts/jquery.validate*"));

//            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios.  De esta manera estará
//            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
//            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
//                        "~/Scripts/modernizr-*"));

//            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
//                      "~/Scripts/bootstrap.js"));

//            bundles.Add(new StyleBundle("~/Content/css").Include(
//                      "~/Content/bootstrap.css",
//                      "~/Content/site.css"));
//        }
//    }
//}
using System.Web;
using System.Web.Optimization;

namespace WEBCAM
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/sb-admin-2.min.css",
                      "~/Content/vendor/datatables/dataTables.bootstrap4.min.css",
                      "~/Content/plugins/fontawesome-free/css/all.min.css",
                      "~/Content/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
                      "~/Content/css/adminlte.min.css",
                      "~/Content/vendor/fontawesome-free/css/all.min.css",
                      "~/Content/plugins/fontawesome-free/css/all.min.css",
                      "~/Content/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css",
                      "~/Content/plugins/datatables-responsive/css/responsive.bootstrap4.min.css",
                      "~/Content/plugins/datatables-buttons/css/buttons.bootstrap4.min.css",
                      "~/Content/css/adminlte.min.css"));




            //          "~/Content/bootstrap.css"));
            ////          "~/Content/site.css"
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/css/sb-admin-2.min.css",
                      "~/Content/vendor/datatables/dataTables.bootstrap4.min.css",
                      "~/Content/plugins/datatables-responsive/css/responsive.bootstrap4.min.css",
                      "~/Content/plugins/datatables-buttons/css/buttons.bootstrap4.min.css",
                      "~/Content/plugins/fontawesome-free/css/all.min.css",
                      "~/Content/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
                      "~/Content/css/adminlte.min.css",
                      "~/Content/vendor/fontawesome-free/css/all.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Content/plugins/jquery/jquery.min.js",
                      "~/Content/js/adminlte.min.js",
                      "~/Content/js/demo.js",
                      "~/Content/vendor/bootstrap/js/bootstrap.bundle.min.js",
                      "~/Content/vendor/jquery-easing/jquery.easing.min.js",
                      "~/Content/vendor/chart.js/Chart.min.js",
                      "~/Content/vendor/datatables/jquery.dataTables.min.js",
                      "~/Content/vendor/datatables/dataTables.bootstrap4.min.js",
                      "~/Content/js/demo/datatables-demo.js",
                      "~/Content/js/sb-admin-2.min.js",
                      "~/Content/js/demo/chart-area-demo.js",
                      "~/Content/plugins/jquery/jquery.min.js",
                      "~/Content/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
                      "~/Content/plugins/jquery-mousewheel/jquery.mousewheel.js",
                      "~/Content/plugins/raphael/raphael.min.js",
                      "~/Content/plugins/jquery-mapael/jquery.mapael.min.js",
                      "~/Content/plugins/jquery-mapael/maps/usa_states.min.js",
                      "~/Content/plugins/chart.js/Chart.min.js",
                      "~/Content/js/demo.js",
                      "~/Content/js/pages/dashboard2.js",
                      "~/Content/js/demo/chart-pie-demo.js",
                      "~/Content/plugins/datatables/jquery.dataTables.min.js",
                      "~/Content/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js",
                      "~/Content/plugins/datatables-responsive/js/dataTables.responsive.min.js",
                      "~/Content/plugins/datatables-responsive/js/responsive.bootstrap4.min.js",
                      "~/Content/plugins/datatables-buttons/js/dataTables.buttons.min.js",
                      "~/Content/plugins/datatables-buttons/js/buttons.bootstrap4.min.js",
                      "~/Content/plugins/jszip/jszip.min.js",
                      "~/Content/plugins/pdfmake/pdfmake.min.js",
                      "~/Content/plugins/pdfmake/vfs_fonts.js",
                      "~/Content/plugins/datatables-buttons/js/buttons.html5.min.js",
                      "~/Content/plugins/datatables-buttons/js/buttons.print.min.js",
                      "~/Content/plugins/datatables-buttons/js/buttons.colVis.min.js",
                      "~/Content/plugins/bootstrap/js/bootstrap.bundle.min.js",
                      "~/Content/js/adminlte.min.js"));













        }
    }
}
