using CandidateCodeTest.Contracts;
using Moq;
using System;
using Xunit;

namespace CandidateCodeTest
{

    public class CustomerServiceTests
    {
        [Fact]
        public void TrySendEmail_WhenTimeIsRight_SendsEmail()
        {
            // Arrange
            var emailServiceMock = new Mock<IEmailService>();
            var timeProviderMock = new Mock<ITimeRangeService>();
            timeProviderMock.Setup(tp => tp.IsWithinTimeRange(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Returns(true);

            var customerService = new CustomerService(emailServiceMock.Object, timeProviderMock.Object);

            // Act
            var result = customerService.TrySendEmail("test@example.com", "Test", "This is a test", true);

            // Assert
            Assert.True(result);
            emailServiceMock.Verify(es => es.SendEmail("test@example.com", "Test", "This is a test"), Times.Once);
        }

        [Fact]
        public void TrySendEmail_WhenTimeIsWrong_DoesNotSendEmail()
        {
            // Arrange
            var emailServiceMock = new Mock<IEmailService>();
            var timeProviderMock = new Mock<ITimeRangeService>();
            timeProviderMock.Setup(tp => tp.IsWithinTimeRange(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Returns(false);

            var customerService = new CustomerService(emailServiceMock.Object, timeProviderMock.Object);

            // Act
            var result = customerService.TrySendEmail("test@example.com", "Test", "This is a test", true);

            // Assert
            Assert.False(result);
            emailServiceMock.Verify(es => es.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void TrySendEmail_WithoutTimeCheck_SendsEmail()
        {
            // Arrange
            var emailServiceMock = new Mock<IEmailService>();
            var timeProviderMock = new Mock<ITimeRangeService>();
            var customerService = new CustomerService(emailServiceMock.Object, timeProviderMock.Object);

            // Act
            var result = customerService.TrySendEmail("test@example.com", "Test", "This is a test", false);

            // Assert
            Assert.True(result);
            emailServiceMock.Verify(es => es.SendEmail("test@example.com", "Test", "This is a test"), Times.Once);
            timeProviderMock.Verify(tp => tp.IsWithinTimeRange(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()), Times.Never);
        }


        [Fact]
        public void TrySendEmail_WhenEmailServiceFails_ReturnsFalse()
        {
            // Arrange
            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock.Setup(es => es.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                            .Throws(new Exception("SMTP server error"));
            var timeProviderMock = new Mock<ITimeRangeService>();
            timeProviderMock.Setup(tp => tp.IsWithinTimeRange(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Returns(true);

            var customerService = new CustomerService(emailServiceMock.Object, timeProviderMock.Object);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => customerService.TrySendEmail("test@example.com", "Test", "This is a test", true));
            Assert.Equal("SMTP server error", exception.Message);
        }


        [Fact]
        public void TrySendEmail_WithInvalidEmailAddress_DoesNotSendEmail()
        {
            // Arrange
            var emailServiceMock = new Mock<IEmailService>();
            var timeProviderMock = new Mock<ITimeRangeService>();
            timeProviderMock.Setup(tp => tp.IsWithinTimeRange(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Returns(true);

            var customerService = new CustomerService(emailServiceMock.Object, timeProviderMock.Object);

            // Act
            var result = customerService.TrySendEmail("invalid_email", "Test", "This is a test", true);

            // Assert
            Assert.False(result);
            emailServiceMock.Verify(es => es.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }


    }
}
