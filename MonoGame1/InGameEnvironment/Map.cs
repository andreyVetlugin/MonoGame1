using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonoGame1.InGameEnvironment
{
    public enum MapCell
    {
        None = 0,
        Block = 1,
        LiveBlock = 2,
        Player = 3,
        LootBox = 4
    }

    public class Map
    {
        public Point Size { get; private set; }
        public MapCell[,] Cells;
        public Point PlayerPosition { get; private set; }

        //public void MovePlayer()
        //{ } /////!!!!!!!!!!!!!!!!!!!!!!!!!!!

        static public bool IsCellPassable(MapCell cell)
        {
            return cell == MapCell.LootBox || cell == MapCell.None;
        }

        public Map(Map anotherMap)
        {
            Size = anotherMap.Size;
            Cells = new MapCell[anotherMap.Size.X, anotherMap.Size.Y];
            Array.Copy(anotherMap.Cells, Cells, anotherMap.Cells.Length);
        }

        public bool IsCoordInsideMap(Point coord)
        {
            return coord.X >= 0 && coord.X < Size.X && coord.Y > 0 && coord.Y < Size.Y;
        }

        public Map(MapCell[,] anotherCells)
        {
            Size = new Point(anotherCells.GetLength(0), anotherCells.GetLength(1));
            Cells = new MapCell[Size.X, Size.Y];
            Array.Copy(anotherCells, Cells, anotherCells.Length);
        }

        public Map(Point anotherSize)
        {
            Size = anotherSize;
            Cells = new MapCell[Size.X, Size.Y];
        }

        public Map()
        {

        }

        private bool TryToFindPlayerPosition()
        {
            for (int i = 0; i < Size.X; i++)
                for (int j = 0; j < Size.Y; j++)
                    if (Cells[i, j] == MapCell.Player)
                    {
                        PlayerPosition = new Point(i, j);
                        return true;
                    }
            return false; // на карте нет игрока 
        }

        public void LoadMapFromFile(string pathToFileWithMap)
        {
            StreamReader reader = new StreamReader(pathToFileWithMap);
            if (TryToFindPlayerPosition() == false)
                SpawnPlayer();
        }

        public void LoadMapFromNothing()
        {
            Cells = new MapCell[5, 5] { { MapCell.Player, MapCell.None, MapCell.None, MapCell.None, MapCell.None },
                { MapCell.None, MapCell.None, MapCell.None, MapCell.None, MapCell.None },
            { MapCell.None, MapCell.None, MapCell.None, MapCell.None, MapCell.None },
            { MapCell.LiveBlock, MapCell.None, MapCell.None, MapCell.LootBox, MapCell.None },
            { MapCell.LiveBlock, MapCell.LiveBlock, MapCell.None, MapCell.None, MapCell.None }};
            Size = new Point(5, 5);
            if (TryToFindPlayerPosition() == false)
                SpawnPlayer();
        }

        public void SpawnPlayer()
        {
            Cells[0, 0] = MapCell.Player;
        }
    }
}
