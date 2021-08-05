using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Item
    {
        public static List<Item> Items = new List<Item>();
        public static Dictionary<ItemClass, Dictionary<object, Texture2D>> SpriteList;
        public static Dictionary<object, string> ItemNames;
        public static SpriteFont Font;

        #region Fields
        #region Entity-related
        public Texture2D Sprite;
        public Vector2 Position;
        public Vector2 Size = Vector2.One * 64;
        #endregion

        #region Attributes
        // Pickup attributes
        public float PickupRange = 50f;
        public float FollowRange = 100f;
        public float DisinterestRange = 500f;
        public bool Following = false;
        public float FollowSpeed = 20f;

        public string Name;
        public bool ShowName = false;
        public float ShowRange = 50f;

        float rotation; // Random rotation (for aesthetics)

        /*public delegate void ItemEvent();
        public event ItemEvent PickupEvent;*/
        // Class might be inherited like Projectile and Gun class 
        // OR might just have a switch statement that decides what to append to players inventory   <-- Implementing this one right now

        ItemClass Class; // Ammo, Loot, Equipment, Weapon
        public object Type; // Gun, Bow, Shotgun  or  Bullet, Arrow, Pellet
        public int Amount = 1; // Amount of items (like how 64 is a stack in minecraft)
        #endregion
        #endregion

        public static void Initialize(Dictionary<ItemClass, Dictionary<object, Texture2D>> _spriteList, Dictionary<object, string> _itemNames, SpriteFont _font)
        {
            SpriteList = _spriteList;
            ItemNames = _itemNames;
            Font = _font;
        }

        public Item(Vector2 _position, ItemClass _class, object _type, int _amount = 1)
        {
            Main.PlayerUpdateEvent += Update;
            Rendering.DrawItems += Draw;
            Class = _class;
            Type = _type;
            Sprite = SpriteList[Class][Type];
            Name = ItemNames[Type];

            Position = _position;
            Amount = _amount;
            rotation = MathHelper.ToRadians(Universe.RANDOM.Next(0, 360));
        }

        void Update()
        {
            float _distanceFromPlayer = Vector2.Distance(Position, Player.Instance.Position);
            if(Following)
            {
                Following = _distanceFromPlayer <= DisinterestRange;
            }
            else
            {
                Following = _distanceFromPlayer <= FollowRange;
            }

            if(Following)
            {
                float _radianToPlayer = Universe.ANGLETO(Position, Player.Instance.Position, false);
                Position += new Vector2(
                    (float)Math.Cos(_radianToPlayer) * FollowSpeed,
                    (float)Math.Sin(_radianToPlayer) * FollowSpeed
                    );
            }

            if(_distanceFromPlayer <= PickupRange)
            {
                Pickup();
            }

            ShowName = Vector2.Distance(Position, Player.Instance.MousePosition) <= ShowRange;

            rotation += 0.01f;
        }

        void Pickup()
        {
            switch(Class)
            {
                case ItemClass.Ammo:
                    switch(Type)
                    {
                        case Projectile.ProjectileType.Arrow:
                            Player.Instance.AmmoInventory[Projectile.ProjectileType.Arrow] += Amount;
                            break;
                        case Projectile.ProjectileType.Bullet:
                            Player.Instance.AmmoInventory[Projectile.ProjectileType.Bullet] += Amount;
                            break;
                        case Projectile.ProjectileType.Pellet:
                            Player.Instance.AmmoInventory[Projectile.ProjectileType.Pellet] += Amount;
                            break;
                        case Projectile.ProjectileType.SeekingMissile:
                            Player.Instance.AmmoInventory[Projectile.ProjectileType.SeekingMissile] += Amount;
                            break;
                        case Projectile.ProjectileType.Grenade:
                            Player.Instance.AmmoInventory[Projectile.ProjectileType.Grenade] += Amount;
                            break;
                        case Projectile.ProjectileType.GravTrap:
                            Player.Instance.AmmoInventory[Projectile.ProjectileType.GravTrap] += Amount;
                            break;
                        default:
                            break;
                    }
                    break;
                case ItemClass.Loot:
                    break;
                case ItemClass.Equipment:
                    break;
                case ItemClass.Weapon:
                    break;
                default:
                    break;
            }
            UI.Instance.AppendPickedItems(this);
            Destroy();
        }

        void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(
                Sprite,
                Position,
                null,
                Color.White,
                rotation,
                Size / 2,
                1f,
                SpriteEffects.None,
                0f
                );

            if (ShowName)
            {
                
                string _shownName = $"{ItemNames[Type]}{(Amount > 1 ? $"[{Amount}]" : "")}";
                _spriteBatch.DrawString(
                    Font,
                    _shownName,
                    Position,
                    Color.White,
                    0f,
                    new Vector2(((_shownName.Length - 1) * Rendering.FontWidth) / 2, -30),
                    1f,
                    SpriteEffects.None,
                    0f
                    );
            }
        }

        public enum ItemClass
        {
            Ammo,
            Loot,
            Equipment,
            Weapon
        }

        public void Destroy()
        {
            Main.PlayerUpdateEvent -= Update;
            Rendering.DrawItems -= Draw;

            Items.Remove(this);
        }
    }
}
