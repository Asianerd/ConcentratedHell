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
        public static Vector2 BoundingBox;
        public static Color BackgroundColor;
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

            BoundingBox = new Vector2(1908, 1040);
            BackgroundColor = new Color(51, 51, 51);

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            Cursor.Initialize(Content.Load<Texture2D>("Cursor"));
            Item.Initialize(
                new Dictionary<Item.ItemClass, Dictionary<object, Texture2D>>()
                {
                    {
                        Item.ItemClass.Ammo,
                        new Dictionary<object, Texture2D>() {
                            { Projectile.ProjectileType.Arrow, Content.Load<Texture2D>("Arrow") },
                            { Projectile.ProjectileType.Bullet, Content.Load<Texture2D>("Bullet") },
                            { Projectile.ProjectileType.Pellet, Content.Load<Texture2D>("Pellet") },
                            { Projectile.ProjectileType.LightShard, Content.Load<Texture2D>("LightShard") },
                            { Projectile.ProjectileType.SeekingMissile, Content.Load<Texture2D>("SeekingMissile") },
                            { Projectile.ProjectileType.Grenade, Content.Load<Texture2D>("Grenade") },
                            { Projectile.ProjectileType.GravTrap, Content.Load<Texture2D>("GravTrap") }
                        }
                    },
                    {
                        Item.ItemClass.Equipment,
                        new Dictionary<object, Texture2D>()
                        {

                        }
                    },
                    {
                        Item.ItemClass.Loot,
                        new Dictionary<object, Texture2D>()
                        {

                        }
                    },
                    {
                        Item.ItemClass.Weapon,
                        new Dictionary<object, Texture2D>()
                        {

                        }
                    }
                },
                new Dictionary<object, string>()
                {
                    { Projectile.ProjectileType.Arrow, "Arrow" },
                    { Projectile.ProjectileType.Bullet, "Bullet" },
                    { Projectile.ProjectileType.Pellet, "Pellet" },
                    { Projectile.ProjectileType.LightShard, "Light Shard" },
                    { Projectile.ProjectileType.SeekingMissile, "Seeking Missile" },
                    { Projectile.ProjectileType.Grenade, "Grenade" },
                    { Projectile.ProjectileType.GravTrap, "Grav Trap" }
                },
                Content.Load<SpriteFont>("Fonts/MainFont")
                );
            Gun.Initialize(new Dictionary<Gun.GunType, Texture2D>() {
                { Gun.GunType.Glock, Content.Load<Texture2D>("Guns/Glock") },
                { Gun.GunType.Bow, Content.Load<Texture2D>("Guns/Bow") },
                { Gun.GunType.Shotgun, Content.Load<Texture2D>("Guns/Shotgun")},
                { Gun.GunType.PlasmaPrism, Content.Load<Texture2D>("Guns/PlasmaPrism") },
                { Gun.GunType.MissileLauncher, Content.Load<Texture2D>("Guns/MissileLauncher") },
                { Gun.GunType.GrenadeLauncher, Content.Load<Texture2D>("Guns/GrenadeLauncher") },
                { Gun.GunType.Trapper, Content.Load<Texture2D>("Guns/Trapper") }
            });
            Enemy.Initialize(Content.Load<Texture2D>("Enemy"), Content.Load<Texture2D>("PlayerEyes"), Content.Load<Texture2D>("EnemyEyes"));
            Player.Initialize(Content.Load<Texture2D>("Player"), new List<Texture2D>() {
                Content.Load<Texture2D>("PlayerEyes"),
                Content.Load<Texture2D>("EyeBlink/EyeBlink1"),
                Content.Load<Texture2D>("EyeBlink/EyeBlink2"),
                Content.Load<Texture2D>("EyeBlink/EyeBlink3"),
                Content.Load<Texture2D>("EyeBlink/EyeBlink2"),
                Content.Load<Texture2D>("EyeBlink/EyeBlink1")
            });
            UI.Initialize(Content.Load<Texture2D>("Blank"), Content.Load<SpriteFont>("Fonts/MainFont"), Content.Load<Texture2D>("RadialWheel"), Content.Load<Texture2D>("RadialWheelSelection"));
            Bar.Initialize(Content.Load<Texture2D>("Blank"));
            Projectile.Initialize(new Dictionary<Projectile.ProjectileType, Texture2D> {
                { Projectile.ProjectileType.Bullet, Content.Load<Texture2D>("Bullet") },
                { Projectile.ProjectileType.Arrow, Content.Load<Texture2D>("Arrow") },
                { Projectile.ProjectileType.Pellet, Content.Load<Texture2D>("Pellet") },
                { Projectile.ProjectileType.LightShard, Content.Load<Texture2D>("LightShard") },
                { Projectile.ProjectileType.SeekingMissile, Content.Load<Texture2D>("SeekingMissile") },
                { Projectile.ProjectileType.Grenade, Content.Load<Texture2D>("Grenade") },
                { Projectile.ProjectileType.GravTrap, Content.Load<Texture2D>("GravTrap") }
            });
            Rendering.Initialize(new SpriteBatch(GraphicsDevice), Content.Load<Texture2D>("Background"), Content.Load<Texture2D>("Wall"));
            DamageBubble.Initialize(Content.Load<SpriteFont>("Fonts/MainFont"));
            base.Initialize();
        }

        protected override void LoadContent()
        {


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (PlayerUpdateEvent != null)
            {
                PlayerUpdateEvent();
            }
            if (UpdateEvent != null && !Player.Instance.TimeStopped)
            {
                UpdateEvent();
            }

            Enemy.StaticUpdate();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);
            FPS = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            Rendering.RenderObjects();

            base.Draw(gameTime);
        }
    }
}
