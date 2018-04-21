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
            var airspace = new AirspaceMonitor(10000, 10000, 90000, 90000, 500, 20000, new TrackCalculator());

            var tos = new TransponderObjectification(TransponderReceiverFactory.CreateTransponderDataReceiver(), airspace);

            Console.ReadKey();
        }
    }
}
