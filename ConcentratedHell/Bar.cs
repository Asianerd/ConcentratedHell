using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Bar
    {
        Vector2 Start;
        Vector2 End;
        int Height;
        Color Color;
        static Texture2D BlankState;

        public static void Initialize(Texture2D _blank)
        {
            BlankState = _blank;
        }

        public Bar(Vector2 _start, Vector2 _end, Color _color, int _height)
        {
            Start = _start;
            End = _end;
            Color = _color;
            Height = _height;
        }

        public Bar(Color _color, int _height)
        {
            Color = _color;
            Height = _height;
        }

        public float Lerp(float _percentage)
        {
            return ((End.X - Start.X) * _percentage);
        }

        public float Lerp(float _percentage, Vector2 _startOverride, Vector2 _endOverride)
        {
            return ((_endOverride.X - _startOverride.X) * _percentage);
        }

        public void Draw(SpriteBatch _spriteBatch, float _value)
        {
            _spriteBatch.Draw(BlankState,
                new Rectangle(
                    (int)Start.X,
                    (int)Start.Y,
                    (int)Lerp(_value),
                    Height),
                Color
                );
        }

        public void Draw(SpriteBatch _spriteBatch, float _value, Vector2 _startPosition, Vector2 _endPosition)
        {
            _spriteBatch.Draw(BlankState,
                new Rectangle(
                    (int)_startPosition.X,
                    (int)_startPosition.Y,
                    (int)Lerp(_value, _startPosition, _endPosition),
                    Height),
                Color
                );
        }
    }
}
