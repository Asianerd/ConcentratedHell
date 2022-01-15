using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ConcentratedHell.Map_related.Levels
{
    class Dust:Map
    {
        public Dust():base(new List<Tile>() {
            new Tile(new Rectangle(-64, -64, 384, 64)), // Up
            new Tile(new Rectangle(-64, -64, 64, 38)),  // Left
            new Tile(new Rectangle(256, -64, 64, 384)), // Right
            new Tile(new Rectangle(-64, 256, 384, 64)), // Down},
        },
            Vector2.Zero, new List<Vector2>() {
                new Vector2(0, 0)
            })
        {

        }
    }
}
