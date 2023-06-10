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

            checkInventory.IsThereAnyInventory = false;
            
            checkInventory.ProcessOrder(order);

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
                var checkInventory = new FakeInventoryOrderProcessed();

                var order = new Order();
                order.OrderNo = "1";
                //Item要做假資料
                order.Items = new System.Collections.Generic.List<OrderItems>
            {
                new OrderItems { Id = 1, Name = "Product 1", Quantity = 2 }
            };

                checkInventory.IsThereAnyInventory = true;

                checkInventory.ProcessOrder(order);

                order.Status.Should().Be(OrderStatus.Processed);

            }
        }

            public class FakeInventoryOrderProcessed: OrderProcessor
        {

            public bool IsThereAnyInventory { get; set; }//有沒有庫存是一個狀態，我們就用屬性而不是方法

            //一個要true，一個要false，所以提取加入屬性讓使用者set他
            protected override bool InventoryCheck(OrderItems item)
            {
                return IsThereAnyInventory;//提取成屬性才能讓使用者給值並存起來。
                //如果不用屬性，而要提取成方法就要再開個欄位才能存取值給這裡使用
            }

        }
    }
}
