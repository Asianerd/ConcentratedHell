using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell
{
    class Gun
    {
        //public static Dictionary<GunType, Gun> PrefabGuns;
        public static Dictionary<GunType, Texture2D> GunSprites;
        public delegate void Weapon();
        public static event Weapon FiringEvent;
        public static event Weapon DestroyEvent;

        public Vector2 Position;
        public Vector2 Size = Vector2.One * 64;
        public float Distance = 50f;

        public GunType Type;
        public GameValue Cooldown;
        public Texture2D Sprite;

        public static void Initialize(Dictionary<GunType, Texture2D> _gunSprites)
        {
            GunSprites = _gunSprites;
        }

        public static object InstantiateGun(GunType _type)
        {
            if (DestroyEvent != null)
            {
                DestroyEvent();
            }
            switch(_type)
            {
                default:
                    return new Glock();
                case GunType.Bow:
                    return new Bow();
                case GunType.Glock:
                    return new Glock();
                case GunType.Shotgun:
                    return new Shotgun();
            }
        }

        public enum GunType
        {
            Glock,
            Bow,
            Shotgun
        }


        public void Update()
        {
            Cooldown.Regenerate();

            var _mInput = Mouse.GetState();
            Position = new Vector2((float)Math.Cos(Player.Instance.RadiansToMouse) * Distance, (float)Math.Sin(Player.Instance.RadiansToMouse) * Distance) + Player.Instance.Position;

            if (_mInput.LeftButton == ButtonState.Pressed && Cooldown.Percent() == 1)
            {
                Cooldown.AffectValue(-100);
                if (FiringEvent != null)
                {
                    FiringEvent();
                }
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            bool _inverted = Math.Abs(Player.Instance.RadiansToMouse) >= (Math.PI / 2);
            _spriteBatch.Draw(
                Sprite,
                Position,
                null,
                Color.White,
                Player.Instance.RadiansToMouse,
                Size / 2,
                1f,
                _inverted ? SpriteEffects.FlipVertically : SpriteEffects.None,
                0f
                );
        }

        public void Destroy()
        {
            Main.PlayerUpdateEvent -= Update;
            Rendering.DrawPlayer -= Draw;
            FiringEvent = null;
        }
    }
}
