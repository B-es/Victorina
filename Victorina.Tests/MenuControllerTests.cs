using Microsoft.AspNetCore.Mvc;
using Victorina.Controllers;
using Victorina.Models;
using Moq;
using Xunit;
using Victorina.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;

namespace Victorina.Tests.Controllers
{
    public class MenuControllerTests
    {
        private readonly Mock<IVictorinaHolder> _mockVictorinaHolder;
        private readonly List<VictorinaModel> _testQuestions;
        private MenuController _controller;

        public MenuControllerTests()
        {
            // Выполняется перед КАЖДЫМ тестом
            _mockVictorinaHolder = new Mock<IVictorinaHolder>();
            _testQuestions = GetTestVics();

            _mockVictorinaHolder.Setup(x => x.Models).Returns(_testQuestions);

            _controller = new MenuController(_mockVictorinaHolder.Object)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            };
        }


        [Fact]
        public void IndexReturnsAViewResultWithAListOfVics()
        {

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<VictorinaModel>>(viewResult.Model);
            Assert.Equal(GetTestVics().Count, model.Count());
        }

        [Fact]
        public void StartVictorinaWithValidId()
        {
            // Act
            var result = _controller.StartVictorina("animal");

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("SetQuestions", redirectResult.ActionName);
        }

        private List<VictorinaModel> GetTestVics()
        {
            var vics = new List<VictorinaModel>
            {
                new VictorinaModel { Id = "animal", ImgUrl = "", Title = "Title1"},
                new VictorinaModel { Id = "plants", ImgUrl = "", Title = "Title2"},
                new VictorinaModel { Id = "mecha", ImgUrl = "", Title = "Title3"},
                new VictorinaModel { Id = "tests", ImgUrl = "", Title = "Title4"}
            };
            return vics;
        }
    }
}