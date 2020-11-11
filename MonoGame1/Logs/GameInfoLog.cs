using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame1.Logs
{
    public class GameInfoLog
    {
        public Point SpawnCoordinate { get; private set; }
        public List<Point> LootBoxCoordinates { get; private set; }
        public List<Point> Path { get; private set; }

        public GameInfoLog(List<Point> path, List<Point> lootBoxCoordinates, Point playerCoordinate)
        {
            Path = path;
            SpawnCoordinate = playerCoordinate;
            LootBoxCoordinates = lootBoxCoordinates;
        }
    }
}
