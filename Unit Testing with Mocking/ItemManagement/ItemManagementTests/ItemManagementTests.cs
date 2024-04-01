using NUnit.Framework;
using Moq;
using ItemManagementApp.Services;
using ItemManagementLib.Repositories;
using ItemManagementLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace ItemManagement.Tests
{
    [TestFixture]
    public class ItemServiceTests
    {
        private ItemService _itemService;
        private Mock<IItemRepository> _mockItemReposistory;


        [SetUp]
        public void Setup()
        {
            // Arrange: Create a mock instance of IItemRepository
            _mockItemReposistory = new Mock<IItemRepository>();

            // Instantiate ItemService with the mocked repository
            _itemService = new ItemService(_mockItemReposistory.Object);
        }

        // Add Item Functin tests
        [Test]
        public void AddItem_ShouldItemItem_IfNameIsValid()
        {
            // Arrange
            var item = new Item { Name = "Test Item" };
            _mockItemReposistory.Setup(x => x.AddItem(It.IsAny<Item>()));

            // Act
            _itemService.AddItem(item.Name);

            // Assert
            _mockItemReposistory.Verify(x => x.AddItem(It.IsAny<Item>()), Times.Once());
        }

        [Test]
        public void AddItem_ShouldThrowError_IfNameIsInvalid()
        {
            // Arrange
            string invalidname = "";
            _mockItemReposistory.Setup(x => x.AddItem(It.IsAny<Item>())).Throws<ArgumentException>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _itemService.AddItem(invalidname));
            _mockItemReposistory.Verify(x => x.AddItem(It.IsAny<Item>()), Times.Once());
        }

        // Get Item Function tests
        [Test]
        public void GetAllItems_ShouldReturnAllItems()
        {
            // Arrange
            var items = new List<Item>() { new Item { Id = 1, Name = "SampleItem", } };
            _mockItemReposistory.Setup(x => x.GetAllItems()).Returns(items);

            // Act
            var result = _itemService.GetAllItems();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            _mockItemReposistory.Verify(x => x.GetAllItems(), Times.Once());
        }


        [Test]
        public void GetItemById_ShouldReturnItemById_IfItemeExist()
        {
            // Arrange
            var item = new Item { Id = 1, Name = "Singele  Item" };
            _mockItemReposistory.Setup(x => x.GetItemById(item.Id)).Returns(item);

            // Act
            var result = _itemService.GetItemById(item.Id);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(item.Name));
            _mockItemReposistory.Verify(x => x.GetItemById(item.Id), Times.Once());
        }

        [Test]
        public void GetItemById_ShouldReturnNull_IfItemeDoesNotExist()
        {
            // Arrange
            
            _mockItemReposistory.Setup(x => x.GetItemById(It.IsAny<int>())).Returns<Item>(null);

            // Act
            var result = _itemService.GetItemById(123);

            // Assert
            Assert.Null(result);
            _mockItemReposistory.Verify(x => x.GetItemById(It.IsAny<int>()), Times.Once());
        }

        // Update Item Function Tests
        [Test]
        public void UpdateItem_ShouldNotUpdateItem_IfItemDoesNotExist()
        {
            // Arrange
            var nonExistingId = 1;
            _mockItemReposistory.Setup(x => x.GetItemById(nonExistingId)).Returns<Item>(null);
            _mockItemReposistory.Setup(x => x.UpdateItem(It.IsAny<Item>()));

            // Act
            _itemService.UpdateItem(nonExistingId, "DoesNotMatter");

            // Assert
            _mockItemReposistory.Verify(x => x.GetItemById(nonExistingId), Times.Once());
            _mockItemReposistory.Verify(x => x.UpdateItem(It.IsAny<Item>()), Times.Never);
        }

        [Test]
        public void UpdateItem_ShouldThrowException_IfNameIsInvalid()
        {
            // Arrange
            var item = new Item { Name = "Sample Item", Id = 1 };
            _mockItemReposistory.Setup(x => x.GetItemById(item.Id)).Returns(item);
            _mockItemReposistory.Setup(x => x.UpdateItem(It.IsAny<Item>())).Throws<ArgumentException>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _itemService.UpdateItem(item.Id, ""));

           
            _mockItemReposistory.Verify(x => x.GetItemById(item.Id), Times.Once());
            _mockItemReposistory.Verify(x => x.UpdateItem(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public void UpdateItem_ShouldUpdateItem_IfNameIsValid()
        {
            // Arrange
            var item = new Item { Name = "Sample Item", Id = 1 };
            _mockItemReposistory.Setup(x => x.GetItemById(item.Id)).Returns(item);
            _mockItemReposistory.Setup(x => x.UpdateItem(It.IsAny<Item>()));

            // Act 
            _itemService.UpdateItem(item.Id, "Sample Item Update");

            // Assert
            _mockItemReposistory.Verify(x => x.GetItemById(item.Id), Times.Once());
            _mockItemReposistory.Verify(x => x.UpdateItem(It.IsAny<Item>()), Times.Once);
        }

        // Delete function tests
        [Test]
        public void DeleteItem_ShouldDeleteItem()
        {
            // Arrange
            var itemId= 12;
            _mockItemReposistory.Setup(x => x.DeleteItem(itemId));

            // Act
            _itemService.DeleteItem(itemId);

            // Assert
            _mockItemReposistory.Verify(x => x.DeleteItem(itemId), Times.Once());
        }

        // Validate ItemName function Tests
        [TestCase ("", false)]
        [TestCase (null, false)]
        [TestCase ("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
        [TestCase ("A", true)]
        [TestCase ("Samplename", true)]
        [TestCase ("Sample", true)]
        public void ValidateItemName_ShouldReturnCorrectAnswer_IfIthemNameIsValid(string name, bool isValid)
        {
            

            // Act
            var result = _itemService.ValidateItemName(name);

            // Assert
            Assert.That(result, Is.EqualTo(isValid));
        }
    }  
}