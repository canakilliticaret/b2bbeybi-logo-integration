using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityObjects.Model.Response
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string SessionId { get; set; }
        public string Mail { get; set; }
        public string Mobile { get; set; }
        public string PlasiyerCode { get; set; }
        public string GroupCode1 { get; set; }
        public string Secret { get; set; }
        public bool ForceablePass { get; set; }
        public int UserTypeId { get; set; }
        public int Status { get; set; }
        public int ParentUserId { get; set; }
    }
}
