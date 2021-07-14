using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Projectile
    {
        public interface IProjectiles {};
        public static List<IProjectiles> Projectiles = new List<IProjectiles>();    // Projectiles in the world
        //public static List<IProjectiles> PrefabProjectiles;                         // Prefabs of projectiles - Unused at the moment
        public static Dictionary<ProjectileType, Texture2D> Sprites;
        public delegate void ProjectileEvents(ProjectileEventType _eventType);
        public event ProjectileEvents OnCollide;

        public Vector2 Position;
        public Vector2 Size = Vector2.One * 64;
        public double Direction; // In Degrees angles
        public Vector2 IncrementedVector; // What to add to position to move (to avoid ddoing trigonometry every frame)
        public ProjectileType Type;

        public static void Initialize(Dictionary<ProjectileType,Texture2D> _projectileSprites)
        {
            Sprites = _projectileSprites;
        }

        public Projectile(double _direction, Vector2 _position)
        {
            Direction = _direction;
            Position = _position;

            IncrementedVector = new Vector2((float)Math.Cos(MathHelper.ToRadians((float)Direction)), (float)Math.Sin(MathHelper.ToRadians((float)Direction)));
        }

        public enum ProjectileType
        {
            Bullet,
            Arrow,
            Pellet
        }

        public enum ProjectileEventType
        {
            Hit,
            Despawn
        }

        public Texture2D Sprite;
        public float Speed = 1f;
        public float CollideDistance = 50f;
        public double Damage = 10;

        public void Update()
        {
            Move();
            if ((Position.X < 0) ||
                (Position.X > Main.screenSize.X) ||
                (Position.Y < 0) ||
                (Position.Y > Main.screenSize.Y))
            {
                
                Dispose(ProjectileEventType.Despawn);
            }
        }

        public void Move()
        {
            Position += IncrementedVector * Speed;
            CollisionCheck();
        }

        public void CollisionCheck()
        {
            float nearest = 2205f;
            Enemy candidate = null;
            foreach (Enemy x in Enemy.Enemies)
            {
                float current = Vector2.Distance(Position, x.Position);
                if (current < nearest)
                {
                    nearest = current;
                    candidate = x;
                }
            }

            if (candidate != null)
            {
                if (nearest <= CollideDistance)
                {
                    candidate.Health.AffectValue(-Damage);
                    candidate.ToggleAnger();
                    Dispose(ProjectileEventType.Hit);
                }
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(Sprite, Position, null, Color.White, MathHelper.ToRadians((float)Direction), Size / 2, 1f, SpriteEffects.None, 0f);
        }

        public void Dispose(ProjectileEventType _type)
        {
            if (OnCollide != null)
            {
                OnCollide(_type);
            }
            Main.UpdateEvent -= Update;
            Rendering.DrawEntities -= Draw;
            OnCollide = null;
        }
    }
}
