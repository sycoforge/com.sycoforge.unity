using System;
using System.Collections.Generic;
using System.Text;

namespace ch.sycoforge.Util.Collections
{
    public abstract class SynchronizedCollection
    {
        internal readonly object _synclock = new object();
    }
}
