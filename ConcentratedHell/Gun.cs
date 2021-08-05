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

        public Vector2 Position = Vector2.One * 2048;
        public Vector2 Size = Vector2.One * 64;
        public float Distance = 50f;

        public GunType Type;
        public GameValue Cooldown;
        public Texture2D Sprite;

        public Projectile.ProjectileType AmmoType;
        public int AmmoUsage = 1;

        public static void Initialize(Dictionary<GunType, Texture2D> _gunSprites)
        {
            GunSprites = _gunSprites;
        }

        public static object InstantiateGun(GunType _type, out Gun _gunObject)
        {
            if (DestroyEvent != null)
            {
                DestroyEvent();
            }
            object final;
            switch (_type)
            {
                default:
                    final = new Glock();
                    break;
                case GunType.Bow:
                    final = new Bow();
                    break;
                case GunType.Glock:
                    final = new Glock();
                    break;
                case GunType.Shotgun:
                    final = new Shotgun();
                    break;
                case GunType.PlasmaPrism:
                    final = new PlasmaPrism();
                    break;
                case GunType.MissileLauncher:
                    final = new MissileLauncher();
                    break;
                case GunType.GrenadeLauncher:
                    final = new GrenadeLauncher();
                    break;
                case GunType.Trapper:
                    final = new Trapper();
                    break;
            }
            _gunObject = (Gun)final;
            return final;
        }

        public enum GunType
        {
            Glock,
            Bow,
            Shotgun,
            PlasmaPrism,
            MissileLauncher,
            GrenadeLauncher,
            Trapper
        }


        public void Update()
        {
            Cooldown.Regenerate();

            var _mInput = Mouse.GetState();
            Position = new Vector2((float)Math.Cos(Player.Instance.RadiansToMouse) * Distance, (float)Math.Sin(Player.Instance.RadiansToMouse) * Distance) + Player.Instance.Position;

            if ((_mInput.LeftButton == ButtonState.Pressed) && 
                (Cooldown.Percent() == 1) && 
                (Player.Instance.AmmoInventory[AmmoType] >= AmmoUsage) &&
                (Player.Instance.CanFire))
            {
                Player.Instance.AmmoInventory[AmmoType] -= AmmoUsage;
                Cooldown.AffectValue(0f);
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
