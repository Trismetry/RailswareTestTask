using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RailswareTestTask;
using RailswareTestTask.Controllers;
using RailswareTestTask.Interfaces;
using System;

namespace UnitTesting
{
    public class Tests
    {
        private readonly Mock<IMailService> _mockMailService;
        private readonly MailController _mailController;

        public Tests(){
            _mockMailService = new Mock<IMailService>();
            _mailController = new MailController(_mockMailService.Object);
            _mailController.ControllerContext = new ControllerContext();
            _mailController.ControllerContext.HttpContext = new DefaultHttpContext();
            
        }
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestEmailSendCorrect()
        {
            var testData = new MailData();
            testData.SenderNameAndEmail = "T abc@demomailtrap.com";
            testData.RecipientNameAndEmail = "Illya trismetry@gmail.com";
            testData.Subject = "Test 22";
            testData.Body = "T";
            _mockMailService.Setup(s => s.SendMail(testData)).Returns(true);     
            bool res = _mailController.SendMail(testData);
            Assert.IsTrue(res);
        }

        [Test]
        public void TestEmailSendIncorrect()
        {
            //does'nt work with incorrect domain (gmail.com) in this case

            var testData = new MailData();
            testData.SenderNameAndEmail = "T abc@gmail.com";
            testData.RecipientNameAndEmail = "Illya trismetry@gmail.com";
            testData.Subject = "Test 22";
            testData.Body = "T";
            _mockMailService.Setup(s => s.SendMail(testData)).Returns(false);
            bool res = _mailController.SendMail(testData);
            Assert.IsFalse(res);
        }
    }
}