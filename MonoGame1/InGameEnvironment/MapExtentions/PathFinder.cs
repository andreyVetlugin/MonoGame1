using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<Point> GetCoordinatePath()
        {
            var directionPath = path.ToList();
            return Map.GetCoordinatesByDirections(CurrentMap.PlayerPosition, directionPath);

        }

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

            Point? lootCoord = null;
            int[,] stepsMap;
            var cellsOrderOfPath = new List<Point>() { CurrentMap.PlayerPosition};
            (stepsMap, lootCoord) = CreateStepsMapToClosestLootBox(worldCalculator);
            while (lootCoord != null)
            {
                CurrentMap.ChangePlayerPosition(lootCoord.Value);
                var swapList = cellsOrderOfPath;
                cellsOrderOfPath = TryToCreateCellsPathFromStepsMap(stepsMap, lootCoord);
                cellsOrderOfPath.RemoveAt(cellsOrderOfPath.Count-1);
                cellsOrderOfPath.AddRange(swapList);
                (stepsMap, lootCoord) = CreateStepsMapToClosestLootBox(worldCalculator);
            }


            //TryToCreateCellsPathFromStepsMap(stepsMap, lootCoord);  // изменить координату игрока и убрать loot box перед след поиском return path;

            if (cellsOrderOfPath != null && cellsOrderOfPath.Count > 2)
            {
                var previousCell = cellsOrderOfPath[0];
                for (int i = 1; i < cellsOrderOfPath.Count; i++)
                {
                    var currentCell = cellsOrderOfPath[i];
                    path.Push(Map.GetDiretionByPoints(currentCell, previousCell));
                    previousCell = currentCell;
                }
            }
        }

        private (int[,] map, Point? lootCoord) CreateStepsMapToClosestLootBox(WorldActivityCalculator worldCalculator)
        {
            //worldCalculator = new WorldActivityCalculator(startMap);

            CurrentMap = worldCalculator.currentMap;
            stepsMap = new int[CurrentMap.Size.X, CurrentMap.Size.Y];
            int stepCount = 1;
            stepsMap[CurrentMap.PlayerPosition.X, CurrentMap.PlayerPosition.Y] = 1;

            Point? findedLootBoxCoordinate = null;
            var lastStepVisitedCells = new List<Point>() { CurrentMap.PlayerPosition };

            while (lastStepVisitedCells.Count > 0 && findedLootBoxCoordinate == null)
            {
                worldCalculator.Tick();
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

                            if (dx * dy != 0 || !CurrentMap.Contains(currentCellCoord))
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
            }
            return (stepsMap, findedLootBoxCoordinate);
        }


        private List<Point> TryToCreateCellsPathFromStepsMap(int[,] stepsMap, Point? endCoord)
        {
            if (endCoord == null)
                return null;

            var path = new List<Point>();
            path.Add(endCoord.Value);
            var lastPoint = path[0];
            while (stepsMap[lastPoint.X, lastPoint.Y] != 1)// пока не стартовая позиция игрока
            {
                bool valueSetsOnThisStep = false;
                for (var x = -1; x <= 1; x++)
                    for (var y = -1; y <= 1; y++)
                    {
                        if (valueSetsOnThisStep || lastPoint.X + x >= stepsMap.GetLength(0) || lastPoint.X + x < 0
                            || lastPoint.Y + y >= stepsMap.GetLength(1) || lastPoint.Y + y < 0)
                            continue;
                        if (stepsMap[lastPoint.X + x, lastPoint.Y + y] == stepsMap[lastPoint.X, path.Last().Y] - 1)
                        {
                            path.Add(new Point(lastPoint.X + x, lastPoint.Y + y));// как выйти из while после этого??
                            lastPoint = new Point(lastPoint.X + x, lastPoint.Y + y);
                            valueSetsOnThisStep = true;
                        }
                    }
            }

            return path;
        }
    }
}

