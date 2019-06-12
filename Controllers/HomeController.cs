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

        public ActionResult Download()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, string uploadName)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
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

                // extract only the filename
                var fileName = "upload.csv";  // Path.GetFileName(file.FileName);
                var p_path = Path.GetFullPath(file.FileName);

                // store the file inside ~/App_Data/uploads folder
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
                var fileName = "upload.csv";// Path.GetFileName(file.FileName);
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

        private string GetStrFromFile(HttpPostedFileBase file)
        {
            System.IO.Stream str;
            String strmContents;
            Int32 counter, strLen, strRead;

            // Create a Stream object.
            str = file.InputStream;
            // Find number of bytes in stream.
            strLen = Convert.ToInt32(str.Length);
            // Create a byte array.
            byte[] strArr = new byte[strLen];
            // Read stream into byte array.
            strRead = str.Read(strArr, 0, strLen);

            // Convert byte array to a text string.
            strmContents = "";
            for (counter = 0; counter < strLen; counter++)
            {
                strmContents = strmContents + strArr[counter].ToString();
            }

            return strmContents;
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