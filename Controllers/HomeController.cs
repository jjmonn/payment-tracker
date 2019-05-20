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
        public ActionResult Index(HttpPostedFileBase file, string uploadName)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                var p_path = Path.GetFullPath(file.FileName);

                if (uploadName == "")
                {
                    ViewBag.alert = "Please enter a name for this upload";
                    return View();
                }
                
                UploadsController l_uploadController = new UploadsController();
                if (l_uploadController.GetUploadByName(uploadName) == true)
                {
                    ViewBag.alert = "Upload name already used";
                    return View();
                }
                // store the file inside ~/App_Data/uploads folder                      DO NOT SAVE THE FILE ON SERVER PROD
                var path = Path.Combine(Server.MapPath("~/"), fileName);
                file.SaveAs(path);

                InvoiceController l_invoiceController = new InvoiceController();
                string l_result = l_invoiceController.UploadInvoices(path, uploadName);
                if (l_result != "")
                {
                    ViewBag.Alert = l_result;
                }
                ViewBag.Message = l_invoiceController.m_message;
            }
            else
            {
                ViewBag.alert = "No file selected or wrong file";
            }
            return View();
        }

        [HttpPost]
        public ActionResult UploadTable(HttpPostedFileBase file)
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
                string l_result = l_invoiceController.UploadInvoiceTable(path);
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
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Upload()
        {
            ViewBag.Message = "Upload functionalities";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}