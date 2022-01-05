using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Cyborg:Entity
    {
        public Cyborg(Rectangle rect) : base(rect, Type.Cyborg, new GameValue(0, 100, 1), 7f)
        {

        }
    }
}
