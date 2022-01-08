using System;
using System.Linq;
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
        public static int maxCount = 2000000;

        public static void Initialize()
        {
            particles = new List<Particle>();

            Main.UpdateEvent += StaticUpdate;
        }

        public static void StaticUpdate()
        {
            foreach(Particle x in particles)
            {
                x.Update();
            }
            particles.RemoveAll(n => !n.active);
        }

        public static void StaticBackgroundDraw()
        {
            foreach (Particle x in particles.Where(n => n.depth == Depth.Background))
            {
                x.Draw();
            }
        }

        public static void StaticForegroundDraw()
        {
            foreach (Particle x in particles.Where(n => n.depth == Depth.Foreground))
            {
                x.Draw();
            }
        }
        #endregion

        public bool active = true;
        public Depth depth;
        public Vector2 position;
        public GameValue age;

        Texture2D sprite;
        public Vector2 spriteOrigin;
        public float renderedScale = 1f;
        public float rotation = 0f;
        public Color color = Color.White;

        public Particle(Texture2D _sprite, Vector2 _position, GameValue _age, Depth _depth = Depth.Foreground)
        {
            if (particles.Count >= maxCount)
            {
                return;
            }

            sprite = _sprite;
            spriteOrigin = sprite.Bounds.Center.ToVector2();

            position = _position;
            age = _age;

            depth = _depth;

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
            if (UI.UI.viewport.Contains(position))
            {
                Main.spriteBatch.Draw(sprite, position, null, color, rotation, spriteOrigin, renderedScale, SpriteEffects.None, 0f);
            }
        }

        public enum Depth
        {
            Background,
            Foreground
        }
    }
}
