using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorp.AuthorizationServer.Models
{
    public class UploadFileViewModel: EntryModel<Guid>
    {
        public long ContentSize { get; set; }
        public string ContentType { get; set; }
    }
}
