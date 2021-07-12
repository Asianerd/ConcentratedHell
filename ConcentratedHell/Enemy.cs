﻿using System;
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
            AngleToPlayer = MathHelper.ToRadians(Universe.RANDOM.Next(0, 360));
        }

        void Update()
        {
            if((Vector2.Distance(Player.Instance.Position, Position) <= TriggerDistance) && !Angry)
            {
                Angry = true;
                Eyes.SpriteUsed = EnemyEyes.AngrySprite;
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
        }

        public void Destroy()
        {
            Main.UpdateEvent -= Update;
            Rendering.DrawEntities -= Draw;

            Enemies.Remove(this);
        }
    }
}