using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TopDownArena
{
    class Player
    {
        #region Initialization stuff
        public static Player Instance;

        public static void Initialize(Texture2D _bodySprite, Texture2D _eyeSprite)
        {
            Instance = new Player(new Vector2(0, 0));
            BodySprite = _bodySprite;
            
            Instance.Size = new Vector2(64, 64); // size in pixels
            Rendering.DrawPlayer += Instance.Draw;
            Main.UpdateEvent += Instance.Update;

            Instance.Eyes = new PlayerEyes(_eyeSprite);
        }
        #endregion


        #region Fields
        #region Physics
        public Vector2 Position;
        public Vector2 Origin
        {
            get
            {
                return Position + (Size/2);
            }
        }
        public float Speed = 5f;
        public float SprintMultiplier = 2f;
        public Vector2 Size;
        #endregion

        #region Assets
        public static Texture2D BodySprite;
        public PlayerEyes Eyes;
        #endregion

        #region Math attributes
        public Vector2 MousePosition;
        public float DegreesToMouse;
        #endregion

        #region Game attributes
        public int Health;
        public int Stamina;
        #endregion
        #endregion

        Player(Vector2 position)
        {
            Position = position;
        }
        
        public void Update()
        {
            MousePosition = Mouse.GetState().Position.ToVector2();
            DegreesToMouse = MathHelper.ToDegrees((float)Math.Atan2(MousePosition.Y - Origin.Y, MousePosition.X - Origin.X));
            Move();
        }

        public void Move()
        {
            var _kInput = Keyboard.GetState();
            float _speed = _kInput.IsKeyDown(Keys.LeftShift) ? SprintMultiplier * Speed: Speed;
            if (_kInput.IsKeyDown(Keys.W))
            {
                Position.Y -= _speed;
            }
            if (_kInput.IsKeyDown(Keys.S))
            {
                Position.Y += _speed;
            }
            if (_kInput.IsKeyDown(Keys.A))
            {
                Position.X -= _speed;
            }
            if (_kInput.IsKeyDown(Keys.D))
            {
                Position.X += _speed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 _renderedPosition = new Vector2((float)(Position.X * Universe.SCALE), (float)(Position.Y * Universe.SCALE));
            spriteBatch.Draw(
                BodySprite,
                _renderedPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                Size/Rendering.SizingScale,
                SpriteEffects.None,
                0f
                );
            Eyes.Draw(spriteBatch, _renderedPosition, Size);
        }
    }
}
