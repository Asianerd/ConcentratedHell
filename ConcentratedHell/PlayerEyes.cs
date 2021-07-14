using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ConcentratedHell
{
    class PlayerEyes
    {
        public List<Texture2D> EyeSprites;
        //Neutral, Blink 1, Blink 2, Blink 3 (fully closed), Blink 2, Blink 1
        public float EyeDistance = 5f;

        public GameValue EyeBlink;

        public PlayerEyes(List<Texture2D> _sprite)
        {
            EyeSprites = _sprite;
            EyeBlink = new GameValue("EyeBlink", 0, 2040, 1,30);
        }

        public void Update()
        {
            EyeBlink.AffectValue(-5, false);
            if (EyeBlink.I <= 0)
            {
                EyeBlink.I = Universe.RANDOM.Next(0, (int)EyeBlink.Max);
            }
        }

        public int EyeState()
        {
            int final;

            final = (int)(EyeBlink.I / 20);
            if (final >= 6)
            {
                return 0;
            }

            return final;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 _position, Vector2 _size)
        {
            Vector2 _offsetPosition = new Vector2(
                ((float)Math.Cos(Player.Instance.RadiansToMouse) * EyeDistance),
                ((float)Math.Sin(Player.Instance.RadiansToMouse) * EyeDistance)
                );

            spriteBatch.Draw(
                EyeSprites[EyeState()],
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
