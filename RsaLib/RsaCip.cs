using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class RsaCip
    {
        private int p, q, n, f, e = 7, d = 1;
        private string input, output = string.Empty;

        public RsaCip(int num1 = 17, int num2 = 23)
        {
            p = num1; q = num2;
            n = p * q;
            f = (p - 1) * (q - 1);

            for (; (e * d) % f != 1; d++) {; }

        }

        public void Cipher(string inp = null)
        {
            List<int> ascvalues = new List<int>(); string temp = string.Empty;

            if (inp != null) input = inp.Replace(" ", "");

            foreach (char item in input) { ascvalues.Add(item); }

            foreach (var item in ascvalues)
            {
                int v = ExpModulus(item, e, n);

                if (v.ToString().Length == 1) { temp += "00" + v.ToString(); }

                else if (v.ToString().Length == 2) { temp += "0" + v.ToString(); }

                else temp += v.ToString();

                output += temp;
                temp = string.Empty;
            }

            Console.WriteLine(output);
            ascvalues.Clear();
            output = string.Empty;
        }

        public void Decipher(string inp = null)
        {
            List<int> ascvalues = new List<int>(); string temp = string.Empty;

            if (inp != null) input = inp;

            for (int i = 0; i < input.Length; i += 3)
            {
                temp = input.Substring(i, 3);
                ascvalues.Add(Convert.ToInt32(temp));
            }

            foreach (var item in ascvalues)
            {
                output += (char)ExpModulus(item, d, n);
            }

            Console.WriteLine(output);

        }


        public int ExpModulus(int num, int exp, int c)
        {
            int result = 1;
            List<int> minis = new List<int>();
            string binary = Convert.ToString(exp, 2); //ussun binary sayiya cevrilmesi

            minis.Add(num % c);

            for (int i = 2, j = 0; i < exp; i *= 2, j++) //kendiden onceki karenin modunun karesi ile sonraki modulusu hesaplayıp kaydeder.
            {
                minis.Add((int)Math.Pow(minis[j], 2) % c);
            }

            char[] array = binary.ToCharArray();
            Array.Reverse(array);
            binary = new String(array); //binary tersleme

            int counter = 0;
            foreach (var item in binary)
            {
                if (item == '1')
                {
                    result = (result % c) * minis[counter];
                }
                counter++;
            }
            minis.Clear();
            return (result % c);

        }

    }
}
