using System;
using System.Collections.Generic;
using System.Text;

namespace ShopCarts
{
    [Serializable]
    public class ShoppingCart : IDisposable
    {
        public List<Item> Items = new List<Item>();

        public int Count
        {
            get { return Items.Count; }
        }

        public void Add(Item item)
        {
            Items.Add(item);
        }

        public void Dispose()
        {
            Items = null;
        }

        public void Remove(int index)
        {
            Items.RemoveAt(index);
        }
    }

    [Serializable]
    public class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
