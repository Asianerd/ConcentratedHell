using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Particles
{
    class GoreParticle:Particle
    {
        Vector2 increment;
        float speed;

        public GoreParticle(Vector2 position, float direction, float _distance) : base(
            Main.Instance.Content.Load<Texture2D>("Particles/gore"),
            position,
            new GameValue(0, Main.random.Next(20, 50), 1, 0)
            )
        {
            float distance = _distance * (Main.random.Next(50, 150) / 100f);

            increment = new Vector2(
                MathF.Cos(direction) * (distance / 5f),
                MathF.Sin(direction) * (distance / 5f)
                );

            renderedScale = 2f;

            speed = Main.random.Next(20, 100) / 100f;
        }

        public override void Update()
        {
            base.Update();

            float inverseAge = 1f - (float)age.Percent();
            position += increment * inverseAge * Universe.speedMultiplier;

            rotation += 0.1f * inverseAge * Universe.speedMultiplier;

            color = Color.White * (inverseAge + 0.2f);
        }
    }
}
