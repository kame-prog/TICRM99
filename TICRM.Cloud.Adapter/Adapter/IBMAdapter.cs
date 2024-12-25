using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.Cloud.Adapter.Adaptee;
using TICRM.Cloud.Adapter.Target;

namespace TICRM.Cloud.Adapter.Adapter
{
    public class IBMAdapter :IBMManager,IIBM
    {
        private string _apiKey, _authToken;

        public IBMAdapter(string apiKey, string authToken) : base(apiKey,authToken)
        {
        }

    }
}
