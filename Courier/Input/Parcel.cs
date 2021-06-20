using System;

namespace Courier.Input
{
    public class Parcel
    {
        public decimal Height { get; }
        public decimal Width { get; }
        public decimal Length { get; }

        public Parcel(decimal height, decimal width, decimal length)
        {
            Height = height;
            Width = width;
            Length = length;
        }

        public override string ToString()
        {
            return $"(Height: {Height}, Width: {Width}, Length: {Length})";
        }
    }
}
