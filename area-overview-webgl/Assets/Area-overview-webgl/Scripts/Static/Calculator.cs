using System;

namespace Area_overview_webgl.Scripts.Static
{
    /**
     * This static class consist functions that can be using in game calculation
     */
    public static class Calculator
    {
        /*
         * Get pow number of 2 
         * powVal = 8, return 3, because 2*2*2
         * powWal = 16, return 4, because 2*2*2*2
         * powWal = 128, return 7...
         */
        public static int GetPowNumber2(int powVal)
        {
            var result = 0;
            while (powVal > 1)
            {
                if (powVal % 2 == 0)
                {
                    powVal /= 2;
                    result++;
                }
                else
                {
                    throw new ArgumentException("Value must be a power of 2.");
                }
            }
            return result;
        }
    }
}