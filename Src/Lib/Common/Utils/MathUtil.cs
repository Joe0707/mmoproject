using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utils
{
    public class MathUtil
    {
        public static Random Random = new Random();
        public static int RoundToInt(float f)
        {
            return (int)Math.Round((double)f);
        }

        public static float Clamp01(float value)
        {
            if (value < 0f)
            {
                value = 0f;
            }
            else if (value > 1f)
            {
                value = 1f;
            }
            return value;
        }


        public static float Clamp(float value,float min,float max)
        {
            if (value < min)
            {
                value = min;
            }else if(value > max)
            {
                value = max;
            }
            return value;
        }
    }
}
