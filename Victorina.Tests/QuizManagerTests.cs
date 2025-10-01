using Moq;
using Victorina.Models;
using Victorina.Services;
using Victorina.Services.Interfaces;
using Xunit;

namespace Victorina.Tests.Services
{
    public class QuizManagerTests
    {
        private readonly QuizManager _quizManager;
        private readonly Mock<IQuizHolder> _mockQuizHolder;

        public QuizManagerTests()
        {
            _quizManager = new QuizManager();
            _mockQuizHolder = new Mock<IQuizHolder>();
        }

        [Fact]
        public void GetCurrentQuestionValidStateReturnsCorrectQuestion()
        {
            // Arrange
            var quizId = "test-quiz";
            var currentIndex = 1;
            var expectedQuestion = new QuestionModel
            {
                ImgUrl = "",
                Title = "TestQuiz",
                Answers = new[] { "Answer 1", "Answer 2", "Answer 3" },
                RightAnswer = 0
            };

            var state = new QuizStateModel
            {
                Id = quizId,
                CurrentIndex = currentIndex,
                QuestionsCount = 3,
                RightCount = 0
            };

            _mockQuizHolder
                .Setup(x => x.GetQuestion(quizId, currentIndex))
                .Returns(expectedQuestion);

            // Act
            var result = _quizManager.GetCurrentQuestion(state, _mockQuizHolder.Object);

            // Assert
            Assert.Equal(expectedQuestion, result);
            _mockQuizHolder.Verify(x => x.GetQuestion(quizId, currentIndex), Times.Once);
        }

        [Fact]
        public void SubmitResultCorrectAnswerIncrementsRightCount()
        {
            // Arrange
            var quizId = "test-quiz";
            var currentIndex = 0;
            var userChoiceIndex = 0; // Correct answer

            var question = new QuestionModel
            {
                Title = "TestQuiz",
                Answers = new[] { "Answer 1", "Answer 2", "Answer 3" },
                RightAnswer = 0
            };

            var state = new QuizStateModel
            {
                Id = quizId,
                CurrentIndex = currentIndex,
                QuestionsCount = 3,
                RightCount = 0
            };

            _mockQuizHolder
                .Setup(x => x.GetQuestion(quizId, currentIndex))
                .Returns(question);

            // Act
            var result = _quizManager.SubmitResult(state, _mockQuizHolder.Object, userChoiceIndex);

            // Assert
            Assert.False(result); // Quiz not finished
            Assert.Equal(1, state.RightCount);
            Assert.Equal(1, state.CurrentIndex);
        }

        [Fact]
        public void SubmitResultWrongAnswerDoesNotIncrementRightCount()
        {
            // Arrange
            var quizId = "test-quiz";
            var currentIndex = 0;
            var userChoiceIndex = 1; // Wrong answer

            var question = new QuestionModel
            {
                Title = "TestQuiz",
                Answers = new[] { "Answer 1", "Answer 2", "Answer 3" },
                RightAnswer = 2
            };

            var state = new QuizStateModel
            {
                Id = quizId,
                CurrentIndex = currentIndex,
                QuestionsCount = 3,
                RightCount = 0
            };

            _mockQuizHolder
                .Setup(x => x.GetQuestion(quizId, currentIndex))
                .Returns(question);

            // Act
            var result = _quizManager.SubmitResult(state, _mockQuizHolder.Object, userChoiceIndex);

            // Assert
            Assert.False(result); // Quiz not finished
            Assert.Equal(0, state.RightCount);
            Assert.Equal(1, state.CurrentIndex);
        }

        [Fact]
        public void SubmitResultLastQuestionReturnsTrue()
        {
            // Arrange
            var quizId = "test-quiz";
            var currentIndex = 2; // Last question index
            var userChoiceIndex = 0;

            var question = new QuestionModel
            {
                Title = "TestQuiz",
                Answers = new[] { "Answer 1", "Answer 2", "Answer 3" },
                RightAnswer = 0
            };

            var state = new QuizStateModel
            {
                Id = quizId,
                CurrentIndex = currentIndex,
                QuestionsCount = 3,
                RightCount = 1
            };

            _mockQuizHolder
                .Setup(x => x.GetQuestion(quizId, currentIndex))
                .Returns(question);

            // Act
            var result = _quizManager.SubmitResult(state, _mockQuizHolder.Object, userChoiceIndex);

            // Assert
            Assert.True(result); // Quiz finished
            Assert.Equal(2, state.RightCount);
            Assert.Equal(3, state.CurrentIndex);
        }

        [Theory]
        [InlineData(0, 3, 0)]  // 0% - 0 
        [InlineData(1, 5, 1)]  // 20% - 1 
        [InlineData(2, 5, 1)]  // 40% - 1 
        [InlineData(4, 5, 2)]  // 80% - 2 
        [InlineData(5, 5, 3)]  // 100% - 3 
        [InlineData(3, 3, 3)]  // 100% - 3 
        public void GetQuizResultVariousScoresReturnsCorrectStars(int rightCount, int questionsCount, int expectedStars)
        {
            // Arrange
            var state = new QuizStateModel
            {
                RightCount = rightCount,
                QuestionsCount = questionsCount,
                CurrentIndex = questionsCount
            };

            // Act
            var result = _quizManager.GetQuizResult(state);

            // Assert
            Assert.Equal(rightCount, result.RightCount);
            Assert.Equal(questionsCount, result.AllCount);
            Assert.Equal(expectedStars, result.StarsCount);
        }

        [Fact]
        public void GetQuizResultPerfectScoreReturnsThreeStars()
        {
            // Arrange
            var state = new QuizStateModel
            {
                RightCount = 10,
                QuestionsCount = 10,
                CurrentIndex = 10
            };

            // Act
            var result = _quizManager.GetQuizResult(state);

            // Assert
            Assert.Equal(10, result.RightCount);
            Assert.Equal(10, result.AllCount);
            Assert.Equal(3, result.StarsCount);
        }

        [Fact]
        public void GetQuizResultAllWrongReturnsZeroStars()
        {
            // Arrange
            var state = new QuizStateModel
            {
                RightCount = 0,
                QuestionsCount = 10,
                CurrentIndex = 10
            };

            // Act
            var result = _quizManager.GetQuizResult(state);

            // Assert
            Assert.Equal(0, result.RightCount);
            Assert.Equal(10, result.AllCount);
            Assert.Equal(0, result.StarsCount);
        }

        [Fact]
        public void GetQuizResultBoundaryCase79PercentReturnsOneStar()
        {
            // Arrange
            var state = new QuizStateModel
            {
                RightCount = 79,
                QuestionsCount = 100,
                CurrentIndex = 100
            };

            // Act
            var result = _quizManager.GetQuizResult(state);

            // Assert
            Assert.Equal(1, result.StarsCount);
        }

        [Fact]
        public void GetQuizResultBoundaryCase80PercentReturnsTwoStars()
        {
            // Arrange
            var state = new QuizStateModel
            {
                RightCount = 80,
                QuestionsCount = 100,
                CurrentIndex = 100
            };

            // Act
            var result = _quizManager.GetQuizResult(state);

            // Assert
            Assert.Equal(2, result.StarsCount);
        }

        [Fact]
        public void GetQuizResultBoundaryCase99PercentReturnsTwoStars()
        {
            // Arrange
            var state = new QuizStateModel
            {
                RightCount = 99,
                QuestionsCount = 100,
                CurrentIndex = 100
            };

            // Act
            var result = _quizManager.GetQuizResult(state);

            // Assert
            Assert.Equal(2, result.StarsCount);
        }
    }
}