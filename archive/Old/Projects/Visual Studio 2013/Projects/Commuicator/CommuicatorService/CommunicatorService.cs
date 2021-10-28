using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicatorService
{
    public class CommunicatorService : ICommunicatorService
    {
        public string Do(string func)
        {
            return func + "has been done";
        }
    }
}
