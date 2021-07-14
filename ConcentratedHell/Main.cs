using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        public static Vector2 screenSize = new Vector2(1920, 1080);
        public delegate void MainEvents();
        public static event MainEvents UpdateEvent;
        public static event MainEvents PlayerUpdateEvent;

        public static float FPS = 0;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = (int)screenSize.X;
            _graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            _graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            Cursor.Initialize(Content.Load<Texture2D>("Cursor"));
            Gun.Initialize(new Dictionary<Gun.GunType, Texture2D>() {
                { Gun.GunType.Glock, Content.Load<Texture2D>("Guns/Glock") },
                { Gun.GunType.Bow, Content.Load<Texture2D>("Guns/Bow") },
                { Gun.GunType.Shotgun, Content.Load<Texture2D>("Guns/Shotgun")}
            });
            Player.Initialize(Content.Load<Texture2D>("Player"), new List<Texture2D>() {
                Content.Load<Texture2D>("PlayerEyes"),
                Content.Load<Texture2D>("EyeBlink/EyeBlink1"),
                Content.Load<Texture2D>("EyeBlink/EyeBlink2"),
                Content.Load<Texture2D>("EyeBlink/EyeBlink3"),
                Content.Load<Texture2D>("EyeBlink/EyeBlink2"),
                Content.Load<Texture2D>("EyeBlink/EyeBlink1")
            });
            UI.Initialize(new SpriteBatch(GraphicsDevice), Content.Load<Texture2D>("Blank"), Content.Load<SpriteFont>("UIFont"));
            Bar.Initialize(Content.Load<Texture2D>("Blank"));
            Enemy.Initialize(Content.Load<Texture2D>("Enemy"), Content.Load<Texture2D>("PlayerEyes"), Content.Load<Texture2D>("EnemyEyes"));
            Projectile.Initialize(new Dictionary<Projectile.ProjectileType, Texture2D> {
                { Projectile.ProjectileType.Bullet , Content.Load<Texture2D>("Bullet") },
                { Projectile.ProjectileType.Arrow , Content.Load<Texture2D>("Arrow") },
                { Projectile.ProjectileType.Pellet , Content.Load<Texture2D>("Pellet") }
            });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Rendering.Initialize(new SpriteBatch(GraphicsDevice), new SpriteBatch(GraphicsDevice), new SpriteBatch(GraphicsDevice));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            FPS = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(PlayerUpdateEvent!=null)
            {
                PlayerUpdateEvent();
            }
            if (UpdateEvent != null && !Player.Instance.TimeStopped)
            {
                UpdateEvent();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Rendering.RenderObjects();
            UI.Instance.Draw();
            Rendering.RenderCursor();

            base.Draw(gameTime);
        }
    }
}
