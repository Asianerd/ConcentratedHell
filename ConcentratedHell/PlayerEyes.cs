using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ConcentratedHell
{
    class PlayerEyes
    {
        public Texture2D NeutralSprite;
        //public List<Texture2D> BlinkSprites;
        public float EyeDistance = 5f;
        public int EyeState;

        public PlayerEyes(Texture2D _sprite)
        {
            NeutralSprite = _sprite;
            EyeState = Universe.RANDOM.Next(0, 300);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 _position, Vector2 _size)
        {
            Vector2 _offsetPosition = new Vector2(
                ((float)Math.Cos(MathHelper.ToRadians(Player.Instance.DegreesToMouse)) * EyeDistance),
                ((float)Math.Sin(MathHelper.ToRadians(Player.Instance.DegreesToMouse)) * EyeDistance)
                );

            spriteBatch.Draw(
                NeutralSprite,
                _position+_offsetPosition,
                null,
                Color.White,
                0f,
                _size/2,
                _size/ Rendering.SizingScale,
                SpriteEffects.None,
                0f
                );
        }
    }
}
