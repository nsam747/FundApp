using System;

namespace Courier.Input
{
    public class Parcel
    {
        public decimal Height { get; }
        public decimal Width { get; }
        public decimal Length { get; }
        public decimal Weight { get; }

        public Parcel(decimal height, decimal width, decimal length, decimal weight)
        {
            Height = height;
            Width = width;
            Length = length;
            Weight = weight;
        }

        public override string ToString()
        {
            return $"(Height: {Height}, Width: {Width}, Length: {Length}, Weight: {Weight})";
        }
    }
}
