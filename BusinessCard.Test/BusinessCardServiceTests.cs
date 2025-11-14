using BusinessCardAPI.Interfaces.Repositories;
using BusinessCardAPI.Interfaces.Services;
using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;
using BusinessCardAPI.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Resources;

namespace BusinessCardAPI.ServiceTests
{
    public class BusinessCardServiceTests
    {
        private readonly IBusinessCardService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IBusinessCardRepository _repository;

        public BusinessCardServiceTests()
        {
            _repository = A.Fake<IBusinessCardRepository>();
            _localizer = A.Fake<IStringLocalizer<SharedResource>>();
            _service = new BusinessCardService(_repository, _localizer);


        }
        [Fact]
        public async Task GetAll_ReturnAll()
        {
            // Arrange
            IEnumerable<BusinessCard> expectedCardList = new List<BusinessCard>
            {
                new BusinessCard("Abd" , "Mael" , new DateTime(1998, 7, 30),"Abd@gmail.com" , "00962788456446" , "Test",null ,DateTime.UtcNow ),
                new BusinessCard("Mohammed" , "Male" , new DateTime(1998, 7, 30),"Mohammed@gmail.com" , "00962777390960" , "Test",null ,DateTime.UtcNow ),
            };


            A.CallTo(() => _repository.GetAll()).Returns(Task.FromResult(expectedCardList));

            // Act
            IEnumerable<BusinessCard> actualCardList =await _service.GetAllCards();

            // Assert
            Assert.True(_repository.GetAll().IsCompletedSuccessfully);
           
            Assert.NotNull(actualCardList);
            Assert.NotEmpty(actualCardList);
            Assert.Equal(expectedCardList.Count(), actualCardList.Count());
            Assert.Equal(expectedCardList.ElementAt(0), actualCardList.ElementAt(0));
            Assert.Equal(expectedCardList.ElementAt(1), actualCardList.ElementAt(1));
        }

        [Fact]

        public async Task GetById_Valid_ReturnCard()
        {
            // Arrange
            int cardId = 1;
            BusinessCard expectedCard = new BusinessCard("Abd", "Male", new DateTime(1998, 7, 30), "Abd@gmail.com", "00962788456446", "Test", null, DateTime.UtcNow)
            {
                Id = 1
            };
            A.CallTo(() => _repository.GetById(cardId)).Returns(Task.FromResult(expectedCard));

            // Act
            BusinessCard? acutalCard = await _service.GetCardById(cardId);

            // Assert
            Assert.True(_repository.GetById(cardId).IsCompletedSuccessfully);

            Assert.NotNull(acutalCard);
            Assert.Equal(cardId, acutalCard.Id);
            Assert.Equal("Abd", acutalCard.Name);
            Assert.Equal("Abd@gmail.com", acutalCard.Email);

        }
        [Fact]
        public async Task GetById_Invalid_ReturnCard()
        {
            // Arrange
            A.CallTo(() => _repository.GetById(1)).Returns(Task.FromResult((BusinessCard?)null));

            // Act
            var acutalCard = await _service.GetCardById(1);

            // Assert
            Assert.True(_repository.GetById(1).IsCompletedSuccessfully);
          
            Assert.Null(acutalCard);
        }

        [Fact]
        public async Task CreateCard_ValidData_ReturnCard()
        {
            // Arrange
            var expectedCard = new BusinessCard("Abd", "Male", new DateTime(1998, 7, 30), "Abd@gmail.com", "00962788456446", "Test", null, DateTime.UtcNow)
            {
                Id = 1
            };
            var createDto = new BusinessCardCreateDto { Name = "Abd", Gender = "Male", DateOfBirth = new DateTime(1998, 7, 30), Email = "Abd@gmail.com", Phone = "00962788456446", Address = "Test" };


            A.CallTo(() => _repository.Create(A<BusinessCard>._)).ReturnsLazily((BusinessCard expectedCardList) => Task.FromResult(expectedCardList));
            // Act
            var acutalCard = await _service.CreateCard(createDto);

            // Assert
            Assert.True(_repository.Create(expectedCard).IsCompletedSuccessfully);
           
            
            Assert.NotNull(acutalCard);
            Assert.Equal("Abd", acutalCard.Name);
            Assert.Equal("Abd@gmail.com", acutalCard.Email);
        }

        [Fact]
        public async Task DeleteCard_ValidId_ReturnTrue()
        {
            // Arrange
            A.CallTo(() => _repository.Delete(1)).Returns(true);

            // Act
            var result = await _service.DeleteCard(1);

            // Assert
            Assert.True(_repository.Delete(1).IsCompletedSuccessfully);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteCard_InvalidId_ReturnFalse()
        {
            // Arrange
            A.CallTo(() => _repository.Delete(1)).Returns(false);

            // Act
            var result = await _service.DeleteCard(1);

            // Assert
            Assert.True(_repository.Delete(1).IsCompletedSuccessfully);
            Assert.False(result);
        }

        [Fact]
        public async Task FilterCards_ByName_ReturnFilteredCards()
        {
            // Arrange
            var filter = new BusinessCardFilterDto { Name = "Abd" };

            var expectedCardList = new List<BusinessCard>
            {
                new BusinessCard("Abd" , "Male" , new DateTime(1998, 7, 30),"Abd@gmail.com" , "00962788456446" , "Test",null ,DateTime.UtcNow ),
            };

            A.CallTo(() => _repository.Filter(filter)).Returns(Task.FromResult((IEnumerable<BusinessCard>)expectedCardList));

            // Act
            var actualCardList = await _service.FilterCards(filter);

            // Assert
            Assert.True(_repository.Filter(filter).IsCompletedSuccessfully);

            Assert.NotNull(actualCardList);
            Assert.Single(actualCardList);
            Assert.Contains("Abd", actualCardList.First().Name);
            Assert.Contains("Abd@gmail.com", actualCardList.First().Email);
        }
    }
}