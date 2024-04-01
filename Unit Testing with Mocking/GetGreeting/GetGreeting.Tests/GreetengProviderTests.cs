using GetGreeting;
using Moq;

namespace GetGreeting.Tests
{
    public class GreetingProviderTests
    {
        private GreetingProvider _greetingProvider;
        private Mock<ITimeProvider> _mockedTimeProvider;

        [SetUp]
        public void Setup()
        {
            _mockedTimeProvider = new Mock<ITimeProvider>();
            _greetingProvider = new GreetingProvider(_mockedTimeProvider.Object);
        }

        [Test]
        public void GetGreeting_ShouldReturnAMorningMessage_WhenItIsMorning()
        {
            // Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2000, 2, 2, 9, 9, 9));

            // Act
            var result = _greetingProvider.GetGreeting();


            // Assert
            Assert.AreEqual("Good morning!", result);
        }

        [Test]
        public void GetGreeting_ShouldReturnAAfternoonMessage_WhenItIsAfternoon()
        {

            // Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2000, 2, 2, 13, 13, 13));


            // Act
            var result = _greetingProvider.GetGreeting();


            // Assert
            Assert.AreEqual("Good afternoon!", result);
        }

        [Test]
        public void GetGreeting_ShouldReturnAEveningMessage_WhenItIsEvening()
        {
            // Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2000, 2, 2, 19, 19, 19));


            // Act
            var result = _greetingProvider.GetGreeting();


            // Assert
            Assert.AreEqual("Good evening!", result);
        }

        [Test]
        public void GetGreeting_ShouldReturnANightMessage_WhenItIsNight()
        {
            // Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2000, 2, 2, 23, 23, 23));


            // Act
            var result = _greetingProvider.GetGreeting();


            // Assert
            Assert.AreEqual("Good night!", result);
        }

        // Всички тестове по нагоре са събрени в един с две променливи в тест кейсове!!!

        [TestCase("Good night!", 4)]
        [TestCase("Good evening!", 19)]
        [TestCase("Good afternoon!", 13)]
        [TestCase("Good morning!", 11)]
        public void GetGreeting_ShouldReturnCorrectMessage_WhenItIsCorrectt(string expectedMessage, int currentHour)
        {
            // Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2000, 2, 2, currentHour, 23, 23));


            // Act
            var result = _greetingProvider.GetGreeting();


            // Assert
            Assert.AreEqual(expectedMessage, result);
        }
    }
}