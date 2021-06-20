using System;
using System.Collections.Generic;

namespace Courier.Input
{
    public class Order
    {
        public IReadOnlyCollection<Parcel> Items { get; }
        public Order(IReadOnlyCollection<Parcel> items)
        {
            Items = items;
        }
    }
}
