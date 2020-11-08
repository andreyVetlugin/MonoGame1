using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;

namespace MonoGame1.InGameEnvironment.MapExtentions
{
    public class PathManager
    {
        public int PossibleAwaitStepsCount = 0; //         
        //public Stack<Point> Path { get { if (Path == null) FindPathToClosestLootBox(); return Path; } };
        public Map CurrentMap { get; private set; }

        private Map startMap;
        private Stack<Direction> path;
        private int[,] stepsMap;
        private WorldActivityCalculator worldCalculator;

        public Direction GetNextStep() // Что делать, если достали последнюю точку .. а если больше нет лутбоксов ?
        {
            if (path == null)
                FindPathToClosestLootBox();

            if (path.Count == 0)
                return Direction.None;

            return path.Pop();
        }

        public PathManager(Map startMap)
        {
            this.startMap = new Map(startMap);
            path = new Stack<Direction>();
        }

        public void FindPathToClosestLootBox()
        {
            worldCalculator = new WorldActivityCalculator(startMap);
            CurrentMap = worldCalculator.currentMap;

            var (stepsMap, lootCoord) = CreateStepsMapToClosestLootBox();
            var cellsOrderOfPath = TryToCreateCellsPathFromStepsMap(stepsMap, lootCoord);  // изменить координату игрока и убрать loot box перед след поиском return path;
            var previousCell = cellsOrderOfPath.Pop();
            while (cellsOrderOfPath.Count > 0)
            {                
                path.Push(Map.GetDiretionByPoints(previousCell, cellsOrderOfPath.Peek()));
                previousCell = cellsOrderOfPath.Pop();   
            }
        }

        private (int[,] map, Point? lootCoord) CreateStepsMapToClosestLootBox()
        {
            worldCalculator = new WorldActivityCalculator(startMap);
            CurrentMap = worldCalculator.currentMap;
            stepsMap = new int[CurrentMap.Size.X, CurrentMap.Size.Y];
            int stepCount = 1;
            stepsMap[CurrentMap.PlayerPosition.X, CurrentMap.PlayerPosition.Y] = 1;

            Point? findedLootBoxCoordinate = null;
            var lastStepVisitedCells = new List<Point>() { CurrentMap.PlayerPosition };


            while (lastStepVisitedCells.Count > 0 && findedLootBoxCoordinate == null)
            {
                stepCount++;
                var currentStepVisitedCells = new List<Point>();
                for (int i = 0; i < lastStepVisitedCells.Count; i++)
                {
                    var visitedCellCoord = lastStepVisitedCells[i];
                    for (var dx = -1; dx < 2; dx++)
                    {
                        for (var dy = -1; dy < 2; dy++)
                        {
                            var currentCellCoord = new Point(visitedCellCoord.X + dx, visitedCellCoord.Y + dy);

                            if (dx * dy != 0 || !CurrentMap.IsCoordInsideMap(currentCellCoord))
                                continue;

                            var currentCellValue = CurrentMap.Cells[currentCellCoord.X, currentCellCoord.Y];

                            if (stepsMap[currentCellCoord.X, currentCellCoord.Y] == 0)
                            {
                                if (currentCellValue == MapCell.LootBox)
                                    findedLootBoxCoordinate = currentCellCoord;

                                if (Map.IsCellPassable(currentCellValue))
                                {
                                    currentStepVisitedCells.Add(currentCellCoord);
                                    stepsMap[currentCellCoord.X, currentCellCoord.Y] = stepCount;
                                }
                            }
                        }
                    }
                }

                lastStepVisitedCells = currentStepVisitedCells;
                worldCalculator.Tick();

            }
            return (stepsMap, findedLootBoxCoordinate);
        }

        private Stack<Point> TryToCreateCellsPathFromStepsMap(int[,] stepsMap, Point? endCoord)
        {
            if (endCoord == null)
                return null;

            var path = new Stack<Point>();
            path.Push(endCoord.Value);
            while (stepsMap[path.Peek().X, path.Peek().Y] != 1)// пока не стартовая позиция игрока
            {
                for (var x = -1; x <= 1; x++)
                    for (var y = -1; y <= 1; y++)
                    {
                        if (path.Peek().X + x >= stepsMap.GetLength(0) || path.Peek().X + x < 0
                            || path.Peek().Y + y >= stepsMap.GetLength(1) || path.Peek().Y + y < 0)
                            continue;
                        if (stepsMap[path.Peek().X + x, path.Peek().Y + y] == stepsMap[path.Peek().X, path.Peek().Y] - 1)
                            path.Push(new Point(path.Peek().X + x, path.Peek().Y + y));// как выйти из while после этого??
                    }
            }

            return path;
        }
    }
}

