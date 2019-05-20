using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcheancierDotNet.Models
{
    public class Upload
    {
        public int UploadID { get; set; }
        public string Name { get; set; }
        public DateTime UploadDate { get; set; }


        public virtual ICollection<Invoice> Invoices { get; set; }

        public Upload()
        {
        }

        public Upload(string p_name)
        {
            this.Name = p_name;
            this.UploadDate = DateTime.Now;
        }


    }
}