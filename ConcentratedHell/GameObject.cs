using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    abstract class GameObject
    {
        Vector2 position;
        float direction; // Radians

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);

        public void ApplyForce(ForceType type)
        {
            switch(type)
            {
                default:
                    break;
                case ForceType.Impulse:
                    break;
                case ForceType.Explosion:
                    break;
            }
        }

        public enum ForceType
        {
            Impulse,
            Explosion
        }
    }
}
