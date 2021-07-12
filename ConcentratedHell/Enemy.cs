using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ConcentratedHell
{
    class Enemy
    {
        public static List<Enemy> Enemies = new List<Enemy>();

        public static Texture2D Sprite;
        public Vector2 Position;
        public Vector2 Size;
        public EnemyEyes Eyes;
        public Bar HealthBar;

        #region Game attributes
        public GameValue Health;
        #endregion

        public double AngleToPlayer;
        float Speed = 5;
        public float TriggerDistance = 500f;
        public bool Angry = false;

        public static void Initialize(Texture2D _sprite, Texture2D _passiveEyeSprite, Texture2D _angryEyeSprite)
        {
            Sprite = _sprite;
            EnemyEyes.Initialize(_passiveEyeSprite, _angryEyeSprite);
        }

        public Enemy(Vector2 _position)
        {
            Position = _position;
            Main.UpdateEvent += Update;
            Rendering.DrawEntities += Draw;

            Enemies.Add(this);
            Size = new Vector2(64, 64);
            Eyes = new EnemyEyes(this);
            HealthBar = new Bar(Color.Red, 10);
            AngleToPlayer = MathHelper.ToRadians(Universe.RANDOM.Next(0, 360));

            #region Game attributes
            Health = new GameValue("Health", 0, 50, 0.2, 100);
            #endregion
        }

        void Update()
        {
            if(Health.I <= 0)
            {
                Destroy();
            }
            if((Vector2.Distance(Player.Instance.Position, Position) <= TriggerDistance) && !Angry)
            {
                ToggleAnger();
            }
            if (Angry)
            {
                AngleToPlayer = Universe.ANGLETO(Position, Player.Instance.Position, false);
                Move();
            }
        }

        void Move()
        {
            Position += new Vector2((float)Math.Cos(AngleToPlayer) * Speed, (float)Math.Sin(AngleToPlayer) * Speed);
        }

        void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(
                Sprite,
                Position,
                null,
                Color.White,
                0f,
                Size / 2,
                Vector2.One,
                SpriteEffects.None,
                0f
                );
            Eyes.Draw(_spriteBatch, Position, Size);
            if (Health.Percent() != 1)
            {
                HealthBar.Draw(_spriteBatch, (float)Health.Percent(),
                    new Vector2(Position.X - (Size.X / 2), Position.Y + (Size.Y / 2) + 10),
                    new Vector2(Position.X + (Size.X / 2), 0)
                    );
            }
        }

        public void AffectHealth(int _value)
        {
            Health.AffectValue(_value);
        }

        public void ToggleAnger()
        {
            Angry = true;
            Eyes.SpriteUsed = EnemyEyes.AngrySprite;
        }

        public void Destroy()
        {
            Main.UpdateEvent -= Update;
            Rendering.DrawEntities -= Draw;

            Enemies.Remove(this);
        }
    }
}