using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SharpMap.Utilities;

namespace UnitTests.Serialization
{
    public abstract class BaseSerializationTest
    {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
        protected static T SandD<T>(T input, IFormatter formatter)
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        {
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, input);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(ms);
            }
        }

#pragma warning disable SYSLIB0011 // Type or member is obsolete
        protected static IFormatter GetFormatter()
        {
            var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            if (formatter.SurrogateSelector == null)
                formatter.SurrogateSelector = new SurrogateSelector();
            formatter.SurrogateSelector.ChainSelector(SharpMap.Utilities.Surrogates.GetSurrogateSelectors());
            Utility.AddBruTileSurrogates(formatter);
            return formatter;
        }
    }
}
