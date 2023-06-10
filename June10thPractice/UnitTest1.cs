using ClassLibrary1;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace June10thPractice
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// �n��ProcessOrder�P�_���L�w�s�A��Ʈw�S�F��A�o��Deny
        /// </summary>
        [TestMethod]
        public void ToGetDeny()
        {
            var checkInventory = new FakeInventoryOrderProcessed();

            var order = new Order();
            order.OrderNo="1";
            //Item�n�������
            order.Items = new System.Collections.Generic.List<OrderItems>
            {
                new OrderItems { Id = 1, Name = "Product 1", Quantity = 2 }
            };

            //.ProcessOrder()�Ovoid���^��
            //�ҥH�����var result=�h��
            checkInventory.ProcessOrder(order);

            //�]���D�n���ժ�ProcessOrder�O���^�ǡA�ҥH���ઽ����Should().Be()
            //�ӧڭ̭n���G�ODeny�ADeny�Oorder.Status
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
