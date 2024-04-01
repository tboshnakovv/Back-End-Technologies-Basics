namespace MyArrayExample.Tests
{
    public class MyArrayTests
    {
        
        [Test]
        public void Replase_ShouldReplace_IfPositionIsValid()
        {
            // Arrange
            var myArray = new MyArray(20);

            // Act
            var result = myArray.Replace(0, 100);

            // Assert
            Assert.IsTrue(result);
            Assert.That(myArray.Array[0], Is.EqualTo(100));
        }

        [Test]
        public void Replase_ShouldReplace_IfPositionIsLessThanZero()
        {
            // Arrange
            var myArray = new MyArray(20);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => myArray.Replace(-20, 2));
        }

        [Test]
        public void Replase_ShouldReplace_IfPositionIsBiggerThanTheSizeOfTheArray()
        {
            // Arrange
            var myArray = new MyArray(20);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => myArray.Replace(100, 2));
        }

        [Test]
        public void FindMax_ShouldReturnMaxValueFromTheArray()
        {
            // Arrange
            var myArray = new MyArray(20);

            // Act
            var result = myArray.FindMax();

            // Assert
            Assert.That(result, Is.EqualTo(19));
        }
        [Test]
        public void FindMax_ShouldReturnThtowExceptionIfArrayIsEmpty()
        {
            // Arrange
            var myArray = new MyArray(0);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => myArray.FindMax());
        }
    }
}