using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TopDownArena
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        public delegate void MainEvents();
        public static event MainEvents UpdateEvent;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Player.Initialize(Content.Load<Texture2D>("Player"),Content.Load<Texture2D>("PlayerEyes"));
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Rendering.RenderObjects();

            base.Draw(gameTime);
        }
    }
}
