using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                HTTPServer server = new HTTPServer(10, 80);
                server.Start();
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
