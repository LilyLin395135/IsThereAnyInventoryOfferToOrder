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
            fakeLog.Received(1).Information("Order is denied");
            fakeLog.Received(1).Information(Arg.Is<string>(s => s.Contains("denied")));//�o�̪��N��N��u�n���]�t"denied"
            //�S���^�ǪF��B�S�����A���ܪ��N��NSubstitue Recived (�M�t�@��Return���Φb�����󨭤W)
            //Received()�PReceived(1)�۵��A�Ʀr�N�����X�h������

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
                var fakeLog = Substitute.For<ILog>();

                fakeinventory.CheckInventory(Arg.Any<OrderItems>()).Returns(true);
                var orderProcessor = new OrderProcessor(fakeinventory, fakeLog);//��"�Ū�"��������null

                var order = new Order();
                order.OrderNo = "1";
                order.Items = new System.Collections.Generic.List<OrderItems>
            {
                new OrderItems { Id = 1, Name = "Product 1", Quantity = 2 }
            };

                orderProcessor.ProcessOrder(order);

                order.Status.Should().Be(OrderStatus.Processed);
                fakeLog.Received(0).Information(Arg.Any<string>());//����r�ꤺ�e������0 //Arg�N�䬰"�Ѽ�"�A�]���o��()�A�����N�O�n��ѼơA���O�O�r��
                //�p�G�ڬA�����g""���N���Mutation��if�����A�o�̤@�w�Ū�""�O�]���ڳo�̫DDeny�N���|��log�X��
                //�p�G�ڬA�����g"Order is denied"���N�N��w��o��r�AMutation�����G�]�O�ڧƱ檺�A���p�G�e����Message�F�A�N�n�^�Y�ӧ�o�̪��r��F�A�ҥH���p�Υ���r�ꤺ�eArg.Any<string>()
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
