using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Particles
{
    class SmokeParticle:Particle
    {
        Vector2 increment;
        float speed;

        public SmokeParticle(Vector2 origin, float _direction, float _distance) : base(
            Main.Instance.Content.Load<Texture2D>("Particles/smoke"),
            new Vector2(
                MathF.Cos(_direction) * _distance + origin.X,
                MathF.Sin(_direction) * _distance + origin.Y
                ),
            new GameValue(0, 60, 1, 0)
            )
        {
            increment = new Vector2(
                MathF.Cos(_direction) * _distance,
                MathF.Sin(_direction) * _distance
                );

            speed = (Main.random.Next(10, 100) / 100f);
        }

        public override void Update()
        {
            base.Update();

            float inverseProgress = 1f - (float)age.Percent();
            position += increment * inverseProgress * Universe.speedMultiplier;
            renderedScale = inverseProgress * 2f;
            rotation += speed * Universe.speedMultiplier;
        }
    }
}
