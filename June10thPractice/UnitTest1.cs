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

            checkInventory.IsThereAnyInventory = false;
            
            checkInventory.ProcessOrder(order);

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
                var checkInventory = new FakeInventoryOrderProcessed();

                var order = new Order();
                order.OrderNo = "1";
                //Item�n�������
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

            public bool IsThereAnyInventory { get; set; }//���S���w�s�O�@�Ӫ��A�A�ڭ̴N���ݩʦӤ��O��k

            //�@�ӭntrue�A�@�ӭnfalse�A�ҥH�����[�J�ݩ����ϥΪ�set�L
            protected override bool InventoryCheck(OrderItems item)
            {
                return IsThereAnyInventory;//�������ݩʤ~�����ϥΪ̵��Ȩæs�_�ӡC
                //�p�G�����ݩʡA�ӭn��������k�N�n�A�}�����~��s���ȵ��o�̨ϥ�
            }

        }
    }
}
