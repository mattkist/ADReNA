using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADReNA_API.Util
{
    public static class Randomizer
    {
        private static Random rnd = new Random();

        /*
         * Retorna um valor double aleatório
         */
        public static double NextDouble()
        {
            return rnd.NextDouble();
        }

        /*
         * Retorna um valor double aleatório de 0 até determinado número
         */
        public static double NextDouble(int max)
        {
            return rnd.NextDouble() * max;
        }
    }
}
