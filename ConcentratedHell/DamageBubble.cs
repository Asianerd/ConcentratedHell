using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class DamageBubble
    {
        public static SpriteFont BubbleFont;

        public Vector2 Position;
        public GameValue Age;
        public double Damage;

        public static void Initialize(SpriteFont _spriteFont)
        {
            BubbleFont = _spriteFont;
        }

        public DamageBubble(Vector2 _position, double _damage, int _deathAge = 100)
        {
            Main.UpdateEvent += Update;
            Rendering.DrawEntities += Draw;

            Position = new Vector2(_position.X - (Damage.ToString().Length * 35f), _position.Y);
            Age = new GameValue("Age", 0, _deathAge, 1, 0);
            Damage = Math.Round(_damage, 2);
        }

        void Update()
        {
            Age.Regenerate();
            if(Age.Percent() == 1f)
            {
                Dispose();
            }
        }

        void Draw(SpriteBatch _spriteBatch)
        {
            Vector2 _renderPosition = new Vector2(Position.X, (float)(Position.Y - ((Math.Log(Age.I)*2))));
            _spriteBatch.DrawString(BubbleFont, Damage.ToString(), _renderPosition, Color.OrangeRed);
        }

        void Dispose()
        {
            Main.UpdateEvent -= Update;
            Rendering.DrawEntities -= Draw;
        }
    }
}
