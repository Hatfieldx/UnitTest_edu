using ShopCarts;
using System;
using System.Diagnostics;
using Xunit;

namespace ShopCartsTests
{
    //[TestClass]
    public class TestInitAndCleanUp : IDisposable
    {
        private ShoppingCart cart;
        private Item item;

        public TestInitAndCleanUp()
        {
            TestInitialize();
        }

        // Запускается перед стартом каждого тестирующего метода
       // [TestInitialize]
        private void TestInitialize()
        {
            Debug.WriteLine("Test Initialize");
            item = new Item();
            item.Name = "Unit Test Video Lessons";
            item.Quantity = 10;

            cart = new ShoppingCart();
            cart.Add(item);
        }

        // Запускается после завершения каждого тестирующего метода
        //[TestCleanup]
        private void TestCleanUp()
        {
            Debug.WriteLine("Test CleanUp");
            cart.Dispose();
        }

        [Fact]
        public void ShopingCart_CheckOut_ContainsItem()
        {
            Assert.Contains(cart.Items, x => x == item);
        }

        [Fact]
        public void ShopingCart_RemoveItem_Empty()
        {
            int expected = 0;

            cart.Remove(0);

            Assert.Equal(expected, cart.Count);
        }

        public void Dispose()
        {
            TestCleanUp();
        }
    }
}
