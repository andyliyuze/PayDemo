using System;
using System.Threading.Tasks;

namespace paypal
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var demo = new demo();
            await demo.AuthorizeOrder("3JA136432S562431C");
            Console.WriteLine("Hello World!");
        }
    }
}
