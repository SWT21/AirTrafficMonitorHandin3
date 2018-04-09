using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Classes;
using TransponderReceiver;

namespace AirTrafficMonitor.Test.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var tos = new TransponderObjectification(TransponderReceiverFactory.CreateTransponderDataReceiver());
            while (true) { }
        }
    }
}
