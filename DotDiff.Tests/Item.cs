using System;

namespace DotDiff.Tests
{
    public class Item
    {
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool OnSale { get; set; }
    }
}
