using System;
using System.IO;

namespace EventCorps.Helper.Models
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
