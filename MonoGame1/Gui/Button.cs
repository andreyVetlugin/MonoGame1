using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame1.Gui
{

    public class Button : TextBox
    {
        public event Action ButtonRealised;
        public Button(SpriteFont font, string message) : base(font, message)
        {
        }

        public Button(SpriteFont font, string message, int padding) : base(font, message, padding)
        {
        }

        private void RealiseButton()
        {
            ButtonRealised?.Invoke();
        }

        public bool HandleMouseClick(Point coordinate)
        {
            if (Bound.Contains(coordinate))
            {
                RealiseButton();
                return true;
            }
            return false;
        }
    }
}
