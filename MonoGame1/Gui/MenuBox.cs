using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame1.Gui
{
    public class MenuBox
    {
        public List<Button> Buttons { get; private set; }
        public Point Coordinate;
        public int distanceBetweenButtons { get; private set; }

        public MenuBox(/*Point coordinate ,*/ List<Button> buttons, int distance)
        {
            //Coordinate = coordinate;
            distanceBetweenButtons = distance;
            Buttons = buttons;
        }

        public void Draw(Point coordinate)
        {
            if (Buttons.Count == 0)
                return;
            for (int i = 0; i < Buttons.Count; i++)
            {
                
            }
        }

        public void HandleMouseClick()
        {
        }
    }
}
