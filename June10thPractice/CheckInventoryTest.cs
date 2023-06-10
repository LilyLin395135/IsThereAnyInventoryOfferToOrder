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
        /// �n��ProcessOrder�P�_���L�w�s�A��Ʈw�S�F��A�o��Deny
        /// </summary>
        [TestMethod]
        public void ToGetDeny()
        {
            var fakeinventory = Substitute.For<Inventory>();//�o�̤��O�Τ����O�]�����Ӫ�Production code�N���O�Τ���
            var fakeLog = Substitute.For<ILog>();//�Τ����N���Υι���A�]�������u�ʤ���j

            fakeinventory.CheckInventory(Arg.Any<OrderItems>()).Returns(false);
            var orderProcessor = new OrderProcessor(fakeinventory,fakeLog);
            //�]���n�ک�log�A�]���ڭ̦b�W�����إ߰���log�A�o�̨ϥ�

            var order = new Order();
            order.OrderNo="1";
            order.Items = new System.Collections.Generic.List<OrderItems>
            {
                new OrderItems { Id = 1, Name = "Product 1", Quantity = 2 }
            };

            orderProcessor.ProcessOrder(order);

            order.Status.Should().Be(OrderStatus.Deny);
            fakeLog.Received().Information("Order is denied");
            //�S���^�ǪF��B�S�����A���ܪ��N��NSubstitue Recived (�M�t�@��Return���Φb�����󨭤W)
            //Received()�PReceived(1)�۵�

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
                var fakeinventory = Substitute.For<Inventory>();
                fakeinventory.CheckInventory(Arg.Any<OrderItems>()).Returns(true);
                var orderProcessor = new OrderProcessor(fakeinventory, null);//��"�Ū�"��������null

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

        //�p�G��J�|�ӰѼƶi�h�̿�`�J�A�N�noverride�|�ӡA�ҥH�o�̥i�H²��
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
