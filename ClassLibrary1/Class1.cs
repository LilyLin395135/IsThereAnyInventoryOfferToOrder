using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1
{
    public class OrderProcessor
    {
        public void ProcessOrder(Order order)
        {
            if (!order.IsValid())
            {
                throw new Exception("Invalid order.");
            }

            var inventory = new Inventory();
            var result = order
                .Items
                .Select(item => inventory.CheckInventory(item)).ToList();

            order.Status = result.Any(a => a == false) ? OrderStatus.Deny : OrderStatus.Processed;
        }
    }

    public class Inventory
    {
        /// <summary>
        /// 判斷是否有足夠庫存，若有庫存回傳 true，否則回傳 false。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CheckInventory(OrderItems item)
        {
            throw new NotImplementedException();
        }
    }

    public class Order
    {
        public string OrderNo { get; set; }
        public List<OrderItems> Items { get; set; }
        public OrderStatus Status { get; set; }

        // ... other properties

        public bool IsValid()//判斷有沒有東西
        {
            return string.IsNullOrEmpty(OrderNo) == false && Items.Any();
        }


    }

    public enum OrderStatus
    {
        Processed,
        Deny
    }

    public class OrderItems
    {
        // ... properties
    }
}
