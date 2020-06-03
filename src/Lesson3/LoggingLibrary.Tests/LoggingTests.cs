using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;

namespace LoggingLibrary.Tests
{
    public class LoggingTests
    {
        private Mock<ILoggingConfiguration> _logconfig = new Mock<ILoggingConfiguration>();
        private Mock<IMessageBodyGenerator> _msgBodyGenerator = new Mock<IMessageBodyGenerator>();
        private Mock<IMessageHeaderGenerator> _msgHeaderGen = new Mock<IMessageHeaderGenerator>();
        private Mock<ISensitiveDataScruber> _sensData = new Mock<ISensitiveDataScruber>();

        private readonly Logger _logger;

        public LoggingTests()
        {
            _logger = new Logger(_logconfig.Object, _msgHeaderGen.Object, _msgBodyGenerator.Object, _sensData.Object); 
        }

        [Fact]
        public void Logger_test()
        {
            //arrange
            _msgHeaderGen.Setup(x => x.CreateHeader(It.IsAny<LogLevel>()));
            _msgBodyGenerator.Setup(x => x.CreateBody(It.IsAny<string>()));

            string msg = "Hello";
            var level = LogLevel.Info;

            //act
            _logger.CreateEntry(msg, level);

            //assert
            _msgHeaderGen.Verify(x => x.CreateHeader(level));
            _logconfig.Verify(x => x.LogStackFor(level));
        }
        [Fact]
        public void Logger_CreateEntry_HeaderCreated()
        {
            _msgHeaderGen.Setup(x => x.CreateHeader(It.IsAny<LogLevel>()));

            _logger.CreateEntry("Test log", LogLevel.Error);

            _msgHeaderGen.VerifyAll();

        }

        [Fact]
        public void Logger_CreateEntry_BodyGeneratedCreated()
        {
            _msgBodyGenerator.Setup(x => x.CreateBody(It.IsAny<string>()));

            _logger.CreateEntry("Test log", LogLevel.Error);

            _msgBodyGenerator.VerifyAll();
        }
    }
}
