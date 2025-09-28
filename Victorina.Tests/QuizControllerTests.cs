using Microsoft.AspNetCore.Mvc;
using Victorina.Controllers;
using Victorina.Models;
using Moq;
using Xunit;
using Victorina.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Victorina.Services;
using System.Text.Json;
using System.Text;

namespace Victorina.Tests
{
    public class QuizControllerTests
    {

        private QuizStateModel GetState()
        {
            return new QuizStateModel() { CurrentIndex = 0, Id = "animal", QuestionsCount = 3, RightCount = 0 };
        }

        private QuestionModel GetQuestionModel()
        {
            return new QuestionModel()
            {
                Answers = ["1", "2", "3", "4"],
                ImgUrl = "",
                RightAnswer = 3,
                Title = "Title1"
            };
        }

        private QuizController CreateControllerWithSession(QuizStateModel state = null)
        {
            var mockQuizHolder = new Mock<IQuizHolder>();
            var mockQuizManager = new Mock<QuizManager>();

            // Настройка моков
            mockQuizHolder.Setup(x => x.GetQuestionCount("animal")).Returns(1);
            mockQuizHolder.Setup(x => x.GetQuestions("animal"))
                         .Returns(new List<QuestionModel> { GetQuestionModel() });
            mockQuizHolder.Setup(x => x.GetQuestion("animal", 0))
                         .Returns(GetQuestionModel());

            if (state != null)
            {
                mockQuizManager.Setup(x => x.GetCurrentQuestion(It.IsAny<QuizStateModel>(), mockQuizHolder.Object))
                              .Returns(GetQuestionModel());
            }

            // Создаем HTTP контекст с реальной сессией
            var httpContext = new DefaultHttpContext();
            var session = new TestSession();

            // Сохраняем состояние в сессии
            if (state != null)
            {
                var json = JsonSerializer.Serialize(state);
                var bytes = Encoding.UTF8.GetBytes(json);
                session.Set("QuizState", bytes);
            }

            httpContext.Session = session;

            var controller = new QuizController(mockQuizManager.Object, mockQuizHolder.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext },
                TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
            };

            return controller;
        }

        [Fact]
        public void IndexWithValidSessionReturnsViewResultWithQuestion()
        {
            // Arrange
            var state = GetState();
            var controller = CreateControllerWithSession(state);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<QuestionModel>(viewResult.Model);
            Assert.NotNull(model);
            Assert.Equal("Title1", model.Title);
        }

        [Fact]
        public void IndexWithoutSessionReturnsViewResult()
        {
            // Arrange
            var controller = CreateControllerWithSession(); // без состояния

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }

    // Реализация тестовой сессии
    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _storage = new Dictionary<string, byte[]>();

        public string Id => "TestSessionId";
        public bool IsAvailable => true;
        public IEnumerable<string> Keys => _storage.Keys;

        public void Clear() => _storage.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _storage.Remove(key);

        public void Set(string key, byte[] value) => _storage[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _storage.TryGetValue(key, out value);
    }
}