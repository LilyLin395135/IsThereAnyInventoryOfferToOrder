using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1
{
    public class OrderProcessor
    {
        private readonly Inventory _inventory;
        
        //無參數建構式
        public OrderProcessor()
        {
             _inventory = new Inventory();
        }

        //有參數建構式，為了做依賴注入，放假的庫存資料
        public OrderProcessor(Inventory anyInventory)
        {
            _inventory = anyInventory;
        }

        
        public void ProcessOrder(Order order)
        {
            //這裡要讓IsValid()通過，就要有OrderNo傳入、Items.Any()有東西
            if (!order.IsValid())
            {
                throw new Exception("Invalid order.");
            }

            //這裡要記得把inventory改成_unventory
            //var inventory = new Inventory();
            var result = order
                .Items
                .Select(item => _inventory.CheckInventory(item)).ToList();

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
        public virtual bool CheckInventory(OrderItems item)
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
