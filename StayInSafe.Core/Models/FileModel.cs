using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Models
{
    public class FileModel
    {
        public IFormFile ImageFile { get; set; }
        public string FileName => ImageFile.FileName;
    }
}
