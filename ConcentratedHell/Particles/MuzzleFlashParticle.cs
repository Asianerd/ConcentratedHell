using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Particles
{
    class MuzzleFlashParticle:Particle
    {
        float direction;
        Vector2 increment;
        float speed;

        public MuzzleFlashParticle(Vector2 position, float _direction, float _speed):base(Main.Instance.Content.Load<Texture2D>("Particles/muzzle_flash"), position, new GameValue(0, 10, 1, 0))
        {
            direction = _direction;
            speed = _speed;

            increment = new Vector2(
                MathF.Cos(direction) * speed,
                MathF.Sin(direction) * speed
                );

            renderedScale = 5f;
        }

        public override void Update()
        {
            base.Update();

            float inverseAge = 1f - (float)age.Percent();
            position += increment * inverseAge * Universe.speedMultiplier;
            color = Color.White * (inverseAge + 0.2f);
            rotation += speed * inverseAge;
            renderedScale = 5f * inverseAge;
        }
    }
}
