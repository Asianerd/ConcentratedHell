using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Pickups
{
    class Pickup
    {
        public static List<Pickup> pickups;

        public static void Initialize()
        {
            pickups = new List<Pickup>();

            Main.UpdateEvent += StaticUpdate;
            Main.MidgroundDrawEvent += StaticDraw;
        }

        public static void StaticUpdate()
        {
            foreach(Pickup x in pickups)
            {
                x.Update();
            }
            pickups.RemoveAll(n => !n.alive);
        }

        public static void StaticDraw()
        {
            foreach(Pickup x in pickups)
            {
                x.Draw();
            }
        }

        #region Inherited
        PickupType pickupType;
        public int amount;
        bool alive = true;

        Vector2 position;

        float detectRange = 100f;
        float pickupRange = 20f;
        public bool detected = false;
        GameValue progress;

        public Texture2D sprite;
        Vector2 spriteOrigin;
        public float renderedScale = 1f;
        public float rotation = 0f;

        public Pickup(PickupType _type, int _amount, Vector2 _position, Texture2D _sprite)
        {
            pickupType = _type;
            amount = _amount;

            position = _position;
            progress = new GameValue(0, 15, 1, 0);

            sprite = _sprite;
            spriteOrigin = sprite.Bounds.Center.ToVector2();

            pickups.Add(this);
        }

        public virtual void Update()
        {
            if(!alive)
            {
                return;
            }

            float distance = Vector2.Distance(Player.Instance.rect.Center.ToVector2(), position);

            if (!detected)
            {
                detected = distance <= detectRange;
            }
            else
            {
                progress.Regenerate();
                position = Vector2.Lerp(position, Player.Instance.rect.Center.ToVector2(), (float)progress.Percent());

                if(distance <= pickupRange)
                {
                    AddToPlayer();
                    alive = false;
                }
            }
        }

        public virtual void AddToPlayer()
        {

        }

        public virtual void Draw()
        {
            Main.spriteBatch.Draw(sprite, position, null, Color.White, rotation, spriteOrigin, renderedScale, SpriteEffects.None, 0f);
        }
        #endregion

        public enum PickupType
        {
            Ammo,
            Weapon,
            Health,
        }
    }
}
