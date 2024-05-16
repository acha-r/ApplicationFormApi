using ApplicationFormApi.Data;
using ApplicationFormApi.DTO;
using ApplicationFormApi.Models;
using ApplicationFormApi.Services;
using Moq;

namespace Tests
{
    public class ApplicationServiceTest
    {
        private readonly ApplicationServices _service;
        private readonly Mock<IApplicationRepository> _mockApplicantRepository = new Mock<IApplicationRepository>();

        public ApplicationServiceTest()
        {
            _service = new ApplicationServices(_mockApplicantRepository.Object);
        }

        [Fact]
        public async Task LoadApplication_ShouldReturnApplicationForm_WhenApplicationExists()
        {
            //Arrange
            var applicationId = Guid.NewGuid().ToString();
            var applicationName = "Mock Program";
            var applicationDescription = "This is a sample application form";
            List<Question> listOfQuestions = new()
            {
                new Question()
                {
                    Id =  Guid.NewGuid().ToString(),
                    Text = "How are you feeling?",
                    QuestionType = QuestionType.Paragraph
                },
                 new Question()
                 {
                    Id =  Guid.NewGuid().ToString(),
                    Text = "If it's not black, it's white?",
                    QuestionType = QuestionType.YesNo,
                    Choices = new[] {"yes", "no"}
                 }
            };

            ApplicationQuestionResponse applicationResponse = new()
            {
                ApplicationId = applicationId,
                ApplicationName = applicationName,
                ApplicationDescription = applicationDescription,
                Questions = listOfQuestions
            };

            _mockApplicantRepository.Setup(x => x.LoadApplication(applicationId)).ReturnsAsync(applicationResponse);

            //Act
            ApplicationQuestionResponse application = await _service.LoadApplication(applicationId);


            //Assert
            Assert.Equal(applicationId, application.ApplicationId);
        }
    }
}