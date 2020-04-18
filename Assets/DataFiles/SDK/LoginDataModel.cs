using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sdk
{
    class LoginDataModel
    {
        public string user_id;
        public string enterprise_id;
        public string password;
        
        public LoginDataModel(string user_id, string enterprise_id, string password)
        {
            this.user_id = user_id;
            this.enterprise_id = enterprise_id;
            this.password = password;
        }
    }
}

