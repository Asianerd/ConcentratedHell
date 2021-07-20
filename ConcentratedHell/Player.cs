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

        public static void Initialize(Texture2D _bodySprite, List<Texture2D> _eyeSprites)
        {
            Instance = new Player(Main.screenSize / 2);
            Instance.__privateInit__(_bodySprite, _eyeSprites);
        }

        void __privateInit__(Texture2D _bodySprite, List<Texture2D> _eyeSprite)
        {
            BodySprite = _bodySprite;

            Size = new Vector2(64, 64); // size in pixels
            Rendering.DrawPlayer += Draw;
            Main.PlayerUpdateEvent += Update;

            Eyes = new PlayerEyes(_eyeSprite);

            #region Game attributes
            GunEquipped = Gun.InstantiateGun(Gun.GunType.Shotgun, out GunEquppedObject);
            Health = new GameValue("Health", 0, 100, 5);
            Stamina = new GameValue("Stamina", 0, 500, 2);
            foreach(Projectile.ProjectileType x in Enum.GetValues(typeof(Projectile.ProjectileType)))
            {
                AmmoInventory[x] = 5;
            }
            #endregion
        }
        #endregion


        #region Fields
        #region Physics
        public Vector2 Position;
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
        public float RadiansToMouse;
        #endregion

        #region Game attributes
        public object GunEquipped;
        public Gun GunEquppedObject;
        public GameValue Health;
        public GameValue Stamina;
        public bool TimeStopped = false;
        public Dictionary<Projectile.ProjectileType, int> AmmoInventory = new Dictionary<Projectile.ProjectileType, int>();
        #endregion
        #endregion

        Player(Vector2 position)
        {
            Position = position;
        }
        
        public void Update()
        {
            MousePosition = Mouse.GetState().Position.ToVector2();
            RadiansToMouse = Universe.ANGLETO(Position, MousePosition, false);
            Interact();
            Move();
            Eyes.Update();
        }

        public void Move()
        {
            var _kInput = Keyboard.GetState();
            bool _sprinting = _kInput.IsKeyDown(Keys.LeftShift);

            float _speed = _sprinting && (Stamina.I > 0) ? SprintMultiplier * Speed : Speed;

            #region Weapon changing
            if(_kInput.IsKeyDown(Keys.D1))
            {
                GunEquipped = Gun.InstantiateGun(Gun.GunType.Glock, out GunEquppedObject);
            }
            if (_kInput.IsKeyDown(Keys.D2))
            {
                GunEquipped = Gun.InstantiateGun(Gun.GunType.Bow, out GunEquppedObject);
            }
            if (_kInput.IsKeyDown(Keys.D3))
            {
                GunEquipped = Gun.InstantiateGun(Gun.GunType.Shotgun, out GunEquppedObject);
            }
            if (_kInput.IsKeyDown(Keys.D4))
            {
                GunEquipped = Gun.InstantiateGun(Gun.GunType.PlasmaPrism, out GunEquppedObject);
            }
            if (_kInput.IsKeyDown(Keys.D5))
            {
                GunEquipped = Gun.InstantiateGun(Gun.GunType.MissileLauncher, out GunEquppedObject);
            }
            if (_kInput.IsKeyDown(Keys.D6))
            {
                GunEquipped = Gun.InstantiateGun(Gun.GunType.GrenadeLauncher, out GunEquppedObject);
            }
            if (_kInput.IsKeyDown(Keys.D7))
            {
                GunEquipped = Gun.InstantiateGun(Gun.GunType.Trapper, out GunEquppedObject);
            }
            #endregion



            #region Movement
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
            #endregion

            #region Regeneration
            if (_sprinting && ((_startingPosition-_endingPosition) != Vector2.Zero))
            {
                Stamina.AffectValue(-7d);
            }
            else
            {
                Stamina.Regenerate();
            }

            Health.Regenerate();
            #endregion
        }

        public void Interact()
        {
            var _mInput = Mouse.GetState();
            var _kInput = Keyboard.GetState();

            if(_mInput.RightButton == ButtonState.Pressed)
            {
                //var x = new Enemy(new Vector2(Universe.RANDOM.Next(0, (int)Main.screenSize.X), Universe.RANDOM.Next(0, (int)Main.screenSize.Y)));
                var x = new Enemy(MousePosition);
            }

            TimeStopped = _kInput.IsKeyDown(Keys.Space);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Vector2 _renderedPosition = new Vector2((float)(Position.X * Universe.SCALE), (float)(Position.Y * Universe.SCALE));
            spriteBatch.Draw(
                BodySprite,
                Position,
                null,
                Color.White,
                0f,
                Size/2,
                Size/Rendering.SizingScale,
                SpriteEffects.None,
                0f
                );
            Eyes.Draw(spriteBatch, Position, Size);
        }
    }
}
