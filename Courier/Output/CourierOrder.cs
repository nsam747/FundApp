using System;
using System.Collections.Generic;
using System.Linq;

namespace Courier.Output
{
    public class CourierOrder
    {
        public IReadOnlyCollection<CourierParcel> Items { get; }
        public decimal TotalCost => Items.Sum(item => item.Cost);
        public CourierOrder(IReadOnlyCollection<CourierParcel> items)
        {
            Items = items;
        }
    }
}
