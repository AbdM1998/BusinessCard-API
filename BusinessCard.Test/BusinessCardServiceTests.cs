using BusinessCardAPI.Interfaces.Services;
using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCard.Test
{
    public class BusinessCardServiceTests
    {
        private readonly IBusinessCardService _service;

        public BusinessCardServiceTests()
        {
            _service = A.Fake<IBusinessCardService>();
        }
        [Fact]
        public async Task GetAll_ReturnAll()
        {
            // Arrange
            var cardsList = new List<BusinessCardAPI.Models.Entities.BusinessCard>
            {
                new BusinessCardAPI.Models.Entities.BusinessCard("Abd" , "Mael" , new DateTime(1998, 7, 30),"Abd@gmail.com" , "00962788456446" , "Test",null ,DateTime.UtcNow ),
                new BusinessCardAPI.Models.Entities.BusinessCard("Mohammed" , "Male" , new DateTime(1998, 7, 30),"Mohammed@gmail.com" , "00962777390960" , "Test",null ,DateTime.UtcNow ),
            };

            A.CallTo(() => _service.GetAllCards()).Returns(Task.FromResult((IEnumerable<BusinessCardAPI.Models.Entities.BusinessCard>)cardsList));

            // Act
            var cards = await _service.GetAllCards();

            // Assert
            Assert.NotNull(cards);
            Assert.NotEmpty(cards);
            Assert.Equal(2, cards.Count());
        }

        [Fact]

        public async Task GetById_Valid_ReturnCard()
        {
            // Arrange
            int cardId = 1;
            var card = new BusinessCardAPI.Models.Entities.BusinessCard("Abd", "Male", new DateTime(1998, 7, 30), "Abd@gmail.com", "00962788456446", "Test", null, DateTime.UtcNow)
            {
                Id = 1
            };
            A.CallTo(() => _service.GetCardById(cardId)).Returns(Task.FromResult(card));

            // Act
            var result = await _service.GetCardById(cardId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cardId, result.Id);
            Assert.Equal("Abd", result.Name);
            Assert.Equal("Abd@gmail.com", result.Email);

        }
        [Fact]
        public async Task GetById_Invalid_ReturnCard()
        {
            // Arrange
            A.CallTo(() => _service.GetCardById(1)).Returns(Task.FromResult((BusinessCardAPI.Models.Entities.BusinessCard?)null));

            // Act
            var result = await _service.GetCardById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateCard_ValidData_ReturnCard()
        {
            // Arrange
            var card = new BusinessCardAPI.Models.Entities.BusinessCard("Abd", "Male", new DateTime(1998, 7, 30), "Abd@gmail.com", "00962788456446", "Test", null, DateTime.UtcNow)
            {
                Id = 1
            };
            var createDto = new BusinessCardCreateDto { Name = "Abd", Gender = "Male", DateOfBirth = new DateTime(1998, 7, 30), Email = "Abd@gmail.com", Phone = "00962788456446", Address = "Test" };


            A.CallTo(() => _service.CreateCard(createDto)).Returns(card);

            // Act
            var result = await _service.CreateCard(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Abd", result.Name);
        }

        [Fact]
        public async Task DeleteCard_ValidId_ReturnTrue()
        {
            // Arrange
            A.CallTo(() => _service.DeleteCard(1)).Returns(true);

            // Act
            var result = await _service.DeleteCard(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteCard_InvalidId_ReturnFalse()
        {
            // Arrange
            A.CallTo(() => _service.DeleteCard(1)).Returns(false);

            // Act
            var result = await _service.DeleteCard(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task FilterCards_ByName_ReturnFilteredCards()
        {
            // Arrange
            var filter = new BusinessCardFilterDto { Name = "Abd" };

            var cardsList = new List<BusinessCardAPI.Models.Entities.BusinessCard>
            {
                new BusinessCardAPI.Models.Entities.BusinessCard("Abd" , "Male" , new DateTime(1998, 7, 30),"Abd@gmail.com" , "00962788456446" , "Test",null ,DateTime.UtcNow ),
            };

            A.CallTo(() => _service.FilterCards(filter)).Returns(Task.FromResult((IEnumerable<BusinessCardAPI.Models.Entities.BusinessCard>)cardsList));

            // Act
            var result = await _service.FilterCards(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Abd", result.First().Name);
            Assert.Contains("Abd@gmail.com", result.First().Email);
        }
    }
}