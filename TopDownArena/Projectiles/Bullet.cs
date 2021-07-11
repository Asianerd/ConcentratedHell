using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Bullet : Projectile, Projectile.IProjectiles, IDisposable
    {
        Texture2D Sprite;
        float Speed = 1f;

        public Bullet(double _direction, Vector2 _position, float _speed) : base(_direction, _position)
        {
            Main.UpdateEvent += Update;
            Rendering.DrawEntities += Draw;

            Type = ProjectileType.Bullet;
            Sprite = Sprites[Type];

            Speed = _speed;

            Projectiles.Add(this);
        }

        public void Update()
        {
            Move();
            if ((Position.X < 0) ||
                (Position.X > Main.screenSize.X) ||
                (Position.Y < 0) ||
                (Position.Y > Main.screenSize.Y))
            {
                Dispose();
            }
        }

        public void Move()
        {
            Position += IncrementedVector * Speed;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(Sprite, (Position - DrawnOffset)*Universe.SCALE, null, Color.White, MathHelper.ToRadians((float)Direction), Vector2.One / 2, 1f, SpriteEffects.None, 0f);
        }

        public void Dispose()
        {
            Main.UpdateEvent -= Update;
            Rendering.DrawEntities -= Draw;

            Projectiles.Remove(this);
        }
    }
}
