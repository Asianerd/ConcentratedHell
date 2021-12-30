using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell
{
    class Input
    {
        static List<Input> inputs;
        public static List<Keys> playerInputKeys;
        public static Dictionary<Keys, Direction> keyDirections;
        public static Dictionary<Direction, Direction> oppositeDirections;
        public static Dictionary<Direction, Vector2> directionalVectors;

        public static void Initialize(List<Input> _inputs)
        {
            inputs = _inputs;

            playerInputKeys = new List<Keys>()
            {
                Keys.W,
                Keys.A,
                Keys.S,
                Keys.D
            };

            keyDirections = new Dictionary<Keys, Direction>()
            {
                { Keys.W, Direction.Up },
                { Keys.S, Direction.Down },
                { Keys.A, Direction.Left },
                { Keys.D, Direction.Right },
            };

            oppositeDirections = new Dictionary<Direction, Direction>()
            {
                { Direction.Up, Direction.Down },
                { Direction.Down, Direction.Up },
                { Direction.Left, Direction.Right },
                { Direction.Right, Direction.Left }
            };

            directionalVectors = new Dictionary<Direction, Vector2>()
            {
                { Direction.Up, new Vector2(0, -1) },
                { Direction.Down, new Vector2(0, 1) },
                { Direction.Left, new Vector2(-1, 0) },
                { Direction.Right, new Vector2(1, 0) }
            };

            Main.UpdateEvent += StaticUpdate;
        }

        public static void StaticUpdate()
        {
            foreach(Input x in inputs)
            {
                x.Update();
            }
        }

        Keys key;
        Action action;
        bool hasAction = false;

        public bool isPressed = false;
        public bool wasPressed = false;
        public bool active = false;

        public Input(Keys _key)
        {
            key = _key;
        }

        public Input(Keys _key, Action _action)
        {
            key = _key;

            action = _action;
            hasAction = true;
        }

        public void Update()
        {
            wasPressed = isPressed;
            isPressed = Main.keyboardState.IsKeyDown(key);

            active = isPressed && !wasPressed;

            if(active && hasAction)
            {
                action.Invoke();
            }
        }

        public enum Direction
        {
            Up,
            Down,
            Right,
            Left
        }
    }
}
