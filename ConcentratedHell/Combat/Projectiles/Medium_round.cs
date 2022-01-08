using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ConcentratedHell.Combat.Projectiles
{
    class Medium_round:Projectile
    {
        public Medium_round(Vector2 position, float direction) : base(Type.Medium_round, 20f, 10f, position, direction)
        {

        }
    }
}
