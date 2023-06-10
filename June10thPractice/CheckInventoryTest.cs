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
            var fakeinventory = Substitute.For<Inventory>();//這裡不是用介面是因為本來的Production code就不是用介面
            var fakeLog = Substitute.For<ILog>();//用介面就不用用實體，因為介面彈性比較大

            fakeinventory.CheckInventory(Arg.Any<OrderItems>()).Returns(false);
            var orderProcessor = new OrderProcessor(fakeinventory,fakeLog);
            //跑錯要我放log，因此我們在上面先建立假的log，這裡使用

            var order = new Order();
            order.OrderNo="1";
            order.Items = new System.Collections.Generic.List<OrderItems>
            {
                new OrderItems { Id = 1, Name = "Product 1", Quantity = 2 }
            };

            orderProcessor.ProcessOrder(order);

            order.Status.Should().Be(OrderStatus.Deny);
            fakeLog.Received(1).Information("Order is denied");
            fakeLog.Received(1).Information(Arg.Is<string>(s => s.Contains("denied")));//這裡的意思代表只要有包含"denied"
            //沒有回傳東西、沒有狀態改變的就用NSubstitue Recived (和另一個Return都用在假物件身上)
            //Received()與Received(1)相等，數字代表有打出去的次數

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
                var fakeLog = Substitute.For<ILog>();

                fakeinventory.CheckInventory(Arg.Any<OrderItems>()).Returns(true);
                var orderProcessor = new OrderProcessor(fakeinventory, fakeLog);//放"空的"→直接放null

                var order = new Order();
                order.OrderNo = "1";
                order.Items = new System.Collections.Generic.List<OrderItems>
            {
                new OrderItems { Id = 1, Name = "Product 1", Quantity = 2 }
            };

                orderProcessor.ProcessOrder(order);

                order.Status.Should().Be(OrderStatus.Processed);
                fakeLog.Received(0).Information(Arg.Any<string>());//任何字串內容都接收0 //Arg意思為"參數"，因為這個()括號內就是要放參數，型別是字串
                //如果我括號內寫""→就算我Mutation把if拿掉，這裡一定空的""是因為我這裡非Deny就不會傳log出來
                //如果我括號內寫"Order is denied"→就代表針對這串字，Mutation的結果也是我希望的，但如果前面的Message了，就要回頭來改這裡的字串了，所以不如用任何字串內容Arg.Any<string>()
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
