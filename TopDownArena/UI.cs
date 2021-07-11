using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ConcentratedHell
{
    class UI
    {
        public static UI Instance = null;
        SpriteBatch spriteBatch;
        public static Texture2D blankState;

        #region Game attributes
        Bar HealthBar;
        Bar StaminaBar;
        #endregion

        public static void Initialize(SpriteBatch _spriteBatch, Texture2D _blankState)
        {
            if (Instance == null)
            {
                Instance = new UI();
                blankState = _blankState;

                Instance.spriteBatch = _spriteBatch;
                Instance.__privateInit__();
            }
        }

        void __privateInit__()
        {
            HealthBar = new Bar(new Vector2(27, 27), new Vector2(527, 0), Color.Red, 35);
            StaminaBar = new Bar(new Vector2(27, 67), new Vector2(427, 0), Color.Blue, 35);
        }


        public void Draw()
        {
            spriteBatch.Begin();
            HealthBar.Draw(spriteBatch, (float)Player.Instance.Health.Percent());
            StaminaBar.Draw(spriteBatch, (float)Player.Instance.Stamina.Percent());

            spriteBatch.End();
        }

        class Bar
        {
            Vector2 Start;
            Vector2 End;
            int Height;
            Color Color;

            public Bar(Vector2 _start, Vector2 _end, Color _color, int _height)
            {
                Start = _start;
                End = _end;
                Color = _color;
                Height = _height;
            }

            public float Lerp(float _percentage)
            {
                return ((End.X - Start.X) * _percentage);
            }

            public void Draw(SpriteBatch spriteBatch, float value)
            {
                spriteBatch.Draw(blankState,
                    new Rectangle(
                        (int)Start.X,
                        (int)Start.Y,
                        (int)Lerp(value),
                        Height),
                    Color
                    );
            }
        }
    }
}
