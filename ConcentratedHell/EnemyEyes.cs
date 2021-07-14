using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ConcentratedHell
{
    class EnemyEyes
    {
        public static Texture2D PassiveSprite;
        public static Texture2D AngrySprite;
        public Texture2D SpriteUsed; // To avoid an if statement every frame
        public float EyeDistance = 5f;
        public int EyeState;
        public Enemy Parent;

        public static void Initialize(Texture2D _passiveSprite, Texture2D _angrySprite)
        {
            PassiveSprite = _passiveSprite;
            AngrySprite = _angrySprite;
        }

        public EnemyEyes(Enemy _parent)
        {
            Parent = _parent;
            EyeState = Universe.RANDOM.Next(0, 300);
            SpriteUsed = PassiveSprite;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 _position, Vector2 _size)
        {
            Vector2 _offsetPosition = new Vector2(
                ((float)Math.Cos((float)Parent.AngleToPlayer) * EyeDistance),
                ((float)Math.Sin((float)Parent.AngleToPlayer) * EyeDistance)
                );

            spriteBatch.Draw(
                SpriteUsed,
                _position + _offsetPosition,
                null,
                Color.White,
                0f,
                _size/2,
                _size / Rendering.SizingScale,
                SpriteEffects.None,
                0f
                );
        }
    }
}
