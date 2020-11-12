using Microsoft.Xna.Framework;
using MonoGame1.RenderTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame1.Gui
{
    public class MenuBox
    {
        public List<Button> Buttons { get; private set; }
        public Point Coordinate { get; private set; }
        public int distanceBetweenButtons { get; private set; }
        public Point Size;

        public MenuBox(/*Point coordinate ,*/ List<Button> buttons, int distance)
        {
            //Coordinate = coordinate;
            distanceBetweenButtons = distance;
            Buttons = buttons;
            CalculateSize();
        }

        private void CalculateSize()
        {
            Point maxSize = new Point(0, 0);

            foreach (var button in Buttons)
            {
                if (button.BoundSize.X > maxSize.X)
                    maxSize.X = (int)button.BoundSize.X;
                maxSize.Y += button.BoundSize.Y;
                maxSize.Y += distanceBetweenButtons;
            }
            Size = maxSize;
        }

        public void Draw(Point coordinate, GraphicsData GraphicsData)
        {
            Coordinate = coordinate;
            if (Buttons.Count == 0)
                return;
            int y = coordinate.Y;
            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].Draw(new Point(((int)coordinate.X + (Size.X - Buttons[i].BoundSize.X) / 4), y), GraphicsData);
                y += Buttons[i].BoundSize.Y;
                y += distanceBetweenButtons;
            }
        }

        public bool HandleMouseClick(Point mouseCoordinate)
        {
            var menuBound = new Rectangle(Coordinate, Size);
            if (new Rectangle(Coordinate, Size).Contains(mouseCoordinate))
            {
                foreach (var button in Buttons)
                {
                    if (button.HandleMouseClick(mouseCoordinate))
                        return true;
                }
                return true; //?????? логично ли 
            }
            return false;
        }
    }
}
