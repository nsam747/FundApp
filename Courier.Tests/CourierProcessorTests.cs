using System;
using Courier.Input;
using Courier.Output;
using System.Linq;
using Xunit;

namespace Courier.Tests
{
    public class CourierProcessorTests
    {
        [Fact]
        public void CalculateOrder_Returns_Correct_CourierOrder_Given_An_Order()
        {
            var parcels = new Parcel[] {
                new Parcel(10, 1, 23),
                new Parcel(105, 20, 20),
                new Parcel(3, 4, 2),
                new Parcel(80, 60, 20),
                new Parcel(10, 30, 5),
            };

            var order = new Order(parcels);

            var courierOrder = CourierProcessor.CalculateOrder(order);
            Assert.Equal(courierOrder.Items.Count, parcels.Length);
            Assert.Equal(courierOrder.Items.ElementAt(0).Type, ParcelType.Medium);
            Assert.Equal(courierOrder.Items.ElementAt(1).Type, ParcelType.ExtraLarge);
            Assert.Equal(courierOrder.Items.ElementAt(2).Type, ParcelType.Small);
            Assert.Equal(courierOrder.Items.ElementAt(3).Type, ParcelType.Large);
            Assert.Equal(courierOrder.Items.ElementAt(4).Type, ParcelType.Medium);
            Assert.Equal(courierOrder.TotalCost, 59);
        }

        [Fact]
        public void CalculateOrder_Returns_Correct_CourierOrder_Given_An_Order_With_SpeedyShipping()
        {
            var parcels = new Parcel[] {
                new Parcel(10, 1, 23),
                new Parcel(105, 20, 20),
                new Parcel(3, 4, 2),
                new Parcel(80, 60, 20),
                new Parcel(10, 30, 5),
            };

            var order = new Order(parcels);

            var courierOrder = CourierProcessor.CalculateOrder(order, true);
            
            Assert.True(courierOrder.UseSpeedyShipping);
            Assert.Equal(courierOrder.Items.Count, parcels.Length + 1);
            Assert.Equal(courierOrder.Items.ElementAt(5).Type, ParcelType.SpeedShipping);
            Assert.Equal(courierOrder.TotalCost, 118);
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
            var parcel = new Parcel(height, width, length);
            var parcelType = CourierProcessor.CalculateParcelType(parcel);
            Assert.Equal(parcelType, expectedParcelType);
        }

        [Theory]
        [InlineData(0,0,0, ParcelType.Small)]
        [InlineData(-1,-1,-1, ParcelType.Small)]
        public void CalculateParcelType_Throws_Exception_Given_An_Invalid_Parcel(decimal height, decimal width, decimal length, ParcelType expectedParcelType)
        {
            var parcel = new Parcel(height, width, length);
            Assert.Throws<Exception>(() => CourierProcessor.CalculateParcelType(parcel));
        }

        [Theory]
        [InlineData(ParcelType.Small, 3)]
        [InlineData(ParcelType.Medium, 8)]
        [InlineData(ParcelType.Large, 15)]
        [InlineData(ParcelType.ExtraLarge, 25)]
        public void CalculateParcelTypeCost_Returns_Correct_Cost_Given_A_ParcelType(ParcelType parcelType, decimal expectedCost)
        {
            var cost = CourierProcessor.CalculateParcelTypeCost(parcelType);
            Assert.Equal(cost, expectedCost);
        }
    }
}
