using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opencv
{
    static class Extensions
    {
        // Расширение float для случайности параметров
        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)(min + (random.NextDouble() * (max - min)));
        }
    }
}
