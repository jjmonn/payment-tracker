using System.Web;
using System.Web.Optimization;

namespace EcheancierDotNet
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.gritter.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/util.js"));

            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                        "~/Scripts/home.js"));

            bundles.Add(new ScriptBundle("~/bundles/suppliers").Include(
                        "~/Scripts/suppliers.js"));

            bundles.Add(new ScriptBundle("~/bundles/supplierDetails").Include(
                    "~/Scripts/supplierDetails.js"));

            bundles.Add(new ScriptBundle("~/bundles/invoices").Include(
                        "~/Scripts/invoices.js"));

            bundles.Add(new ScriptBundle("~/bundles/payments").Include(
                        "~/Scripts/payments.js"));

            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                        "~/Scripts/layout.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour le développement et l'apprentissage. Puis, une fois
            // prêt pour la production, utilisez l'outil de génération à l'adresse https://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-toggle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/jquery.gritter.css",
                      "~/Content/bootstrap-toggle.css"));

        }
    }
}
