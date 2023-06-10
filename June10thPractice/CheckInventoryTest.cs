using ClassLibrary1;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IsThereAnyInventoryOfferToOrder
{
    [TestClass]
    public class CheckInventoryTest
    {
        /// <summary>
        /// �n��ProcessOrder�P�_���L�w�s�A��Ʈw�S�F��A�o��Deny
        /// </summary>
        [TestMethod]
        public void ToGetDeny()
        {
            var fakeinventory = new FakeInventory();
            var orderProcessor = new OrderProcessor(fakeinventory);

            var order = new Order();
            order.OrderNo="1";
            //Item�n�������
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
            /// ��Ʈw���F��A�o��Processed
            /// </summary>
            [TestMethod]
            public void ToGetProcessed()
            {
                var fakeinventory = new FakeInventory();
                var orderProcessor = new OrderProcessor(fakeinventory);

                var order = new Order();
                order.OrderNo = "1";
                //Item�n�������
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

            public bool IsThereAnyInventory { get; set; }//���S���w�s�O�@�Ӫ��A�A�ڭ̴N���ݩʦӤ��O��k

            public override bool CheckInventory(OrderItems item)
            {
                return IsThereAnyInventory;
            }

        }
    }
}
