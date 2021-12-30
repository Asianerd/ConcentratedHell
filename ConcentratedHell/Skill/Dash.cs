using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ConcentratedHell
{
    class Dash : Skill
    {
        public override void Execute()
        {
            Debug.WriteLine("Dash was executed");
            if (cooldown.Percent() >= 0.5f)
            {
                Debug.WriteLine("Something");
                cooldown.I -= cooldown.PercentToValue(0.5f);
            }
        }
    }
}
