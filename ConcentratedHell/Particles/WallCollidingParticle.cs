using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Particles
{
    class WallCollidingParticle:Particle
    {
        public WallCollidingParticle(Vector2 position) : base(Main.Instance.Content.Load<Texture2D>("Particles/wall_colliding_particle"), position, new GameValue(0, 5, 1, 0))
        {
            renderedScale = 1.5f;
        }

        public override void Update()
        {
            base.Update();

            color = Color.White * (1f - (float)age.Percent());
        }
    }
}
