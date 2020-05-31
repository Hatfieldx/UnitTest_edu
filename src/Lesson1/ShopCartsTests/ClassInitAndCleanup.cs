using ShopCarts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ShopCartsTests
{
    //[TestClass]
    [Collection("ShoppingCart")]
    public class ClassInitAndCleanup
    {
        private readonly ShoppingCartFixture _scfixture;
        private readonly ITestOutputHelper _output;

        public ClassInitAndCleanup(ITestOutputHelper helper, ShoppingCartFixture scf)
        {
            _scfixture = scf;
            _output = helper;
        }

        [Fact]
        public void ShopingCart_AddToCart()
        {
            int expected = _scfixture.ShoppingCart.Items.Count + 1;

            _scfixture.ShoppingCart.Add(new Item() { Name = "Test", Quantity = 1 });

            Assert.Equal(expected, _scfixture.ShoppingCart.Count);
        }

        [Fact]
        public void ShopingCart_RemoveFromCart()
        {
            int expected = _scfixture.ShoppingCart.Items.Count - 1;

            _scfixture.ShoppingCart.Remove(0);

            Assert.Equal(expected, _scfixture.ShoppingCart.Count);
        }
    }

    public class ShoppingCartFixture : IDisposable
    {
        public ShoppingCart ShoppingCart { get; set; }
        public ShoppingCartFixture()
        {
            Debug.WriteLine("Class Initialize");
            Item item = new Item();
            item.Name = "Unit Test Video Lessons";
            item.Quantity = 10;

            ShoppingCart = new ShoppingCart();
            ShoppingCart.Add(item);
        }

        public void Dispose()
        {
            Debug.WriteLine("Class CleanUp");
            //cart.Dispose();
        }
    }
    [CollectionDefinition("ShoppingCart")]
    public class ShoppingCartCollection : ICollectionFixture<ShoppingCartFixture>
    {
    }
}
