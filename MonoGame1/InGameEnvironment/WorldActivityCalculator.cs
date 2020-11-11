using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;
using System.Text;

namespace MonoGame1.InGameEnvironment
{
    public class WorldActivityCalculator
    {
        public Map currentMap { get; private set; }

        public WorldActivityCalculator(Map currentMap)
        {
            this.currentMap = new Map(currentMap);
        }

        public void Tick()
        {
            var mapBeforeTick = new MapCell[currentMap.Size.X, currentMap.Size.Y];
            Array.Copy(currentMap.Cells, mapBeforeTick, mapBeforeTick.Length);
            for (int x = 0; x < currentMap.Size.X; x++)
            {
                for (int y = 0; y < currentMap.Size.Y; y++)
                {
                    int livingCellsAroundCurrentCell = 0;
                    for (int i = -1; i < 2; i++)
                    {
                        if (x + i >= currentMap.Size.X || x + i < 0)
                            continue;

                        for (int j = -1; j < 2; j++)
                        {
                            if (y + j >= currentMap.Size.Y || y + j < 0 || (i == 0 && j == 0))
                                continue;
                            if (mapBeforeTick[x + i, j + y] == MapCell.LiveBlock)
                                livingCellsAroundCurrentCell++;
                        }
                    }

                    bool currentCellIsAlive = mapBeforeTick[x, y] == MapCell.LiveBlock ? true : false;

                    if (currentCellIsAlive && (livingCellsAroundCurrentCell == 2 || livingCellsAroundCurrentCell == 3))
                        currentMap.Cells[x, y] = MapCell.LiveBlock;
                    else if (!currentCellIsAlive && livingCellsAroundCurrentCell == 3 && (mapBeforeTick[x, y] == MapCell.None || mapBeforeTick[x, y] == MapCell.Player))
                        currentMap.Cells[x, y] = MapCell.LiveBlock;
                    else if (mapBeforeTick[x, y] == MapCell.None || mapBeforeTick[x, y] == MapCell.LiveBlock)
                        currentMap.Cells[x, y] = MapCell.None;
                }
            }
            //currentMap.Cells = mapAfterTick;
        }
    }
}