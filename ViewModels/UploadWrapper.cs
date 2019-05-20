using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcheancierDotNet.Models;

namespace EcheancierDotNet.ViewModels
{
    public class UploadWrapper
    {

        public int UploadID { get; set; }
        public string Name { get; set; }
        public DateTime UploadDate { get; set; }

        public UploadWrapper(Upload p_upload)
        {
            this.UploadID = p_upload.UploadID;
            this.Name = p_upload.Name;
            this.UploadDate = p_upload.UploadDate;
        }

    }
}