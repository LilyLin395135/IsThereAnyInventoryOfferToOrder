using ClassLibrary1;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace June10thPractice
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// 要讓ProcessOrder判斷有無庫存，資料庫沒東西，得到Deny
        /// </summary>
        [TestMethod]
        public void ToGetDeny()
        {
            var checkInventory = new FakeInventoryOrderProcessed();

            var order = new Order();
            order.OrderNo="1";
            //Item要做假資料
            order.Items = new System.Collections.Generic.List<OrderItems>
            {
                new OrderItems { Id = 1, Name = "Product 1", Quantity = 2 }
            };

            //.ProcessOrder()是void不回傳
            //所以不能用var result=去接
            checkInventory.ProcessOrder(order);

            //因為主要測試的ProcessOrder是不回傳，所以不能直接接Should().Be()
            //而我們要結果是Deny，Deny是order.Status
            order.Status.Should().Be(OrderStatus.Deny);

        }

        private class FakeInventoryOrderProcessed: OrderProcessor
        {
            protected override bool InventoryCheck(OrderItems item)
            {
                return false;
            }
        }
    }
}
