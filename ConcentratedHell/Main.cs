using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using ConcentratedHell.Combat;

namespace ConcentratedHell
{
    public class Main : Game
    {
        public static Main Instance;

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
            Instance = this;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenSize.Width;
            graphics.PreferredBackBufferHeight = screenSize.Height;
            if(screenSize == new Rectangle(0, 0, 1920, 1080))
            {
                graphics.IsFullScreen = true;
            }
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            UI.Initialize();

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
                }
            });

            Map.placeholderSprite = Content.Load<Texture2D>("blank");
            Map.Initialize(new List<Tile>(){
                // Doors first to avoid weird graphic inconsistencies
                new Door(new Rectangle(0, 0, 64, 128), new Vector2(416, 256), new Vector2(416, 128)),

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
            Player.Initialize();
            Camera.Initialize();

            Skill.Initialize();
            Enemy.Initialize();

            Combat.Projectiles.Projectile.LoadContent();
            Combat.Projectiles.Projectile.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Cursor.LoadContent(Content.Load<Texture2D>("cursor"));
            UI.LoadContent(Content.Load<SpriteFont>("Fonts/mainFont"));

            mainFont = Content.Load<SpriteFont>("Fonts/MainFont");
            Player.LoadContent(Content.Load<Texture2D>("player"));
            Enemy.LoadContent(Content.Load<Texture2D>("Enemy/hotbar"));
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

            Matrix renderPosition = Matrix.CreateTranslation(new Vector3(Camera.Instance.offset, 0));
            spriteBatch.Begin(samplerState:SamplerState.PointClamp, transformMatrix:renderPosition);
            if (DrawEvent != null)
            {
                DrawEvent();
            }
            spriteBatch.End();

            spriteBatch.Begin();
            UI.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
