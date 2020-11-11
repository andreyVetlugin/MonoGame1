using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame1.RenderTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame1.Gui
{
    public class TextBox
    {
        //event Action ButtonRealised;
        public Rectangle Bound { get; private set; }
        public Color TextColor { get; set; } = Color.Red;
        public Color BoundColor { get; set; } = Color.Black;
        public bool WithBounding
        {
            get; private set;
        }

        private Point boundSize;
        public Point BoundSize
        {
            get
            {
                if (boundSize == null) SetBoundSize();
                return boundSize;
            }
            private set { boundSize = value; }
        }

        private int padding;
        public int Padding
        {
            get { return padding; }
            set { padding = value; SetBoundSize(); }
        }

        private string messageText;
        public string MessageText
        {
            get { return messageText; }
            set
            {
                messageText = value;
                SetBoundSize();
            }
        }

        private SpriteFont messageFont;
        public SpriteFont MessageFont
        {
            get { return messageFont; }
            set { messageFont = value; SetBoundSize(); }
        }

        public TextBox(SpriteFont font, string message, int padding = 3)
        {
            messageFont = font;
            messageText = message;
            Padding = padding;
        }

        private void SetBoundSize()
        {
            if (messageFont == null)
                return;
            var messageTextSize = MessageFont.MeasureString(MessageText);
            BoundSize = new Point((int)messageTextSize.X + 2 * Padding, (int)messageTextSize.Y + 2 * Padding);
        }

        public void Draw(Point position, GraphicsData GraphicsData)
        {
            Bound = new Rectangle(position, BoundSize);
            GraphicsData.SpriteBatch.DrawRectangle(position.ToVector2(), BoundSize.ToVector2(), BoundColor);
            GraphicsData.SpriteBatch.DrawString(MessageFont, MessageText, position.ToVector2() + new Vector2(Padding), TextColor);
        }
    }
}
