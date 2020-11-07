using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
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


        public PathFinder(Map currentMap)
        {
            this.startMap = new Map(startMap);
        }

        public List<Point> FindPathToClosestLootBox()
        {
            //var stepsMap = new Stack<int>[currentMap.Size.X, currentMap.Size.Y];
            //stepsMap[currentMap.PlayerPosition.X, currentMap.PlayerPosition.Y] = new Stack<int>();
            //stepsMap[currentMap.PlayerPosition.X, currentMap.PlayerPosition.Y].Push(0);
            var test = CreateStepsMapToClosestLootBox();

            return new List<Point>();


        }

        //private 


        private (int[,] map, Point? lootCoord) CreateStepsMapToClosestLootBox()
        {
            var worldCalculator = new WorldActivityCalculator(startMap);
            currentMap = new Map(startMap);
            stepsMap = new int[currentMap.Size.X, currentMap.Size.Y];
            int currentStepCount = 1;
            stepsMap[currentMap.PlayerPosition.X, currentMap.PlayerPosition.Y] = 1;

            Point? findedLootBoxCoordinate = null;
            var lastStepVisitedCells = new List<Point>() { currentMap.PlayerPosition };            
            

            while (lastStepVisitedCells.Count > 0 && findedLootBoxCoordinate == null)
            {
                currentStepCount++;
                var currentStepVisitedCells = new List<Point>();
                for (int i = 0; i < lastStepVisitedCells.Count; i++)
                {
                    var visiteCellCoord = lastStepVisitedCells[i];
                    // цикл по координатам с continue, если произведение == 0

                    if (visiteCellCoord.X > 0)
                    {
                        if (stepsMap[visiteCellCoord.X - 1, visiteCellCoord.Y] == 0 && currentMap.Cells[visiteCellCoord.X - 1, visiteCellCoord.Y] == MapCell.LootBox)
                        {
                            findedLootBoxCoordinate = visiteCellCoord;
                            currentStepVisitedCells.Add(visiteCellCoord);
                            stepsMap[visiteCellCoord.X - 1, visiteCellCoord.Y] = currentStepCount;
                        }
                        if (stepsMap[visiteCellCoord.X - 1, visiteCellCoord.Y] == 0 && currentMap.Cells[visiteCellCoord.X - 1, visiteCellCoord.Y] == MapCell.None)
                        {
                            currentStepVisitedCells.Add(new Point(visiteCellCoord.X - 1, visiteCellCoord.Y));
                            stepsMap[visiteCellCoord.X - 1, visiteCellCoord.Y] = currentStepCount;
                        }
                    }


                    if (visiteCellCoord.X < currentMap.Size.X - 1)
                    {
                        if (stepsMap[visiteCellCoord.X + 1, visiteCellCoord.Y] == 0 && currentMap.Cells[visiteCellCoord.X + 1, visiteCellCoord.Y] == MapCell.LootBox)
                        {
                            findedLootBoxCoordinate = visiteCellCoord;
                            currentStepVisitedCells.Add(visiteCellCoord);
                            stepsMap[visiteCellCoord.X + 1, visiteCellCoord.Y] = currentStepCount;
                        }
                        if (stepsMap[visiteCellCoord.X + 1, visiteCellCoord.Y] == 0 && currentMap.Cells[visiteCellCoord.X + 1, visiteCellCoord.Y] == MapCell.None)
                        {
                            stepsMap[visiteCellCoord.X + 1, visiteCellCoord.Y] = currentStepCount;
                            currentStepVisitedCells.Add(new Point(visiteCellCoord.X + 1, visiteCellCoord.Y));
                        }
                    }


                    if (visiteCellCoord.Y < currentMap.Size.Y - 1)
                    {
                        if (stepsMap[visiteCellCoord.X, visiteCellCoord.Y + 1] == 0 && currentMap.Cells[visiteCellCoord.X, visiteCellCoord.Y + 1] == MapCell.LootBox)
                        {
                            findedLootBoxCoordinate = visiteCellCoord;
                            currentStepVisitedCells.Add(visiteCellCoord);
                            stepsMap[visiteCellCoord.X, visiteCellCoord.Y + 1] = currentStepCount;
                        }
                        if (stepsMap[visiteCellCoord.X, visiteCellCoord.Y + 1] == 0 && currentMap.Cells[visiteCellCoord.X, visiteCellCoord.Y + 1] == MapCell.None)
                        {
                            currentStepVisitedCells.Add(new Point(visiteCellCoord.X, visiteCellCoord.Y + 1));
                            stepsMap[visiteCellCoord.X, visiteCellCoord.Y + 1] = currentStepCount;
                        }
                    }

                    if (visiteCellCoord.Y > 0)
                    {
                        if (stepsMap[visiteCellCoord.X, visiteCellCoord.Y - 1] == 0 && currentMap.Cells[visiteCellCoord.X, visiteCellCoord.Y - 1] == MapCell.LootBox)
                        {
                            findedLootBoxCoordinate = visiteCellCoord;
                            currentStepVisitedCells.Add(visiteCellCoord);
                            stepsMap[visiteCellCoord.X, visiteCellCoord.Y - 1] = currentStepCount;
                        }
                        if (stepsMap[visiteCellCoord.X, visiteCellCoord.Y - 1] == 0 && currentMap.Cells[visiteCellCoord.X, visiteCellCoord.Y - 1] == MapCell.None)
                        {
                            currentStepVisitedCells.Add(new Point(visiteCellCoord.X, visiteCellCoord.Y - 1));
                            stepsMap[visiteCellCoord.X, visiteCellCoord.Y - 1] = currentStepCount;
                        }
                    }

                }
                lastStepVisitedCells = currentStepVisitedCells;
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

