using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DotDiff.Tests
{
    [TestFixture]
    public class JsonAuditBuilderTests
    {
        private JsonAuditBuilder<Item>? _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new JsonAuditBuilder<Item>();
        }

        [Test]
        public void Serialize_Include_CorrectProperties()
        {
            var item1 = GetItem(222, true, 33.5M, "MyItem1");
            var item2 = GetItem(4355, true, 0.55M, "MyItem2");

            var jsonString = _builder?.Audit(item1, item2)
                .Include(_ => _.CategoryId)
                .Include(_ => _.Price)
                .Include(_ => _.OnSale)
                .Include(_ => _.SKU)
                .Serialize();
            var pairs = DeserializeJson(jsonString??"");

            pairs.Single(_=> _.Key == nameof(item1.CategoryId));
            pairs.Single(_=> _.Key == nameof(item1.Price));
            pairs.Single(_=> _.Key == nameof(item1.OnSale));
            pairs.Single(_=> _.Key == nameof(item1.SKU));
        }

        [Test]
        public void Serialize_Include_AuditPairs()
        {
            var key = "key1";
            var oldValue = "OldValue1";
            var newValue = "NewValue2";

            var json = _builder?.Include(new AuditPair
            {
                Key = key,
                OldValue = oldValue,
                NewValue = newValue
            })
                .Serialize();
            var pairs = DeserializeJson(json??"");
            var pricePair = pairs.First(_ => _.Key == key);

            Assert.That(pricePair.OldValue, Is.EqualTo(oldValue));
            Assert.That(pricePair.NewValue, Is.EqualTo(newValue));
        }

        [Test]
        public void Serialize_Include_CorrectPropertValues()
        {
            var item1 = GetItem(222, true, 33.5M, "MyItem1");
            var item2 = GetItem(4355, true, 0.55M, "MyItem2");

            var json = _builder?.Audit(item1, item2)
                .Include(_ => _.SKU)
                .Serialize();
            var pairs = DeserializeJson(json??"");
            var skuPair = pairs.First(_ => _.Key == nameof(item1.SKU));

            Assert.That(skuPair.OldValue, Is.EqualTo(item1.SKU));
            Assert.That(skuPair.NewValue, Is.EqualTo(item2.SKU));
        }

        [Test]
        public void Serialize_Include_CorrectPropertValues_For_Decimals()
        {
            var item1 = GetItem(222, true, 33.5M, "MyItem1");
            var item2 = GetItem(4355, true, 0.55M, "MyItem2");

            var jsonString = _builder?.Audit(item1, item2)
                .Include(_ => _.Price)
                .Serialize();
            var pairs = DeserializeJson(jsonString ?? "");
            var pricePair = pairs.First(_ => _.Key == nameof(item1.Price));

            Assert.That(decimal.Parse(pricePair.OldValue), Is.EqualTo(item1.Price));
            Assert.That(decimal.Parse(pricePair.NewValue), Is.EqualTo(item2.Price));
        }

        [Test]
        public void Serialize_Clears_Old_Pairs()
        {
            var item1 = GetItem(222, true, 33.5M, "MyItem1");
            var item2 = GetItem(4355, true, 0.55M, "MyItem2");

            var jsonString = _builder?.Audit(item1, item2)
                .Include(_ => _.Price)
                .Serialize();
            jsonString = _builder?.Serialize();

            var pairs = DeserializeJson(jsonString ?? "");

            CollectionAssert.IsEmpty(pairs);
        }


        private List<AuditPair> DeserializeJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<AuditPair>>(jsonString);
        }

        private Item GetItem(long categoryId = 100,
            bool onSale = false, decimal price = 100M, string skuNumber = "I2344BB")
        {
            return new Item
            {
                CategoryId = categoryId,
                ExpirationDate = DateTime.Now,
                OnSale = onSale,
                Price = price,
                SKU = skuNumber
            };
        }
    }
}
