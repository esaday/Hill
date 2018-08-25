using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillLib
{
    public class HillCip
    {
        private Dictionary<char, int> _alfabe = new Dictionary<char, int>()
        {
            {'a', 1 },{'b', 2 },{'c', 3 },{'ç', 4 },{'d', 5 },{'e', 6 },{'f', 7 },{'g', 8 },{'ğ', 9 },{'h', 10 },
            {'ı', 11 },{'i', 12 },{'j', 13 },{'k', 14 },{'l', 15 },{'m', 16 },{'n', 17 },{'o', 18 },{'ö', 19 },{'p', 20 },
            {'r', 21 },{'s', 22 },{'ş', 23 },{'t', 24 },{'u', 25 },{'ü', 26 },{'v', 27 },{'y', 28 },{'z', 29 }
        };
        int _matrixDim = 3;
        int[,] keyMatrix, inverseMatrix;
        string _input;
        public int MatrixDim { get => _matrixDim; set => _matrixDim = value; }
        protected Dictionary<char, int> Alfabe { get => _alfabe; set => _alfabe = value; }

        public HillCip()
        {
            //benim keyMatrix = new int[3, 3] { { 12, 4, 3 }, { 22, 1, 13 }, { 11, 15, 5 } };
            keyMatrix = new int[3, 3] { { 5, 7, 11 }, { 8, 10, 12 }, { 6, 3, 29 } };
            inverseMatrix = new int[3, 3];
            int det = HillMath.Determ(keyMatrix);
            inverseMatrix = HillMath.GetInverse(HillMath.ModularDet(det), HillMath.TransposenMod(HillMath.Cofactor(HillMath.Minor(keyMatrix))));

        }

        public void Cipher(string input = null)
        {
            string cipheredText = string.Empty;
            if (input != null) _input = input;
            while (_input.Length % MatrixDim != 0) _input += "a";
            int[] incMatrix = new int[MatrixDim];

            for (int i = 0; i < _input.Length; i += incMatrix.Length)
            {
                for (int j = 0; j < incMatrix.Length; j++)
                {
                    incMatrix[j] = Alfabe[_input[i + j]];
                }
                incMatrix = HillMath.MultiplyWithMod(keyMatrix, incMatrix);
                cipheredText += NumtoText(incMatrix);
            }
            Console.WriteLine(cipheredText);
        }

        public void Decipher(string input = null)
        {
            string decipheredText = string.Empty;
            if (input != null) _input = input;

            int[] incMatrix = new int[MatrixDim];

            for (int i = 0; i < _input.Length; i += incMatrix.Length)
            {
                for (int j = 0; j < incMatrix.Length; j++)
                {
                    incMatrix[j] = Alfabe[_input[i + j]];
                }
                incMatrix = HillMath.MultiplyWithMod(inverseMatrix, incMatrix);
                decipheredText += NumtoText(incMatrix);
            }
            Console.WriteLine(decipheredText);
        }

        string NumtoText(int[] part)
        {
            string output = string.Empty;
            foreach (var item in part)
            {
                output += Alfabe.SingleOrDefault(x => x.Value == item).Key;
            }
            return output;
        }
    }

    public static class HillMath
    {
        public static int[] MultiplyWithMod(int[,] keyMatrix, int[] incPart)
        {
            int[] mult = new int[incPart.Length];

            for (int i = 0; i < incPart.Length; i++)
            {
                for (int j = 0; j < incPart.Length; j++)
                {
                    mult[i] += keyMatrix[j, i] * incPart[j];
                }
                mult[i] %= 29;
                if (mult[i] % 29 <= 0) mult[i] += 29;
            }
            return mult;
        }

        public static int Determ(int[,] keyM)
        {
            int d = 0;
            for (int i = 0; i < 3; i++)
                d = d + (keyM[0, i] * (keyM[1, (i + 1) % 3] * keyM[2, (i + 2) % 3] - keyM[1, (i + 2) % 3] * keyM[2, (i + 1) % 3]));
            return d;
        }

        public static int[,] Minor(int[,] incMatrix)
        {
            int[,] minor = new int[3, 3];
            minor[0, 0] = incMatrix[1, 1] * incMatrix[2, 2] - incMatrix[1, 2] * incMatrix[2, 1];
            minor[0, 1] = incMatrix[1, 0] * incMatrix[2, 2] - incMatrix[1, 2] * incMatrix[2, 0];
            minor[0, 2] = incMatrix[1, 0] * incMatrix[2, 1] - incMatrix[1, 1] * incMatrix[2, 0];
            minor[1, 0] = incMatrix[0, 1] * incMatrix[2, 2] - incMatrix[0, 2] * incMatrix[2, 1];
            minor[1, 1] = incMatrix[0, 0] * incMatrix[2, 2] - incMatrix[0, 2] * incMatrix[2, 0];
            minor[1, 2] = incMatrix[0, 0] * incMatrix[2, 1] - incMatrix[0, 1] * incMatrix[2, 0];
            minor[2, 0] = incMatrix[0, 1] * incMatrix[1, 2] - incMatrix[0, 2] * incMatrix[1, 1];
            minor[2, 1] = incMatrix[0, 0] * incMatrix[1, 2] - incMatrix[0, 2] * incMatrix[1, 0];
            minor[2, 2] = incMatrix[0, 0] * incMatrix[1, 1] - incMatrix[0, 1] * incMatrix[1, 0];

            return minor;
        }

        public static int[,] Cofactor(int[,] incMatrix)
        {
            int c = incMatrix.GetLength(0);
            int r = incMatrix.GetLength(1);

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    incMatrix[i, j] = incMatrix[i,j]*(int)(Math.Pow(-1.0d,(i+j)));
                }
            }

            return incMatrix;
        }

        public static int[,] TransposenMod(int[,] incMatrix)
        {
            int c = incMatrix.GetLength(0);
            int r = incMatrix.GetLength(1);

            int[,] trans = new int[r, c];

            for (int i = 0; i < c; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    trans[j, i] = incMatrix[i, j] % 29;
                    if (trans[i, j] <= 0) trans[i, j] += 29;
                }
            }

            return trans;
        }

        public static int ModularDet(int det)
        {
            int i;
            det %= 29;
            if (det <= 0) det += 29;

            for (i = 0; ; i++) { if ((det * i) % 29 == 1) break; }

            return i;
        }

        public static int[,] GetInverse(int mDet, int[,] cofT)
        {
            int[,] inversM = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    inversM[i, j] = mDet * cofT[i, j] % 29;
                    if (inversM[i, j] <= 0) inversM[i, j] += 29;
                }
            }
            return inversM;
        }
    }
}
