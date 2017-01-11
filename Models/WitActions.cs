using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitAi.Models
{
    public class WitActions : Dictionary<string, Func<ConverseRequest, ConverseResponse, WitContext>> {}
}