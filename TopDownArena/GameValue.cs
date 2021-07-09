using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownArena
{
    class GameValue
    {
        string Name;
        double I;
        double Min;
        double Max;
        double Regeneration;
        //  |--------------------|    |--------------------|    |--------------------|
        // Min        I         Max  Min  I               Max  Min               I  Max


        public GameValue(string _name)
        {

        }

        public void Regenerate()
        {
            I += Regeneration;
            I = Math.Clamp(I, Min, Max);
        }
    }
}
