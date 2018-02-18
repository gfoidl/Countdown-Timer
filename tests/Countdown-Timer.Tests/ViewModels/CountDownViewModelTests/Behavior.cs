using Countdown_Timer.Contracts;
using Countdown_Timer.ViewModels;
using Moq;
using NUnit.Framework;

namespace Countdown_Timer.Tests.ViewModels.CountDownViewModelTests
{
    [TestFixture]
    public class Behavior
    {
        private Mock<IBeeper>        _beeperMock;
        private Mock<IConfiguration> _configMock;

        private TestTimer      _timer;
        private IBeeper        _beeper;
        private IConfiguration _config;
        //---------------------------------------------------------------------
        private CountDownViewModel CreateSut() => new CountDownViewModel(_timer, _config, _beeper);
        //---------------------------------------------------------------------
        [SetUp]
        public void SetUp()
        {
            _beeperMock = new Mock<IBeeper>();
            _configMock = new Mock<IConfiguration>();

            _timer  = new TestTimer();
            _beeper = _beeperMock.Object;
            _config = _configMock.Object;
        }
        //---------------------------------------------------------------------
        [Test]
        public void Init___Start_can_execute()
        {
            CountDownViewModel sut = this.CreateSut();

            Assert.IsTrue(sut.StartCommand.CanExecute(null));
        }
        //---------------------------------------------------------------------
        [Test]
        public void Init___Stop_cant_execute()
        {
            CountDownViewModel sut = this.CreateSut();

            Assert.IsFalse(sut.StopCommand.CanExecute(null));
        }
        //---------------------------------------------------------------------
        [Test]
        public void Start_executed___Start_cant_execute()
        {
            CountDownViewModel sut = this.CreateSut();

            sut.StartCommand.Execute(null);

            Assert.IsFalse(sut.StartCommand.CanExecute(null));
        }
        //---------------------------------------------------------------------
        [Test]
        public void Start_executed___Stop_can_execute()
        {
            CountDownViewModel sut = this.CreateSut();

            sut.StartCommand.Execute(null);

            Assert.IsTrue(sut.StopCommand.CanExecute(null));
        }
        //---------------------------------------------------------------------
        [Test]
        public void Stop_executed_after_Start___Start_can_execute()
        {
            CountDownViewModel sut = this.CreateSut();

            sut.StartCommand.Execute(null);
            sut.StopCommand.Execute(null);

            Assert.IsTrue(sut.StartCommand.CanExecute(null));
        }
        //---------------------------------------------------------------------
        [Test]
        public void Stop_executed_after_Start___Stop_cant_execute()
        {
            CountDownViewModel sut = this.CreateSut();

            sut.StartCommand.Execute(null);
            sut.StopCommand.Execute(null);

            Assert.IsFalse(sut.StopCommand.CanExecute(null));
        }
        //---------------------------------------------------------------------
        [Test]
        public void Counting_down_to_0___restarting()
        {
            _configMock
                .Setup(c => c.StartSeconds)
                .Returns(3)
                .Verifiable();

            CountDownViewModel sut = this.CreateSut();

            _configMock.Verify();

            sut.StartCommand.Execute(null);

            Assert.AreEqual(3, sut.Seconds, "after Start");
            Assert.AreEqual(State.Red, sut.State, "after Start");

            _timer.OnTick();    // 2
            _timer.OnTick();    // 1
            _timer.OnTick();    // 0
            _timer.OnTick();    // 3

            Assert.AreEqual(3, sut.Seconds, "should have restarted");
            Assert.AreEqual(State.Red, sut.State, "should have restarted");
        }
        //---------------------------------------------------------------------
        [Test]
        public void Counting_down_to_0___correct_states()
        {
            _configMock
                .Setup(c => c.StartSeconds)
                .Returns(3)
                .Verifiable();

            _configMock
                .Setup(c => c.SecondsForYellow)
                .Returns(1)
                .Verifiable();

            CountDownViewModel sut = this.CreateSut();

            _configMock.Verify();

            sut.StartCommand.Execute(null);

            Assert.AreEqual(3, sut.Seconds, "state Red");
            Assert.AreEqual(State.Red, sut.State, "state Red");

            _timer.OnTick();    // 2
            _timer.OnTick();    // 1

            Assert.AreEqual(1, sut.Seconds, "state Yellow");
            Assert.AreEqual(State.Yellow, sut.State, "state Yellow");

            _timer.OnTick();    // 0

            Assert.AreEqual(0, sut.Seconds, "state Green");
            Assert.AreEqual(State.Green, sut.State, "state Green");

            _timer.OnTick();    // 3

            Assert.AreEqual(3, sut.Seconds, "state Red (restart)");
            Assert.AreEqual(State.Red, sut.State, "state Red (restart)");
        }
        //---------------------------------------------------------------------
        [Test]
        public void Timer_ticks___Beeper_called()
        {
            _configMock
                .Setup(c => c.StartSeconds)
                .Returns(3)
                .Verifiable();

            CountDownViewModel sut = this.CreateSut();

            sut.StartCommand.Execute(null);

            _timer.OnTick();
            _timer.OnTick();
            _timer.OnTick();

            _beeperMock.Verify(b => b.Beep(2), Times.Once());
            _beeperMock.Verify(b => b.Beep(1), Times.Once());
            _beeperMock.Verify(b => b.Beep(0), Times.Once());
        }
    }
}
