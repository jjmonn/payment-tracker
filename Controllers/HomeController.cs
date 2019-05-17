using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcheancierDotNet.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                var p_path = Path.GetFullPath(file.FileName);

                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/"), fileName);
                file.SaveAs(path);

                InvoiceController l_invoiceController = new InvoiceController();
                string l_result = l_invoiceController.UploadInvoices(path);
                if (l_result == "")
                {
                    ViewBag.Message = "Upload successfull";
                }
                else
                {
                    ViewBag.Alert = l_result;
                }
            }
            else
            {
                ViewBag.Alert = "No file selected or wrong file";
            }
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Upload()
        {
            ViewBag.Message = "Uploads functionalities";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}