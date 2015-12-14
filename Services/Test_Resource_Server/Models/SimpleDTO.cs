using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test_Resource_Server.Models
{
    public class SimpleDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class MYIDTO
    {
        public int ID { get; set; }
        public string Name { get { return MYID.ToString(); } }
        public int MYID { get; set; }
    }

    public class TestUserDTO
    {
        public int MYID { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public bool HasAccess { get; set; }
    }

    public class AbrDTO
    {
        public int MYID { get; set; }
        public bool IsActive { get; set; }
    }
}