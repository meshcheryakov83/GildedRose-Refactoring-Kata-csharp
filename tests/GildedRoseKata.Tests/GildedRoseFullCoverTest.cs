﻿using System;
using System.Collections.Generic;
using GildedRoseKata.Model;
using NUnit.Framework;

namespace GildedRoseKata.Tests
{
    /// <summary>
    /// This test fixture cover all items with all possible input values sellIn -100..100 and quality -100..100.
    /// Thus it a bit duplicates tests from GildedRoseTest
    /// </summary>
    [TestFixture]
    public class GildedRoseFullCoverTest
    {
        [TestCase(Item.Dexterity)]
        [TestCase(Item.Elixir)]
        [TestCase("foo")]
        public void UpdateQuality_RegularItem_FullCover(string itemName)
        {
            FullCover(itemName, item =>
            {
                var expectedQuality = item.Quality;
                if (item.Quality > 0)
                {
                    expectedQuality = item.SellIn > 0
                        ? item.Quality - 1
                        : item.Quality - 2;

                    expectedQuality = expectedQuality < 0 ? 0 : expectedQuality;
                }

                return new Item { Name = itemName, SellIn = item.SellIn - 1, Quality = expectedQuality };
            });
        }

        [Test]
        public void UpdateQuality_AgedBrie_FullCover()
        {
            FullCover(Item.AgedBrie, item =>
            {
                var expectedQuality = item.Quality;
                if (item.Quality < 50)
                {
                    expectedQuality = item.SellIn > 0
                        ? item.Quality + 1
                        : item.Quality + 2;
                    expectedQuality = expectedQuality > 50 ? 50 : expectedQuality;
                }

                return new Item { Name = Item.AgedBrie, SellIn = item.SellIn - 1, Quality = expectedQuality };
            });
        }

        [Test]
        public void UpdateQuality_Sulfuras_FullCover()
        {
            FullCover(Item.Sulfuras, item => new Item { Name = Item.Sulfuras, SellIn = item.SellIn, Quality = item.Quality });
        }

        [Test]
        public void UpdateQuality_BackstagePasses_FullCover()
        {
            FullCover(Item.BackstagePasses, item =>
            {
                var expectedQuality = item.Quality;
                if (item.Quality < 50)
                {
                    expectedQuality =
                        item.SellIn <= 0
                            ? 0
                            : item.SellIn > 0 && item.SellIn <= 5
                                ? item.Quality + 3
                                : item.SellIn > 5 && item.SellIn <= 10
                                    ? item.Quality + 2
                                    : item.Quality + 1;

                    expectedQuality = expectedQuality > 50 ? 50 : expectedQuality;
                }

                if (item.SellIn <= 0)
                {
                    expectedQuality = 0;
                }

                return new Item { Name = Item.BackstagePasses, SellIn = item.SellIn - 1, Quality = expectedQuality };
            });
        }

        private static void FullCover(string itemName, Func<Item, Item> calcExpectedItem)
        {
            for (var sellIn = -100; sellIn < 100; sellIn++)
            {
                for (var quality = -100; quality < 100; quality++)
                {
                    var initialItem = new Item { Name = itemName, SellIn = sellIn, Quality = quality };
                    var expectedItem = calcExpectedItem(initialItem);

                    var items = new List<Item> { initialItem };
                    new GildedRose(items).UpdateQuality();

                    Assert.AreEqual(expectedItem.Quality, items[0].Quality, $"{itemName}, sellIn={sellIn}, quality={quality}: quality was updated incorrectly");
                    Assert.AreEqual(expectedItem.SellIn, items[0].SellIn, $"{itemName}, sellIn={sellIn}, quality={quality}: sellIn was updated incorrectly");
                    Assert.AreEqual(expectedItem.Name, items[0].Name, $"{itemName}, sellIn={sellIn}, quality={quality}: name was updated");
                }
            }
        }
    }
}