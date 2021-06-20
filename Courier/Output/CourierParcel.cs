using System;
using Courier.Input;
using System.Linq;

namespace Courier.Output
{
    public class CourierParcel
    {
        public ParcelType Type { get; }
        public decimal Cost { get; }

        public CourierParcel(ParcelType parcelType, decimal cost)
        {
            Type = parcelType;
            Cost = cost;
        }

        // private decimal CalculateCost(ParcelType size) {
        //     var dimensions = new decimal[] {parcel.Height, parcel.Width, parcel.Length};
        //     if(dimensions.Any(dimension => dimension >= 100)) {
        //         return ParcelType.ExtraLarge;
        //     } else if(dimensions.All(dimension => dimension < 100)) {
        //         return ParcelType.Large;
        //     } else if(dimensions.All(dimension => dimension < 50)) {
        //         return ParcelType.Medium;
        //     } else if(dimensions.All(dimension => dimension < 10)) {
        //         return ParcelType.Small;
        //     } else {
        //         throw new Exception($"Unable to map parcel: {parcel} to ParcelType");
        //     }
        // }
    }
}
