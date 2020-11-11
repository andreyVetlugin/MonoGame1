using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

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

    public enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Right = 3,
        Left = 4,
    }

    public class Map
    {
        public Point Size { get; private set; }
        public MapCell[,] Cells;
        public Point PlayerPosition
        {
            get
            {
                if (!playerPosition.HasValue)
                {
                    SpawnPlayer();
                    TryToFindPlayerPosition();
                }
                return playerPosition.Value;
            }
        }
        private Point? playerPosition;

        static private Point GetMoveVectorByDirection(Direction moveDirection)
        {
            switch (moveDirection)
            {
                case Direction.Up:
                    return new Point(0, 1); ;
                case Direction.Down:
                    return new Point(0, -1);
                case Direction.Left:
                    return new Point(-1, 0);
                case Direction.Right:
                    return new Point(1, 0);
            }
            return new Point(0, 0);
        }

        static public Direction GetDiretionByPoints(Point startPoint, Point endPoint)
        {
            var resultPoint = endPoint - startPoint;
            if (resultPoint == new Point(0, 1))
                return Direction.Up;
            if (resultPoint == new Point(0, -1))
                return Direction.Down;
            if (resultPoint == new Point(1, 0))
                return Direction.Right;
            if (resultPoint == new Point(-1, 0))
                return Direction.Left;
            return Direction.None;
        }

        static public List<Point> GetCoordinatesByDirections(Point startPoint, List<Direction> directions)
        {
            Point currentPoint = startPoint;
            var path = new List<Point>();
            for (var i = 0; i < directions.Count; i++)
            {
                switch (directions[i])
                {
                    case Direction.Down:
                        path.Add(currentPoint + new Point(0, -1));
                        currentPoint = currentPoint + new Point(0, -1);
                        break;
                    case Direction.Up:
                        path.Add(currentPoint + new Point(0, 1));
                        currentPoint = currentPoint + new Point(0, 1);
                        break;
                    case Direction.Right:
                        path.Add(currentPoint + new Point(1, 0));
                        currentPoint = currentPoint + new Point(1, 0);
                        break;
                    case Direction.Left:
                        path.Add(currentPoint + new Point(-1, 0));
                        currentPoint = currentPoint + new Point(-1, 0);
                        break;
                }
            }
            return path;
        }

        static public bool IsCellPassable(MapCell cell)
        {
            return cell == MapCell.LootBox || cell == MapCell.None;
        }

        public Map(Map anotherMap)
        {
            Size = anotherMap.Size;
            Cells = new MapCell[anotherMap.Size.X, anotherMap.Size.Y];
            Array.Copy(anotherMap.Cells, Cells, anotherMap.Cells.Length);
            TryToFindPlayerPosition();
        }

        public bool ChangePlayerPosition(Point newPosition)
        {
            if (!Contains(newPosition))
                return false;
            Cells[PlayerPosition.X, PlayerPosition.Y] = MapCell.None;
            playerPosition = newPosition;
            Cells[PlayerPosition.X, PlayerPosition.Y] = MapCell.Player;
            return true;
        }

        public bool TryToMovePlayer(Direction moveDirection)
        {
            if (Cells[playerPosition.Value.X, playerPosition.Value.Y] == MapCell.Player)
                Cells[playerPosition.Value.X, playerPosition.Value.Y] = MapCell.None;
            playerPosition += GetMoveVectorByDirection(moveDirection);
            Cells[playerPosition.Value.X, playerPosition.Value.Y] = MapCell.Player;
            return true;
        }

        public bool Contains(Point coord)
        {
            return coord.X >= 0 && coord.X < Size.X && coord.Y >= 0 && coord.Y < Size.Y;
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

        public Map(string pathToMap, int lootBoxesCount)
        {
            if (pathToMap != null && pathToMap.Length != 0)
                LoadMapFromFile(pathToMap);
            SpawnPlayer();
            SpawnLootBoxes(lootBoxesCount);
        }

        public bool TryToFindPlayerPosition()
        {
            for (int i = 0; i < Size.X; i++)
                for (int j = 0; j < Size.Y; j++)
                    if (Cells[i, j] == MapCell.Player)
                    {
                        playerPosition = new Point(i, j);
                        return true;
                    }
            return false; // на карте нет игрока 
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

        public void LoadMapFromFile(string path)
        {
            List<string> lines = new List<string>();
            using (var file = new StreamReader(path))
            {
                string currentLine;
                while ((currentLine = file.ReadLine()) != null)
                {
                    lines.Add(currentLine);
                }
            }
            if (lines.Count == 0)
                return;// ???????? что делать 
            Size = new Point(lines[0].Length, lines.Count);
            Cells = new MapCell[Size.X, Size.Y];
            for (var i = 0; i < Size.X; i++)
            {
                for (var j = 0; j < Size.Y; j++)
                {
                    Cells[i, j] = ConvertSymbolToMapCell(lines[j][i]);
                }
            }
        }

        private MapCell ConvertSymbolToMapCell(char c)
        {
            switch (c)
            {
                case '-':
                    return MapCell.LiveBlock;
                case '+':
                    return MapCell.None;
                case ' ':
                    return MapCell.Block;
            }
            return MapCell.None;
        }

        private void SpawnLootBoxes(int count)
        {
            var emptyCells = FindEmptyCells();
            count = emptyCells.Count < count ? emptyCells.Count : count;
            for (var i = 0; i < count; i++)
            {
                if (emptyCells.Count > 0)
                {
                    emptyCells = FindEmptyCells();
                    var random = new Random().Next(emptyCells.Count);
                    Cells[emptyCells[random].X, emptyCells[random].Y] = MapCell.LootBox;
                }
            }

            //Cells[0, 2] = MapCell.LootBox;
        }

        private void SpawnPlayer()
        {
            var emptyCells = FindEmptyCells();
            if (emptyCells.Count > 0)
            {

                var random = new Random().Next(emptyCells.Count);
                playerPosition = emptyCells[random];
                Cells[playerPosition.Value.X, playerPosition.Value.Y] = MapCell.Player;
            }
            //Cells[2, 1] = MapCell.Player;
            //playerPosition = new Point(2, 1);
        }

        public List<Point> GetLootCoordinates()
        {
            List<Point> lootCoordinates = new List<Point>();
            for (var i = 0; i < Size.X; i++)
                for (var j = 0; j < Size.Y; j++)
                {
                    if (Cells[i, j] == MapCell.LootBox)
                        lootCoordinates.Add(new Point(i, j));
                }

            return lootCoordinates;
        }

        private List<Point> FindEmptyCells()
        {
            var potentialSpawnCells = new List<Point>();
            for (int i = 0; i < Size.X; i++)
                for (int j = 0; j < Size.Y; j++)
                    if (Cells[i, j] == MapCell.None)
                        potentialSpawnCells.Add(new Point(i, j));
            return potentialSpawnCells;
        }
    }
}
