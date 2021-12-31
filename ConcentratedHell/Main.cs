using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace ConcentratedHell
{
    public class Main : Game
    {
        public static Rectangle screenSize = new Rectangle(0, 0, 1000, 1000);
        public static SpriteFont mainFont;
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public delegate void RenderEvent();
        public static RenderEvent DrawEvent;

        public delegate void GameEvent();
        public static GameEvent UpdateEvent;

        public static KeyboardState keyboardState;
        public static MouseState mouseState;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenSize.Width;
            graphics.PreferredBackBufferHeight = screenSize.Height;
            if(screenSize == new Rectangle(0, 0, 1920, 1080))
            {
                graphics.IsFullScreen = true;
                graphics.ApplyChanges();
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
                        graphics.IsFullScreen = !graphics.IsFullScreen;
                        graphics.ApplyChanges();
                    })
                },
                { Keys.LeftShift, new Input(Keys.LeftShift) },
                { Keys.B, new Input(Keys.B, () => {
                        var x = new Cyborg(Player.Instance.rect);
                    })
                },
            });
            Player.Initialize();
            Camera.Initialize();

            Skill.Initialize();
            Enemy.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mainFont = Content.Load<SpriteFont>("Fonts/MainFont");
            Player.LoadContent(Content.Load<Texture2D>("player"));
            string _enemySpritePath = "Enemy";
            Enemy.LoadContent(new Dictionary<Enemy.Type, Texture2D>()
            {
                { Enemy.Type.Cyborg, Content.Load<Texture2D>($"{_enemySpritePath}/Cyborg") },
            });
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
            spriteBatch.Begin(samplerState:SamplerState.PointClamp, transformMatrix:renderPosition);
            if (DrawEvent != null)
            {
                DrawEvent();
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
