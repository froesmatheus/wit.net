using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wit.Models;

namespace Wit.NET
{
    class MainTests
    {
        public void Main(String[] args)
        {
            Dictionary<string, Func<Context, ConverseRequest, ConverseResponse>> actions;           
            
                 
        }

        public Dictionary<string, dynamic> send(ConverseRequest request, ConverseResponse response)
        {
            return null;
        }

        public string getEndereco(string cep)
        {
            return "Rua A";
        }
    }
}
