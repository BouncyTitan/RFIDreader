namespace LAN_UDP_Com_Tests
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void ConvertFromJson_ReturnsCorrectObject()
        {
            var senderIp = "192.168.137.2";
            var targetIp = "192.168.137.5";
            var messageText = "This is a message!";

            var json = $"{{\"SenderIp\":\"{senderIp}\",\"TargetIp\":\"{targetIp}\",\"MessageText\":\"{messageText}\"}}";

            // Arrange
            var message = new Message(senderIp, targetIp, messageText);

            // Act
            message.ConvertFromJson(json);

            // Assert
            Assert.AreEqual(message.SenderIp, senderIp);
            Assert.AreEqual(message.TargetIp, targetIp);
            Assert.AreEqual(message.MessageText, messageText);
        }

        [TestMethod]
        public void ConvertToJson_ReturnsCorrectJson()
        {
            // Arrange
            var senderIp = "192.168.137.2";
            var targetIp = "192.168.137.5";
            var messageText = "This is a message!";

            var message = new Message(senderIp, targetIp, messageText);

            // Act
            var json = message.ConvertToJson();

            // Assert
            Assert.AreEqual(json, $"{{\"SenderIp\":\"{senderIp}\",\"TargetIp\":\"{targetIp}\",\"MessageText\":\"{messageText}\"}}");
        }

        [TestMethod]
        public void IsMessageForMe_ReturnsTrue()
        {
            // Arrange
            var senderIp = "192.168.137.2";
            var targetIp = "192.168.137.5";
            var messageText = "This is a message!";
            var myIp = "192.168.137.5";

            var message = new Message(senderIp, targetIp, messageText);

            // Act
            var isMessageForMe = message.IsMessageForMe(myIp);

            // Assert
            Assert.IsTrue(isMessageForMe);
        }

        [TestMethod]
        public void IsMessageForMe_ReturnsFalse()
        {
            // Arrange
            var senderIp = "192.168.137.2";
            var targetIp = "192.168.137.5";
            var messageText = "This is a message!";
            var myIp = "192.168.137.1";

            var message = new Message(senderIp, targetIp, messageText);

            // Act
            var isMessageForMe = message.IsMessageForMe(myIp);

            // Assert
            Assert.IsFalse(isMessageForMe);
        }
    }
}