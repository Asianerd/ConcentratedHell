using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Particles
{
    class AmmoCartridgeParticle:Particle
    {
        Vector2 angledIncrement;

        float y = -5f;
        float x;
        float r = 0.2f;
        float speed;

        float yCapM;
        bool secondBounce = false;

        public AmmoCartridgeParticle(Ammo.Type type, Vector2 position, float _direction) : base(Main.Instance.Content.Load<Texture2D>($"Particles/AmmoCartridge/{type.ToString().ToLower()}"), position, new GameValue(0, 120, 1, 0), _depth:Depth.Background)
        {
            renderedScale = 4f;

            angledIncrement = new Vector2(MathF.Cos(_direction), MathF.Sin(_direction));

            rotation = MathF.PI * 2f * (Main.random.Next(0, 100) / 100f);
            x = (Main.random.Next(-100, 100) / 100f) ;

            yCapM = (Main.random.Next(80, 120) / 100f);
            speed = Main.random.Next(90, 110) / 100f;
        }

        public override void Update()
        {
            base.Update();

            if (y*yCapM < (secondBounce?3.5f:5f))
            {
                if (secondBounce && (y >= 2f))
                {
                    r *= 0.8f * speed;
                }

                position.Y += y * speed;
                position.X += x * speed;
                rotation += r * speed;
                y += 0.5f * (1f - (float)age.Percent());
            }
            else
            {
                if (!secondBounce)
                {
                    secondBounce = true;
                    y = -3f;
                    x *= 2f;
                    r *= 1.2f;
                }
                else
                {
                    r *= 0.1f;
                    rotation += r;
                }
            }

            color = Color.White * (1f-(float)age.Percent());
        }
    }
}
