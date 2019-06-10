using Countdown_Timer.Abstractions;
using Countdown_Timer.Models;
using Countdown_Timer.ViewModels;
using Moq;
using NUnit.Framework;

namespace Countdown_Timer.Tests.ViewModels.CountDownViewModelTests
{
    [TestFixture]
    public class Ctor
    {
        [Test]
        public void Initial_State_is_Stopped()
        {
            var timerMock  = new Mock<ITimer>();
            var configMock = new Mock<IConfiguration>();
            var beeperMock = new Mock<IBeeper>();

            var sut = new CountDownViewModel(timerMock.Object, configMock.Object, beeperMock.Object);

            Assert.AreEqual(State.Stopped, sut.State);
        }
        //---------------------------------------------------------------------
        [Test]
        public void FontSize_gets_set()
        {
            var timerMock  = new Mock<ITimer>();
            var configMock = new Mock<IConfiguration>();
            var beeperMock = new Mock<IBeeper>();

            configMock
                .Setup(c => c.FontSize)
                .Returns(42)
                .Verifiable();

            var sut = new CountDownViewModel(timerMock.Object, configMock.Object, beeperMock.Object);

            Assert.AreEqual(42, sut.FontSize);
        }
    }
}
