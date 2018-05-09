using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckype.Interfaces
{
    public interface IMessage
    {
        MessageType MessageType { get; set; }
        
        object Content { get; set; }
    }
}
