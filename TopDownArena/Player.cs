using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ConcentratedHell
{
    class Player
    {
        #region Initialization stuff
        public static Player Instance;

        public static void Initialize(Texture2D _bodySprite, Texture2D _eyeSprite)
        {
            Instance = new Player(Main.screenSize / 2);
            Instance.__privateInit__(_bodySprite, _eyeSprite);
        }

        void __privateInit__(Texture2D _bodySprite, Texture2D _eyeSprite)
        {
            BodySprite = _bodySprite;

            Size = new Vector2(64, 64); // size in pixels
            Rendering.DrawPlayer += Draw;
            Main.UpdateEvent += Update;

            Eyes = new PlayerEyes(_eyeSprite);

            #region Game attributes
            Health = new GameValue("Health", 0, 100, 0.1);
            Stamina = new GameValue("Stamina", 0, 500, 2);
            #endregion
        }
        #endregion


        #region Fields
        #region Physics
        public Vector2 Position;
        Vector2 DrawnOffset;
        public Vector2 DrawPosition
        {
            get
            {
                return Position - DrawnOffset;
            }
        }
        public float Speed = 5f;
        public float SprintMultiplier = 2f;
        public Vector2 Size = new Vector2(64 , 64);
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
        public GameValue Health;
        public GameValue Stamina;
        #endregion
        #endregion

        Player(Vector2 position)
        {
            Position = position;
            DrawnOffset = Size / 2;
        }
        
        public void Update()
        {
            MousePosition = Mouse.GetState().Position.ToVector2();
            DegreesToMouse = Universe.ANGLETO(Position, MousePosition);
            Move();
        }

        public void Move()
        {
            var _kInput = Keyboard.GetState();
            bool _sprinting = _kInput.IsKeyDown(Keys.LeftShift);

            float _speed = _sprinting && (Stamina.I > 0) ? SprintMultiplier * Speed : Speed;

            Vector2 _startingPosition = Position;
            Vector2 MovingDirection = new Vector2(0, 0);
            if (_kInput.IsKeyDown(Keys.W))
            {
                MovingDirection.Y = -1;
            }
            if (_kInput.IsKeyDown(Keys.S))
            {
                MovingDirection.Y = 1;
            }
            if (_kInput.IsKeyDown(Keys.A))
            {
                MovingDirection.X = -1;
            }
            if (_kInput.IsKeyDown(Keys.D))
            {
                MovingDirection.X = 1;
            }
            if (MovingDirection != Vector2.Zero)
            {
                MovingDirection.Normalize();
            }
            Position += MovingDirection * _speed;

            Vector2 _endingPosition = Position;

            if (_sprinting && ((_startingPosition-_endingPosition) != Vector2.Zero))
            {
                Stamina.AffectValue(-7);
            }
            else
            {
                Stamina.Regenerate();
            }

            Health.Regenerate();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            //Vector2 _renderedPosition = new Vector2((float)(Position.X * Universe.SCALE), (float)(Position.Y * Universe.SCALE));
            
            spriteBatch.Draw(
                BodySprite,
                DrawPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                Size/Rendering.SizingScale,
                SpriteEffects.None,
                0f
                );
            Eyes.Draw(spriteBatch, DrawPosition, Size);
        }
    }
}
