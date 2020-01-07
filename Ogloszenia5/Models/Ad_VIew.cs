using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ogloszenia5.Models
{
    public class Ad_VIew
    {
        public IEnumerable<AdExtends> AdExtends { get; set; }
        public IEnumerable<AdExtends> Rating { get; set; }
        public IEnumerable<AdExtends> Photo { get; set; }
    }
}