using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ConcentratedHell.Combat.Projectiles
{
    class Plasma_orb:Projectile
    {
        public Plasma_orb(Vector2 position, float direction) : base(Type.Plasma_orb, 20f, 5f, position, direction)
        {

        }

        public override void Update()
        {
            base.Update();
            rotation += 0.1f * Universe.speedMultiplier;
        }
    }
}
