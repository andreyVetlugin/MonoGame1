using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;

namespace MonoGame1.InGameEnvironment.MapExtentions
{
    public class PathFinder
    {
        private Map startMap;
        public int PossibleAwaitStepsCount = 0; // 
        private Map currentMap;
        private int[,] stepsMap;
        private WorldActivityCalculator worldCalculator;

        public PathFinder(Map startMap)
        {
            this.startMap = new Map(startMap);
        }

        public List<Point> FindPathToClosestLootBox()
        {
            worldCalculator = new WorldActivityCalculator(startMap);
            currentMap = worldCalculator.currentMap;
            //var stepsMap = new Stack<int>[currentMap.Size.X, currentMap.Size.Y];
            //stepsMap[currentMap.PlayerPosition.X, currentMap.PlayerPosition.Y] = new Stack<int>();
            //stepsMap[currentMap.PlayerPosition.X, currentMap.PlayerPosition.Y].Push(0);
            var (stepsMap,lootCoord) = CreateStepsMapToClosestLootBox();


            return new List<Point>();

        }      

        private (int[,] map, Point? lootCoord) CreateStepsMapToClosestLootBox()
        {            
            currentMap = new Map(startMap);
            stepsMap = new int[currentMap.Size.X, currentMap.Size.Y];
            int stepCount = 1;
            stepsMap[currentMap.PlayerPosition.X, currentMap.PlayerPosition.Y] = 1;

            Point? findedLootBoxCoordinate = null;
            var lastStepVisitedCells = new List<Point>() { currentMap.PlayerPosition };


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

                            if (dx * dy != 0 || !currentMap.IsCoordInsideMap(currentCellCoord))
                                continue;
                            
                            var currentCellValue = currentMap.Cells[currentCellCoord.X, currentCellCoord.Y];

                            if (stepsMap[currentCellCoord.X, currentCellCoord.Y] == 0)
                            {
                                if (currentCellValue == MapCell.LootBox)
                                    findedLootBoxCoordinate = currentCellCoord;

                                if (Map.IsCellPassable(currentCellValue))
                                {
                                    currentStepVisitedCells.Add(visitedCellCoord);
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

        //private (bool, List<Point> path) TryToCreatePathFromStepsMap(int[,] stepsMap, Point? endCoord)
        //{
        //    if (endCoord == null)
        //        return (false, null);

        //    var path = new List<Point>();
        //    var currentStepCoord = endCoord.Value;
        //    //while(stepsMap[currentStepCoord.X,stepsMap[]])
        //}
        //return new List<Point>();
    }
}

