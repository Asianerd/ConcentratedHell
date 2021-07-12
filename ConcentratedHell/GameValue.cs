using System;
using System.Collections.Generic;
using System.Text;

namespace ConcentratedHell
{
    class GameValue
    {
        public string Name;
        public double I;
        public double Min;
        public double Max;
        public double Regeneration;
        //  |---------I----------|    |---I----------------|    |----------------I----|
        // Min                  Max  Min                  Max  Min                  Max


        public GameValue(string _name, double _min, double _max, double _regeneration, double _iPercent = 100.0) // i is some adsurbly random number i wont ever use in initialization
        {
            Name = _name;
            Min = _min;
            Max = _max;
            Regeneration = _regeneration;
            I = (_max - _min) * (_iPercent / 100);
        }

        public void Regenerate(double _multiplier = 1)
        {
            I += Regeneration * _multiplier;
            I = Math.Clamp(I, Min, Max);
        }

        public void AffectValue(double _amount, bool _limit = true)
        {
            I += _amount;
            if(_limit)
            {
                I = Math.Clamp(I, Min, Max);
            }
        }

        public double Percent()
        {
            return I / (Max - Min);
        }
    }
}
