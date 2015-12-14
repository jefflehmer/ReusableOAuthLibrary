using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test_Resource_Server.Models
{
    public class ItemDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
    }
}