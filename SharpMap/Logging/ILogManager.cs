using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMap.Logging
{
    public interface ILogManager
    {
        ILog GetLogger ( Type type );
        ILog GetLogger<T> ();
    }
}
