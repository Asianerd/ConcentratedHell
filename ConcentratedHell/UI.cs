using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace ConcentratedHell
{
    class UI
    {
        public static UI Instance = null;
        SpriteBatch SpriteBatch;
        public static Texture2D BlankState;
        public static SpriteFont UIFont;

        #region Game attributes
        Bar HealthBar;
        Bar StaminaBar;
        #endregion

        public static void Initialize(SpriteBatch _spriteBatch, Texture2D _blankState, SpriteFont _uiFont)
        {
            if (Instance == null)
            {
                Instance = new UI();
                BlankState = _blankState;
                UIFont = _uiFont;

                Instance.SpriteBatch = _spriteBatch;
                Instance.__privateInit__();
            }
        }

        void __privateInit__()
        {
            HealthBar = new Bar(new Vector2(27, 27), new Vector2(527, 0), Color.Red, 35);
            StaminaBar = new Bar(new Vector2(27, 67), new Vector2(427, 0), Color.Blue, 35);
            Main.PlayerUpdateEvent += Update;
        }


        public void Draw()
        {
            SpriteBatch.Begin();
            HealthBar.Draw(SpriteBatch, (float)Player.Instance.Health.Percent());
            StaminaBar.Draw(SpriteBatch, (float)Player.Instance.Stamina.Percent());
            //SpriteBatch.DrawString(UIFont, $"{Main.FPS} :: {Projectile.Projectiles.Count}", Vector2.Zero, Color.White);
            string _ammoText = $"{Player.Instance.GunEquppedObject.AmmoUsage} / {Player.Instance.AmmoInventory[Player.Instance.GunEquppedObject.AmmoType]}";
            SpriteBatch.DrawString(
                UIFont,
                _ammoText,
                new Vector2(
                    Main.screenSize.X-(25*_ammoText.Length),
                    Main.screenSize.Y-50
                    ),
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
                );
            DrawPickedItems();
            SpriteBatch.End();
        }

        void Update()
        {
            PickedItem.AdvanceAge();
        }

        void DrawPickedItems()
        {
            foreach (var i in PickedItem.PickedItems.TakeLast(5).Select((value, i) => new {i, value}))
            {
                Vector2 _renderedPosition = new Vector2(1830, 900 - (i.i * 50));
                Vector2 _renderedTextPosition = _renderedPosition + new Vector2(25, -25);
                SpriteBatch.Draw(
                    i.value.Item.Sprite,
                    _renderedPosition,
                    null,
                    Color.White,
                    -1.5708f,
                    i.value.Item.Size / 2,
                    0.7f,
                    SpriteEffects.None,
                    0f
                    );

                SpriteBatch.DrawString(UIFont, $"x{i.value.Item.Amount}", _renderedTextPosition, Color.White);
            }
        }

        public void AppendPickedItems(Item _item)
        {
            PickedItem _previousItem = null;
            if(PickedItem.PickedItems.Count != 0)
            {
                _previousItem = PickedItem.PickedItems.Last();
            }

            if(_previousItem != null)
            {
                if (_previousItem.Item.Name == _item.Name)
                {
                    PickedItem.PickedItems.Last().Item.Amount += _item.Amount;
                    PickedItem.PickedItems.Last().Age = 0;
                    return;
                }
            }
            PickedItem.PickedItems.Add(new PickedItem(_item));
        }

        class PickedItem
        {
            public static List<PickedItem> PickedItems = new List<PickedItem>();

            public Item Item;
            public int Age;
            public static int AgeDeath = 240;

            public PickedItem(Item _item)
            {
                Item = _item;
            }

            public static void AdvanceAge()
            {
                List<PickedItem> _toBeRemoved = new List<PickedItem>();
                foreach(PickedItem x in PickedItems)
                {
                    x.Age++;
                    if (x.Age >= AgeDeath)
                    {
                        _toBeRemoved.Add(x);
                    }
                }
                foreach(PickedItem x in _toBeRemoved)
                {
                    PickedItems.Remove(x);
                }
            }
        }
    }
}
