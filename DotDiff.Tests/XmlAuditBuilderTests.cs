using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NUnit.Framework;

namespace DotDiff.Tests
{
    [TestFixture]
    public class XmlAuditBuilderTests
    {
        private XmlAuditBuilder<Item> _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new XmlAuditBuilder<Item>();
        }

        [Test]
        public void Serialize_Include_CorrectProperties()
        {
            var item1 = GetItem(222, true, 33.5M, "MyItem1");
            var item2 = GetItem(4355, true, 0.55M, "MyItem2");

            var xml = _builder.Audit(item1, item2)
                .Include(_ => _.CategoryId)
                .Include(_ => _.Price)
                .Include(_ => _.OnSale)
                .Include(_ => _.SKU)
                .Serialize();
            var pairs = DeserializeXml(xml);

            pairs.Single(_=> _.Key == nameof(item1.CategoryId));
            pairs.Single(_=> _.Key == nameof(item1.Price));
            pairs.Single(_=> _.Key == nameof(item1.OnSale));
            pairs.Single(_=> _.Key == nameof(item1.SKU));
        }

        [Test]
        public void Serialize_Include_CorrectPropertValues()
        {
            var item1 = GetItem(222, true, 33.5M, "MyItem1");
            var item2 = GetItem(4355, true, 0.55M, "MyItem2");

            var xml = _builder.Audit(item1, item2)
                .Include(_ => _.SKU)
                .Serialize();
            var pairs = DeserializeXml(xml);
            var skuPair = pairs.First(_ => _.Key == nameof(item1.SKU));

            Assert.That(skuPair.OldValue, Is.EqualTo(item1.SKU));
            Assert.That(skuPair.NewValue, Is.EqualTo(item2.SKU));
        }

        [Test]
        public void Serialize_Include_CorrectPropertValues_For_Decimals()
        {
            var item1 = GetItem(222, true, 33.5M, "MyItem1");
            var item2 = GetItem(4355, true, 0.55M, "MyItem2");

            var xml = _builder.Audit(item1, item2)
                .Include(_ => _.Price)
                .Serialize();
            var pairs = DeserializeXml(xml);
            var pricePair = pairs.First(_ => _.Key == nameof(item1.Price));

            Assert.That(decimal.Parse(pricePair.OldValue), Is.EqualTo(item1.Price));
            Assert.That(decimal.Parse(pricePair.NewValue), Is.EqualTo(item2.Price));
        }

        [Test]
        public void Serialize_Include_AuditPairs()
        {
            var key = "key1";
            var oldValue = "OldValue1";
            var newValue = "NewValue2";

            var xml = _builder.Include(new AuditPair
                {
                    Key = key,
                    OldValue = oldValue,
                    NewValue = newValue
                })
                .Serialize();
            var pairs = DeserializeXml(xml);
            var pricePair = pairs.First(_ => _.Key == key);

            Assert.That(pricePair.OldValue, Is.EqualTo(oldValue));
            Assert.That(pricePair.NewValue, Is.EqualTo(newValue));
        }

        [Test]
        public void Serialize_Clears_Old_Pairs()
        {
            var item1 = GetItem(222, true, 33.5M, "MyItem1");
            var item2 = GetItem(4355, true, 0.55M, "MyItem2");

            var xml = _builder.Audit(item1, item2)
                .Include(_ => _.Price)
                .Serialize();
            xml = _builder.Serialize();

            var pairs = DeserializeXml(xml);
          
            CollectionAssert.IsEmpty(pairs);
        }

        private List<AuditPair> DeserializeXml(string xml)
        {
            var serializer = new XmlSerializer(typeof(List<AuditPair>));
            var str = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));
            return (List<AuditPair>)serializer.Deserialize(str);
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
