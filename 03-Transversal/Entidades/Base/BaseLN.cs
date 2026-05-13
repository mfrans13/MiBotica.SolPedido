using log4net;
using System.Reflection;

namespace Entidades.Base
{
    public abstract class BaseLN
    {
        protected readonly ILog Log;

        public BaseLN()
        {
            Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}