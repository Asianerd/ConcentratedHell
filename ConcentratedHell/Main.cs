using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace ConcentratedHell
{
    public class Main : Game
    {
        public static Rectangle screenSize = new Rectangle(0, 0, 500, 500);
        //public static Rectangle screenSize = new Rectangle(0, 0, 800, 800);
        public static SpriteFont mainFont;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public delegate void RenderEvent(SpriteBatch spriteBatch);
        public static RenderEvent DrawEvent;

        public delegate void GameEvent();
        public static GameEvent UpdateEvent;

        public static KeyboardState keyboardState;
        public static MouseState mouseState;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = screenSize.Width;
            _graphics.PreferredBackBufferHeight = screenSize.Height;
            if(screenSize == new Rectangle(0, 0, 1920, 1080))
            {
                _graphics.IsFullScreen = true;
                _graphics.ApplyChanges();
            }

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Map.placeholderSprite = Content.Load<Texture2D>("blank");
            Map.Initialize(new List<Tile>(){
                new Tile(new Rectangle(-128, -128, 1152, 128)),
                new Tile(new Rectangle(-128, -128, 128, 896)),
                new Tile(new Rectangle(896, -128, 128, 896)),
                new Tile(new Rectangle(-128, 640, 1152, 128)),

                new Tile(new Rectangle(128, 128, 128, 128)),
                new Tile(new Rectangle(128, 384, 128, 128)),
                new Tile(new Rectangle(640, 256, 128, 128)),

                new Tile(new Rectangle(384, 0, 128, 256)),
                new Tile(new Rectangle(384, 384, 128, 256)),
            });
            Input.Initialize(new Dictionary<Keys, Input>()
            {
                { Keys.F11,
                    new Input(Keys.F11, () => {
                        _graphics.IsFullScreen = !_graphics.IsFullScreen;
                        _graphics.ApplyChanges();
                    })
                },
                { Keys.LeftShift, new Input(Keys.LeftShift) },
            });
            Player.Initialize();
            Camera.Initialize();

            Skill.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            mainFont = Content.Load<SpriteFont>("Fonts/MainFont");
            Player.LoadContent(Content.Load<Texture2D>("player"));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            Input.StaticUpdate();

            if (UpdateEvent != null)
            {
                UpdateEvent();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix renderPosition = Matrix.CreateTranslation(new Vector3((screenSize.Size.ToVector2() / 2f) - Camera.Instance.position, 0));
            _spriteBatch.Begin(samplerState:SamplerState.PointClamp, transformMatrix:renderPosition);
            if (DrawEvent != null)
            {
                DrawEvent(_spriteBatch);
            }

            _spriteBatch.DrawString(mainFont, $"{Camera.Instance.position}\n{Player.Instance.rect.Location}", Vector2.Zero, Color.White);
            _spriteBatch.End();

            /*_spriteBatch.Begin();
            _spriteBatch.Draw(Map.placeholderSprite, screenSize.Size.ToVector2() / 2f, Color.White);
            _spriteBatch.End();*/

            base.Draw(gameTime);
        }
    }
}
