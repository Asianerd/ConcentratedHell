using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using ConcentratedHell.Combat;
using ConcentratedHell.Entity;

namespace ConcentratedHell
{
    public class Main : Game
    {
        public static Main Instance;

        //public static Rectangle screenSize = new Rectangle(0, 0, 800, 800);
        public static Rectangle screenSize = new Rectangle(0, 0, 1920, 1080);
        public static SpriteFont mainFont;
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public delegate void GameEvent();
        public static GameEvent UpdateEvent;
        public static GameEvent ForegroundDrawEvent;
        public static GameEvent MidgroundDrawEvent;
        public static GameEvent BackgroundDrawEvent;

        public static Random random;
        public static KeyboardState keyboardState;
        public static MouseState mouseState;

        public Main()
        {
            Instance = this;
            random = new Random();

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
            base.Initialize();

            StaticInitialize();
        }

        public static void StaticInitialize()
        {
            UpdateEvent = null;
            ForegroundDrawEvent = null;
            MidgroundDrawEvent = null;
            BackgroundDrawEvent = null;

            UI.MainMenu.Initialize();

            UI.UI.Initialize();
            UI.PauseMenu.Initialize();
            UI.Button.Initialize();

            Input.Initialize(new Dictionary<Keys, Input>()
            {
                { Keys.Escape,
                    new Input(Keys.Escape, () =>
                    {
                        Universe.paused = !Universe.paused;
                    })
                },
                { Keys.F11,
                    new Input(Keys.F11, () => {
                        if ( screenSize.Size == new Point(1920,1080) )
                        {
                            graphics.IsFullScreen = !graphics.IsFullScreen;
                            graphics.ApplyChanges();
                        }
                    })
                },
                { Keys.F4,
                    new Input(Keys.F4, () => {
                        Instance.Exit();
                    })
                },
                { Keys.LeftShift, new Input(Keys.LeftShift) },
                { Keys.F3,
                    new Input(Keys.F3, () => {
                        UI.UI.showDebug = !UI.UI.showDebug;
                    })
                }
            });

            Map.placeholderSprite = Instance.Content.Load<Texture2D>("blank");
            Map.Initialize();
            Enemy.LoadContent(Instance.Content.Load<Texture2D>("Enemy/healthbar"));

            Entity.Entity.Initialize();
            Camera.Initialize();

            Skill.Initialize();

            Pickups.Pickup.Initialize();

            Ammo.LoadContent();
            Weapon.Initialize();
            Combat.Projectiles.Projectile.LoadContent();
            Combat.Projectiles.Projectile.Initialize();

            Particles.Particle.Initialize();

            /* For testing ; Remove later */
            Player.Instance.AddWeapon(new Combat.Weapons.Shotgun());
            Player.Instance.AddWeapon(new Combat.Weapons.Plasma_rifle());
            Player.Instance.AddWeapon(new Combat.Weapons.Hell());
            Player.Instance.AddWeapon(new Combat.Weapons.Barrett());
            Player.Instance.AddWeapon(new Combat.Weapons.AutoShotgun());
            Player.Instance.AddWeapon(new Combat.Weapons.Gatling_gun());

            Player.Instance.EquipWeapon(Weapon.Type.Auto_Shotgun);

            Cursor.LoadContent();
            UI.UI.LoadContent(Instance.Content.Load<SpriteFont>("Fonts/mainFont"));

            mainFont = Instance.Content.Load<SpriteFont>("Fonts/MainFont");
            Player.LoadContent(Instance.Content.Load<Texture2D>("player"));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            if(keyboardState.IsKeyDown(Keys.F4))
            {
                Exit();
            }

            Input.StaticUpdate();
            Cursor.Instance.Update();

            if (Universe.state == Universe.GameState.Main_menu)
            {
                UI.MainMenu.Update();
            }
            else
            {
                if (Universe.paused)
                {
                    UI.PauseMenu.Update();
                }
                else
                {
                    if (UpdateEvent != null)
                    {
                        UpdateEvent();
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            UI.UI.fps = 1f / (float)gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.Gray);

            if(Universe.state == Universe.GameState.Main_menu)
            {
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                UI.MainMenu.Draw();
                Cursor.Instance.Draw();
                spriteBatch.End();
                return;
            }
            Matrix renderPosition = Matrix.CreateTranslation(new Vector3(Camera.Instance.offset, 0));
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: renderPosition);
            if (BackgroundDrawEvent != null)
            {
                BackgroundDrawEvent();
            }
            Particles.Particle.StaticBackgroundDraw();
            if (MidgroundDrawEvent != null)
            {
                MidgroundDrawEvent();
            }
            if (ForegroundDrawEvent != null)
            {
                ForegroundDrawEvent();
            }
            Particles.Particle.StaticForegroundDraw();
            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            UI.UI.Draw();

            if(Universe.paused)
            {
                UI.PauseMenu.Draw();
            }
            Cursor.Instance.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
