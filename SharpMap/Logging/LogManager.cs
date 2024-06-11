using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMap.Logging
{
    public class LogManager : ILogManager
    {
        public static ILogManager Global { get; set; } = new LogManager();

        #region Methods

                      ILog ILogManager.GetLogger ( Type type ) => new NoOpLogger();
                      ILog ILogManager.GetLogger<T> ()         => ( ( ILogManager ) this ).GetLogger( typeof( T ) );

        public static ILog             GetLogger ( Type type ) => Global.GetLogger( type );
        public static ILog             GetLogger<T> ()         => GetLogger( typeof( T ) );

        #endregion
    }
}
