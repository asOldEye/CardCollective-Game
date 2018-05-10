using System;

namespace AuxiliaryLibrary
{
    public class DistributionRandom : Random
    {
        public bool NextPercent(int percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentException("Percent value must be in the interim [0,100]");

            if (base.Next(0, 101) < percent) return true;
            else return false;
        }
    }
}