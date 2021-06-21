using System;
using Courier.Input;
using Courier.Output;
using System.Linq;
using Xunit;
using System.Collections.Generic;

namespace Courier.Tests
{
    public class CourierProcessorTests
    {
        [Fact]
        public void CalculateOrder_Returns_Correct_CourierOrder_Given_An_Order()
        {
            var parcels = new Parcel[] {
                new Parcel(10, 1, 23, 1),
                new Parcel(105, 20, 20, 1),
                new Parcel(3, 4, 2, 1),
                new Parcel(80, 60, 20, 1),
                new Parcel(10, 30, 5, 1),
            };

            var order = new Order(parcels);

            var courierOrder = CourierProcessor.CalculateOrder(order);
            Assert.Equal(7, courierOrder.Items.Count);
            Assert.Equal(ParcelType.Medium, courierOrder.Items.ElementAt(0).Type);
            Assert.Equal(ParcelType.ExtraLarge, courierOrder.Items.ElementAt(1).Type);
            Assert.Equal(ParcelType.Small, courierOrder.Items.ElementAt(2).Type);
            Assert.Equal(ParcelType.Large, courierOrder.Items.ElementAt(3).Type);
            Assert.Equal(ParcelType.Medium, courierOrder.Items.ElementAt(4).Type);
            Assert.Equal(43, courierOrder.TotalCost);
        }

        [Fact]
        public void CalculateOrder_Returns_Correct_CourierOrder_Given_An_Order_With_SpeedyShipping()
        {
            var parcels = new Parcel[] {
                new Parcel(10, 1, 23, 1),
                new Parcel(105, 20, 20, 1),
                new Parcel(3, 4, 2, 1),
                new Parcel(80, 60, 20, 1),
                new Parcel(10, 30, 5, 1),
            };

            var order = new Order(parcels);

            var courierOrder = CourierProcessor.CalculateOrder(order, true);

            Assert.True(courierOrder.UseSpeedyShipping);
            Assert.Equal(8, courierOrder.Items.Count);
            Assert.Equal(ParcelType.SpeedShipping, courierOrder.Items.ElementAt(7).Type);
            Assert.Equal(86, courierOrder.TotalCost);
        }

        [Theory]
        [InlineData(1,1,1, ParcelType.Small)]
        [InlineData(9,9,9, ParcelType.Small)]
        [InlineData(10,10,10, ParcelType.Medium)]
        [InlineData(49,49,49, ParcelType.Medium)]
        [InlineData(50,50,50, ParcelType.Large)]
        [InlineData(99,99,99, ParcelType.Large)]
        [InlineData(100,1,1, ParcelType.ExtraLarge)]
        [InlineData(1,100,1, ParcelType.ExtraLarge)]
        [InlineData(1,1,100, ParcelType.ExtraLarge)]
        public void CalculateParcelType_Returns_Correct_ParcelType_Given_A_Valid_Parcel(decimal height, decimal width, decimal length, ParcelType expectedParcelType)
        {
            var parcel = new Parcel(height, width, length, 1);
            var parcelType = CourierProcessor.CalculateParcelType(parcel);
            Assert.Equal(expectedParcelType, parcelType);
        }

        [Theory]
        [InlineData(0,0,0,0, ParcelType.Small)]
        [InlineData(-1,-1,-1,-1, ParcelType.Small)]
        public void CalculateParcelType_Throws_Exception_Given_An_Invalid_Parcel(decimal height, decimal width, decimal length, decimal weight, ParcelType expectedParcelType)
        {
            var parcel = new Parcel(height, width, length, weight);
            Assert.Throws<Exception>(() => CourierProcessor.CalculateParcelType(parcel));
        }

        [Theory]
        [InlineData(ParcelType.Small, 1, 3)]
        [InlineData(ParcelType.Medium, 3, 8)]
        [InlineData(ParcelType.Large, 6, 15)]
        [InlineData(ParcelType.ExtraLarge, 10, 25)]
        public void CalculateParcelCost_Returns_Correct_Cost_Given_A_ParcelType_And_Weight_Within_The_Weight_Limit(ParcelType parcelType, decimal weight, decimal expectedCost)
        {
            var cost = CourierProcessor.CalculateParcelCost(parcelType, weight);
            Assert.Equal(expectedCost, cost);
        }

        [Theory]
        [InlineData(ParcelType.Small, 2, 5)]
        [InlineData(ParcelType.Medium, 5, 12)]
        [InlineData(ParcelType.Large, 7, 17)]
        [InlineData(ParcelType.ExtraLarge, 12, 29)]
        public void CalculateParcelCost_Returns_Correct_Cost_Given_A_ParcelType_And_Weight_Outside_The_Weight_Limit(ParcelType parcelType, decimal weight, decimal expectedCost)
        {
            var cost = CourierProcessor.CalculateParcelCost(parcelType, weight);
            Assert.Equal(expectedCost, cost);
        }

        [Theory]
        [InlineData(ParcelType.Small, 32, 50)]
        [InlineData(ParcelType.Medium, 34 , 50)]
        public void CalculateParcelCost_Returns_Correct_Cost_Given_A_ParcelType_Eligible_For_HeavyWeight_Classification(ParcelType parcelType, decimal weight, decimal expectedCost)
        {
            var cost = CourierProcessor.CalculateParcelCost(parcelType, weight);
            Assert.Equal(expectedCost, cost);
        }

        [Fact]
        public void GetAvailableDiscounts_Returns_Correct_Discounts()
        {
            var items = new List<CourierParcel>() {
                new CourierParcel(ParcelType.Small, 10),
                new CourierParcel(ParcelType.Small, 10),
                new CourierParcel(ParcelType.Small, 10),
                new CourierParcel(ParcelType.Small, 20),
                new CourierParcel(ParcelType.Small, 20),
                new CourierParcel(ParcelType.Small, 20),
                new CourierParcel(ParcelType.Medium, 20),
                new CourierParcel(ParcelType.Medium, 20),
                new CourierParcel(ParcelType.Medium, 20),
            };

            var discounts = CourierProcessor.GetAvailableDiscounts(items);

            var totalPrice = discounts.Sum(discount => discount.Cost);
            Assert.Equal(5, discounts.Count);
            Assert.Equal(-80, totalPrice);
            Assert.True(discounts.All(discount => discount.Type == ParcelType.Discount));
        }
    }
}
