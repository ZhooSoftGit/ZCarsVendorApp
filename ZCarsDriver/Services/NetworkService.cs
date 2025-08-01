using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhooSoft.ServiceBase;

namespace ZCarsDriver.Services
{
    public class NetworkService : INetworkService
    {
        public bool IsConnected()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    
    }
}
