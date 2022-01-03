using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Particles
{
    class Particle
    {
        public bool active = true;
        public Vector2 position;
        public GameValue age;

        Texture2D sprite;
        public Vector2 spriteOrigin;
        public float renderedScale = 1f;
        public float rotation = 0f;

        public Particle(Texture2D _sprite, Vector2 _position, GameValue _age)
        {
            sprite = _sprite;
            spriteOrigin = sprite.Bounds.Center.ToVector2();

            position = _position;
            age = _age;
        }

        public virtual void Update()
        {
            age.Regenerate();
            if (age.Percent() >= 1f)
            {
                active = false;
            }
        }

        public virtual void Draw()
        {
            Main.spriteBatch.Draw(sprite, position, null, Color.White, rotation, spriteOrigin, renderedScale, SpriteEffects.None, 0f);
        }
    }
}
