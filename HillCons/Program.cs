using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HillLib;
using ClassLibrary1;

namespace HillCons
{
    class Program
    {
        static void Main(string[] args)
        {
            HillCip hll = new HillCip();
            hll.Cipher("teknoloji");
            hll.Decipher("pğnzuüdbe");
            Console.ReadKey();

            RsaCip rsa = new RsaCip();
            rsa.Cipher("teknoloji");
            rsa.Decipher("346050333236291133291387147");
            Console.ReadKey();


        }
    }
}
