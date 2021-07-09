using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TopDownArena
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
            base.Initialize();
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

            System.Diagnostics.Debug.WriteLine(1 / (float)gameTime.ElapsedGameTime.TotalSeconds);

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
