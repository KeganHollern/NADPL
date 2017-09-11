using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NADPLServer {
    class Program {
        static void Main(string[] args) {
            Console.Title = "NADPL Server Controller";
            Server mainServer = new Server();
            mainServer.init();
        }
    }
}
