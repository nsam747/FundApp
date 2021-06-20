﻿using System;
using Courier.Input;
using Courier.Output;
using System.Linq;
using System.Collections.Generic;

namespace Courier
{
    public static class CourierProcessor
    {
        private static readonly Dictionary<ParcelType, decimal> ParcelTypeCostMap = new Dictionary<ParcelType, decimal>{
            { ParcelType.Small, 3 },
            { ParcelType.Medium, 8 },
            { ParcelType.Large, 15 },
            { ParcelType.ExtraLarge, 25 }
        };

        private static readonly Dictionary<ParcelType, decimal> ParcelTypeWeightLimitMap = new Dictionary<ParcelType, decimal>{
            { ParcelType.Small, 1 },
            { ParcelType.Medium, 3 },
            { ParcelType.Large, 6 },
            { ParcelType.ExtraLarge, 10 }
        };

        public static CourierOrder CalculateOrder(Order order, bool speedyShipping = false)
        {
            var items = order.Items.Select(item =>
            {
                var parcelType = CalculateParcelType(item);
                var cost = CalculateParcelCost(parcelType, item.Weight);
                return new CourierParcel(parcelType, cost);
            }).ToList();

            if(speedyShipping) {
                var speedyShippingEntry = new CourierParcel(ParcelType.SpeedShipping, items.Sum(item => item.Cost));
                items.Add(speedyShippingEntry);
            }

            return new CourierOrder(items, speedyShipping);
        }

        public static ParcelType CalculateParcelType(Parcel parcel)
        {
            var dimensions = new decimal[] { parcel.Height, parcel.Width, parcel.Length };

            if (dimensions.Any(dimension => dimension <= 0)) {
                 throw new Exception($"Unable to map invalid parcel: {parcel} to ParcelType");
            }

            if (dimensions.Any(dimension => dimension >= 100))
            {
                return ParcelType.ExtraLarge;
            }
            else if (dimensions.All(dimension => dimension < 100))
            {
                if (dimensions.All(dimension => dimension < 50))
                {
                    if (dimensions.All(dimension => dimension < 10))
                    {
                        return ParcelType.Small;
                    }
                    else
                    {
                        return ParcelType.Medium;
                    }
                }
                else
                {
                    return ParcelType.Large;
                }
            }
            else
            {
                throw new Exception($"Unable to map parcel: {parcel} to ParcelType");
            }
        }

        public static decimal CalculateParcelCost(ParcelType parcelType, decimal weight)
        {
            var baseCost = ParcelTypeCostMap[parcelType];
            var weightLimit = ParcelTypeWeightLimitMap[parcelType];
            if(weight > weightLimit) {
                var weightFee = (weight - weightLimit) * 2;
                return weightFee + baseCost;
            } else {
                return baseCost;
            }
        }
    }
}
