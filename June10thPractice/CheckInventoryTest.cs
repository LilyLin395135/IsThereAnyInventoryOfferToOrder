using ClassLibrary1;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IsThereAnyInventoryOfferToOrder
{
    [TestClass]
    public class CheckInventoryTest
    {
        /// <summary>
        /// 要讓ProcessOrder判斷有無庫存，資料庫沒東西，得到Deny
        /// </summary>
        [TestMethod]
        public void ToGetDeny()
        {
            var fakeinventory = new FakeInventory();
            var orderProcessor = new OrderProcessor(fakeinventory);

            var order = new Order();
            order.OrderNo="1";
            //Item要做假資料
            order.Items = new System.Collections.Generic.List<OrderItems>
            {
                new OrderItems { Id = 1, Name = "Product 1", Quantity = 2 }
            };

            fakeinventory.IsThereAnyInventory = false;

            orderProcessor.ProcessOrder(order);

            order.Status.Should().Be(OrderStatus.Deny);

        }

        [TestClass]
        public class UnitTest2
        {
            /// <summary>
            /// 資料庫有東西，得到Processed
            /// </summary>
            [TestMethod]
            public void ToGetProcessed()
            {
                var fakeinventory = new FakeInventory();
                var orderProcessor = new OrderProcessor(fakeinventory);

                var order = new Order();
                order.OrderNo = "1";
                //Item要做假資料
                order.Items = new System.Collections.Generic.List<OrderItems>
            {
                new OrderItems { Id = 1, Name = "Product 1", Quantity = 2 }
            };

                fakeinventory.IsThereAnyInventory = true;

                orderProcessor.ProcessOrder(order);

                order.Status.Should().Be(OrderStatus.Processed);

            }
        }

        public class FakeInventory: Inventory
        {

            public bool IsThereAnyInventory { get; set; }//有沒有庫存是一個狀態，我們就用屬性而不是方法

            public override bool CheckInventory(OrderItems item)
            {
                return IsThereAnyInventory;
            }

        }
    }
}
