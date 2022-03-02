using System;
using System.Collections.Generic;
using System.Text;

namespace ConcentratedHell
{
    public static class Universe
    {
        public static bool paused = false;
        public static float speedMultiplier = 1f;
        public static GameState state = GameState.Main_menu;

        public enum GameState
        {
            Playing,
            Main_menu,
        }
    }
}
