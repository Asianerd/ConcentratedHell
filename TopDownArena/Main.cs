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

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = (int)screenSize.X;
            _graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            _graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Player.Initialize(Content.Load<Texture2D>("Player"),Content.Load<Texture2D>("PlayerEyes"));
            UI.Initialize(new SpriteBatch(GraphicsDevice), Content.Load<Texture2D>("Blank"));
            Enemy.Initialize(Content.Load<Texture2D>("Enemy"), Content.Load<Texture2D>("PlayerEyes"), Content.Load<Texture2D>("EnemyEyes"));
            Projectile.Initialize(new Dictionary<Projectile.ProjectileType, Texture2D> {
                { Projectile.ProjectileType.Bullet , Content.Load<Texture2D>("Bullet") }
                /*{ Projectile.ProjectileType.Arrow , Content.Load<Texture2D>("Arrow") }*/
            });
            base.Initialize();

            Enemy.Enemies.Add(new Enemy(new Vector2(100, 100)));
            Enemy.Enemies.Add(new Enemy(new Vector2(300, 300)));

            Bullet x = new Bullet(90f, new Vector2(500, 500), 1f);
        }

        protected override void LoadContent()
        {
            Rendering.Initialize(new SpriteBatch(GraphicsDevice), new SpriteBatch(GraphicsDevice));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(UpdateEvent != null)
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

            base.Draw(gameTime);
        }
    }
}
