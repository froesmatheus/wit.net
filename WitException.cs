using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wit.NET
{
    class WitException : Exception
    {
        public WitException(string message) : base(message) { }
    }
}
