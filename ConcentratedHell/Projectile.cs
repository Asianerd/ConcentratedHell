using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Projectile
    {
        //public static List<Projectile> Projectiles = new List<Projectile>(); // Unused at the moment
        public interface IProjectiles {};
        public static List<IProjectiles> Projectiles = new List<IProjectiles>();
        /*public static Texture2D[] Sprites;*/
        public static Dictionary<ProjectileType, Texture2D> Sprites;

        public Vector2 Position;
        public Vector2 DrawnOffset;
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
            DrawnOffset = Size / 2;

            IncrementedVector = new Vector2((float)Math.Cos(MathHelper.ToRadians((float)Direction)), (float)Math.Sin(MathHelper.ToRadians((float)Direction)));
        }

        public enum ProjectileType
        {
            Bullet,
            Arrow
        }
    }
}
