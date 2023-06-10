using ClassLibrary1;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

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
            var fakeinventory = Substitute.For<Inventory>();
            //1.代表指定空的OrderItems就會回傳false↓
            //fakeinventory.CheckInventory(new OrderItems()).Returns(false);
            //2.代表指定Name是"Product 1"的回傳false。//o => o.Name == "Product 1"【o代表前面的OrderItems】
            //fakeinventory.CheckInventory(Arg.Is<OrderItems>(o => o.Name == "Product 1")).Returns(false);
            fakeinventory.CheckInventory(Arg.Any<OrderItems>()).Returns(false);
            var orderProcessor = new OrderProcessor(fakeinventory);

            var order = new Order();
            order.OrderNo="1";
            //Item要做假資料
            order.Items = new System.Collections.Generic.List<OrderItems>
            {
                new OrderItems { Id = 1, Name = "Product 1", Quantity = 2 }
            };

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
                var fakeinventory = Substitute.For<Inventory>();
                fakeinventory.CheckInventory(Arg.Any<OrderItems>()).Returns(true);
                var orderProcessor = new OrderProcessor(fakeinventory);

                var order = new Order();
                order.OrderNo = "1";
                order.Items = new System.Collections.Generic.List<OrderItems>
            {
                new OrderItems { Id = 1, Name = "Product 1", Quantity = 2 }
            };

                orderProcessor.ProcessOrder(order);

                order.Status.Should().Be(OrderStatus.Processed);

            }
        }

        //如果放入四個參數進去依賴注入，就要override四個，所以這裡可以簡化
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
