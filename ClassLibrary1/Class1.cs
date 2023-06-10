using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1
{
    public class OrderProcessor
    {
        public void ProcessOrder(Order order)
        {
            //這裡要讓IsValid()通過，就要有OrderNo傳入、Items.Any()有東西
            if (!order.IsValid())
            {
                throw new Exception("Invalid order.");
            }

            //這裡要讓原本的inventory.CheckInventory(item)等於false，就把整個提取出來可以覆寫
            //首先因為這裡只有使用一次inventory，所以可以把它寫成一句new Inventory().CheckInventory(item)，一起提取
            //就不會影響原本的，外面也可以直接return false
            //var inventory = new Inventory();
            var result = order
                .Items
                .Select(item => InventoryCheck(item)).ToList();

            order.Status = result.Any(a => a == false) ? OrderStatus.Deny : OrderStatus.Processed;
        }

        protected virtual bool InventoryCheck(OrderItems item)
        {
            return new Inventory().CheckInventory(item);
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

        public bool IsValid()//判斷有沒有東西，通過就要有OrderNo傳入、Items.Any()有東西
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
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
