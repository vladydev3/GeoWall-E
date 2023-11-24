global using System.Collections.Generic;
global using System;
namespace GeoWall_E
{
    public enum Colors
    {
        Black,
        White,
        Red,
        Green,
        Blue,
        Yellow,
        Magenta,
        Cyan,
        Gray
    }

    public class Color
    {
        private Colors Color_ { get; set; }
        public Color(Colors color)
        {
            Color_ = color;
        }

        public void SetColor(Colors color)
        {
            Color_ = color;
        }
        public override string ToString()
        {
            return Color_.ToString();
        }

        public Colors GetColor()
        {
            return Color_;
        }
    }
}