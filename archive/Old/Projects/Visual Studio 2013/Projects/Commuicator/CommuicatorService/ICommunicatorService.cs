using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace CommunicatorService
{
    [ServiceContract]
    public interface ICommunicatorService
    {
        [OperationContract]
        string Do(string func);
    }
}
