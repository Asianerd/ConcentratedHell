using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TopDownArena
{
    class PlayerEyes
    {
        public Texture2D Sprite;
        public float EyeDistance = 5f;

        public PlayerEyes(Texture2D _sprite)
        {
            Sprite = _sprite;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 _position, Vector2 _size)
        {
            Vector2 _offsetPosition = new Vector2(
                ((float)Math.Cos(MathHelper.ToRadians(Player.Instance.DegreesToMouse)) * EyeDistance),
                ((float)Math.Sin(MathHelper.ToRadians(Player.Instance.DegreesToMouse)) * EyeDistance)
                );

            spriteBatch.Draw(
                Sprite,
                _position+_offsetPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                _size/ Rendering.SizingScale,
                SpriteEffects.None,
                0f
                );
        }
    }
}
