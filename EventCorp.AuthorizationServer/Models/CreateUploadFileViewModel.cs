using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorp.AuthorizationServer.Models
{
    public class CreateUploadFileViewModel
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public FileInfo TempFileInfo { get; set; }
      public bool Global { get; set; }
      public Guid Owner { get; set; }
    }
}
