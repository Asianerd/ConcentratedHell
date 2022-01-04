using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Particles
{
    class Particle
    {
        #region Statics
        public static List<Particle> particles;

        public static void Initialize()
        {
            particles = new List<Particle>();

            Main.UpdateEvent += StaticUpdate;
            Main.DrawEvent += StaticDraw;
        }

        public static void StaticUpdate()
        {
            foreach(Particle x in particles)
            {
                x.Update();
            }
            particles.RemoveAll(n => !n.active);
        }

        public static void StaticDraw()
        {
            foreach(Particle x in particles)
            {
                x.Draw();
            }
        }
        #endregion

        public bool active = true;
        public Vector2 position;
        public GameValue age;

        Texture2D sprite;
        public Vector2 spriteOrigin;
        public float renderedScale = 1f;
        public float rotation = 0f;
        public Color color = Color.White;

        public Particle(Texture2D _sprite, Vector2 _position, GameValue _age)
        {
            sprite = _sprite;
            spriteOrigin = sprite.Bounds.Center.ToVector2();

            position = _position;
            age = _age;

            particles.Add(this);
        }

        public virtual void Update()
        {
            age.Regenerate(Universe.speedMultiplier);
            if (age.Percent() >= 1f)
            {
                active = false;
            }
        }

        public virtual void Draw()
        {
            Main.spriteBatch.Draw(sprite, position, null, color, rotation, spriteOrigin, renderedScale, SpriteEffects.None, 0f);
        }
    }
}
