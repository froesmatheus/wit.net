using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitAi
{

    /// <summary>
    /// Represents an exception thrown by Wit.AI
    /// </summary>
    class WitException : Exception
    {
        public WitException(string message) : base(message) { }
    }
}
