using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace ConcentratedHell
{
    class UI
    {
        public static UI Instance = null;
        public static Texture2D BlankState;
        public static SpriteFont UIFont;

        #region Game attributes
        Bar HealthBar;
        Bar StaminaBar;
        Bar ComboBar;
        #endregion

        public static void Initialize(Texture2D _blankState, SpriteFont _uiFont, Texture2D _radialWheelSprite, Texture2D _selectionSprite)
        {
            if (Instance == null)
            {
                Instance = new UI();
                BlankState = _blankState;
                UIFont = _uiFont;

                Instance.__privateInit__();
            }
            RadialWeaponWheel.Initialize(_radialWheelSprite, _selectionSprite);
        }

        void __privateInit__()
        {
            HealthBar = new Bar(new Vector2(27, 27), new Vector2(527, 0), Color.Red, 35);
            StaminaBar = new Bar(new Vector2(27, 67), new Vector2(427, 0), Color.Blue, 35);
            ComboBar = new Bar(new Vector2(27, 107), new Vector2(427, 0), Color.Yellow, 35);
            Main.PlayerUpdateEvent += Update;
        }


        public void Draw(SpriteBatch _spritebatch)
        {
            HealthBar.Draw(_spritebatch, (float)Player.Instance.Health.Percent());
            StaminaBar.Draw(_spritebatch, (float)Player.Instance.Stamina.Percent());
            ComboBar.Draw(_spritebatch, (float)Player.Instance.Combo.Percent());
            //SpriteBatch.DrawString(UIFont, $"{Main.FPS} :: {Projectile.Projectiles.Count}", Vector2.Zero, Color.White);
            string _ammoText = $"{Player.Instance.GunEquppedObject.AmmoUsage} / {Player.Instance.AmmoInventory[Player.Instance.GunEquppedObject.AmmoType]}";
            _spritebatch.DrawString(
                UIFont,
                _ammoText,
                new Vector2(
                    Main.screenSize.X - (Rendering.FontWidth * _ammoText.Length) - 100,
                    Main.screenSize.Y - 50
                    ),
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
                );
            _spritebatch.DrawString(
                UIFont,
                Player.Instance.Level.ToString(),
                new Vector2(427, 107),
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
                );
            _spritebatch.DrawString(
                UIFont,
                $"FPS : {Main.FPS}",
                new Vector2(64, 157),
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
                );
            DrawPickedItems(_spritebatch);
            RadialWeaponWheel.Instance.Draw(_spritebatch);
        }

        void Update()
        {
            PickedItem.AdvanceAge();
        }

        void DrawPickedItems(SpriteBatch _spritebatch)
        {
            foreach (var i in PickedItem.PickedItems.Select((value, i) => new {i, value}))
            {
                Vector2 _renderedPosition = new Vector2(1720, 900 - (i.i * 50));
                Vector2 _renderedTextPosition = _renderedPosition + new Vector2(25, -25);
                int _dithered = (int)(i.value.Age / 128);
                int _c = (255 * (_dithered == 1 ? 0 : 1)) + (2 * _dithered * (255 - i.value.Age));
                //int _c = 255 - i.value.Age;
                Color _renderedColor = new Color(_c, _c, _c, _c);
                _spritebatch.Draw(
                    i.value.Item.Sprite,
                    _renderedPosition,
                    null,
                    _renderedColor,
                    -1.5708f,
                    i.value.Item.Size / 2,
                    0.7f,
                    SpriteEffects.None,
                    0f
                    );

                _spritebatch.DrawString(UIFont, $"x{i.value.Item.Amount}", _renderedTextPosition, _renderedColor);
            }
        }

        public void AppendPickedItems(Item _item)
        {
            var _match = PickedItem.PickedItems.FirstOrDefault(n => n.Item.Name == _item.Name);
            if (_match != null)
            {
                _match.Item.Amount += _item.Amount;
                _match.Age = 0;
            }
            else
            {
                PickedItem.PickedItems.Add(new PickedItem(_item));
            }
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

        class RadialWeaponWheel
        {
            public static RadialWeaponWheel Instance;

            public static void Initialize(Texture2D _wheelSprite, Texture2D _selectionSprite)
            {
                if(Instance == null)
                {
                    Instance = new RadialWeaponWheel(_wheelSprite, _selectionSprite);
                    WeaponWheelWeapon.Initialize();
                }
            }

            public RadialWeaponWheel(Texture2D _wheelSprite, Texture2D _selectionSprite)
            {
                Main.PlayerUpdateEvent += Update;

                WheelSprite = _wheelSprite;
                SelectionSprite = _selectionSprite;
                SpriteSize = new Vector2(800, 800);

                Position = Main.screenSize / 2;
                Value = new GameValue("Wheel Value", 0, 60, 5, 0);
            }

            public Texture2D WheelSprite;
            public Texture2D SelectionSprite;
            public Vector2 SpriteSize;

            public Vector2 Position;
            public GameValue Value;


            public void Update()
            {
                var _kInput = Keyboard.GetState();
                
                if(_kInput.IsKeyDown(Keys.Tab))
                {
                    Value.Regenerate();
                }
                else
                {
                    Value.AffectValue(-5d);
                }
            }

            public void Draw(SpriteBatch _spriteBatch)
            {
                if (Value.Percent() != 0)
                {
                    int _c = (int)(255 * Value.Percent());
                    Color _renderedColor = new Color(_c, _c, _c, _c);
                    _spriteBatch.Draw(WheelSprite, Position, null, _renderedColor, 0f, SpriteSize / 2, (float)Value.Percent(), SpriteEffects.None, 0f);
                    _spriteBatch.Draw(SelectionSprite, Position, null, _renderedColor, Player.Instance.RadiansToMouse, SpriteSize / 2, (float)Value.Percent(), SpriteEffects.None, 0f);
                    WeaponWheelWeapon.Draw(_spriteBatch, _renderedColor, (float)Value.Percent());
                }
            }

            class WeaponWheelWeapon
            {
                public static List<Gun.GunType> AvailableGuns = new List<Gun.GunType>();
                public static Gun.GunType WantedGun;
                public static void Initialize()
                {
                    /*foreach(Gun.GunType x in Enum.GetValues(typeof(Gun.GunType)))
                    {
                        AvailableGuns.Add(x);
                    }*/
                    for (int i = 0; i < 7; i++)
                    {
                        AvailableGuns.Add((Gun.GunType)i);
                    }
                }

                public static void Draw(SpriteBatch _spriteBatch, Color _color, float _scale)
                {
                    float _radianIncrement = MathHelper.ToRadians(360 / AvailableGuns.Count);
                    Gun.GunType _gunSelected = Player.Instance.GunEquppedObject.Type;
                    foreach(var item in AvailableGuns.Select((value, i) => new { i, value}))
                    {
                        bool selected = false;

                        float _radians = (item.i * _radianIncrement) + _radianIncrement / 2;
                        float _max = _radians + (_radianIncrement / 2);
                        float _min = _radians - (_radianIncrement / 2);
                        if ((FullRadian(Player.Instance.RadiansToMouse) > _min) &&
                            (FullRadian(Player.Instance.RadiansToMouse) < _max))
                        {
                            selected = true;
                            _gunSelected = item.value;
                        }
                        Vector2 _renderedPosition = new Vector2(MathF.Cos(_radians) * (326 * _scale), MathF.Sin(_radians) * (326 * _scale)) + (Main.screenSize / 2);
                        _spriteBatch.Draw(Gun.GunSprites[item.value], _renderedPosition, null, _color, 0f, Vector2.One * 32, _scale * (selected ? 1.5f : 1), SpriteEffects.None, 0f);
                    }
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        if (_gunSelected != Player.Instance.GunEquppedObject.Type)
                        {
                            Player.Instance.GunEquipped = Gun.InstantiateGun(_gunSelected, out Player.Instance.GunEquppedObject);
                        }
                    }
                }

                public static float FullRadian(float _radian)
                {
                    if(_radian < 0)
                    {
                        return (float)((Math.PI - MathF.Abs(_radian)) + Math.PI);
                    }
                    else
                    {
                        return _radian;
                    }
                }
            }
        }
    }
}
