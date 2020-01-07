using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ogloszenia5.Models
{
    public class AdExtends
    {
        public int Id { get; set; }
        public int IdCategory { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Url { get; set; }
        public string CName { get; set; }
        public int Position { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string ApplicationUserId { get; set; }
        public int Mark { get; set; }
        public string Comment { get; set; }
        public int IdR { get; set; }

    }
}