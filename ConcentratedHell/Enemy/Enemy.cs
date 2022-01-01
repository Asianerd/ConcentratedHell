using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Enemy
    {
        Rectangle rect;
        GameValue health;
        float direction;
        Type type;
        Texture2D sprite;

        public Enemy(Rectangle _rect, Type _type)
        {
            rect = _rect;
            type = _type;
            sprite = spriteTable[type];
            collection.Add(this);
        }

        public virtual void Update()
        {
            PathFind(Player.Instance.rect.Location, 5f);
        }

        public virtual void PathFind(Point target, float _speed)
        {
            float distance = MathF.Sqrt(MathF.Pow(MathF.Abs(target.X - rect.X), 2) + MathF.Pow(MathF.Abs(target.Y - rect.Y), 2));
            // (x + y)^0.5

            float speed = distance < _speed ? distance : _speed;

            direction = MathF.Atan2(
                target.Y - rect.Y,
                target.X - rect.X
                );
            Vector2 increment = new Vector2(
                (int)(MathF.Cos(direction) * speed),
                (int)(MathF.Sin(direction) * speed)
                );

            Rectangle candidate = new Rectangle(
                rect.Location + increment.ToPoint(),
                rect.Size
                );

            if(increment != Vector2.Zero)
            {
                increment.Normalize();
            }

            if(Map.IsValidPosition(candidate))
            {
                rect.Location = candidate.Location;
                return;
            }

            Point xVel = new Point((int)(increment.X * speed), 0);
            Rectangle xRect = new Rectangle(rect.Location + xVel, rect.Size);
            if(Map.IsValidPosition(xRect))
            {
                rect.Location = xRect.Location;
            }

            Point yVel = new Point(0, (int)(increment.Y * speed));
            Rectangle yRect = new Rectangle(rect.Location + yVel, rect.Size);
            if(Map.IsValidPosition(yRect))
            {
                rect.Location = yRect.Location;
            }
        }

        public virtual void AffectHealth(double damage)
        {
            health.AffectValue(damage);
        }

        public virtual void Die()
        {
            collection.Remove(this);
        }

        public virtual void Draw()
        {
            Main.spriteBatch.Draw(sprite, rect, Color.White);
        }

        #region Statics
        public static Dictionary<Type, Texture2D> spriteTable;
        public static List<Enemy> collection;

        public static void Initialize()
        {
            collection = new List<Enemy>();
            Main.UpdateEvent += StaticUpdate;
            Main.DrawEvent += StaticDraw;
        }

        public static void LoadContent(Dictionary<Type, Texture2D> _spriteTable)
        {
            spriteTable = _spriteTable;
        }

        public static void StaticUpdate()
        {
            foreach(Enemy x in collection)
            {
                x.Update();
            }
        }

        public static void StaticDraw()
        {
            foreach(Enemy x in collection)
            {
                x.Draw();
            }
        }

        public enum Type
        {
            Cyborg
        }
        #endregion
    }
}
