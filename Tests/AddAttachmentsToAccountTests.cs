using NUnit.Framework;
using System.IO;
using Xero.Api.Core.Model;
using Xero.Api.Core.Model.Types;
using XeroTestJob.Helpers;

namespace XeroTestJob.Tests
{
    class AddAttachmentsToAccountTests : ApiWrapperTest
    {
        private const string ImagePath = @"resources\images\connect_xero_button_blue.png";
        private const string HeavyImagePath = @"resources\images\too_heavy.bmp";
        private const string ElevenImagesPath = @"resources\images\test_image_";

        [Test]
        public Attachment AddAttachmentToAccount()
        {
            var account = new CreateAccountTests().CreateAccount();
            var attachmentFile = new Attachment(new FileInfo(ElevenImagesPath + "2.png"));

            var attachment = Api.Attachments.AddOrReplace(attachmentFile, AttachmentEndpointType.Accounts, account.Id);

            AttachmentHelper.CompareFileAndCreatedAttachment(attachmentFile, attachment);

            account = Api.Accounts.Find(account.Id);

            Assert.AreEqual(true, account.HasAttachments, "Account HasAttachments attribute is false after adding an attachment");
            return attachment;
        }

        [Test]
        public Attachment AddAttachmentHeavyFile()
        {
            try
            {
                var account = new CreateAccountTests().CreateAccount();
                var attachmentFile = new Attachment(new FileInfo(HeavyImagePath));

                var attachment = Api.Attachments.AddOrReplace(attachmentFile, AttachmentEndpointType.Accounts, account.Id);
            }
            catch (Xero.Api.Infrastructure.Exceptions.ValidationException e)
            {
                Assert.AreEqual("The attachment must be less than 3MB in size.", e.ValidationErrors[0].Message);
                return null;
            }
            Assert.Fail("Add attachment to account request didn't throw expected exception when file size is > 3MB");
            return null;
        }

        [Test]
        public Attachment Add11AttachmentsToAccount()
        {
            try{
            var account = new CreateAccountTests().CreateAccount();

            for (var i = 1; i < 12; i++)
            {
                var attachmentFile = new Attachment(new FileInfo(ElevenImagesPath + i.ToString() + ".png"));
                var attachment = Api.Attachments.AddOrReplace(attachmentFile, AttachmentEndpointType.Accounts, account.Id);
            }

            }
            catch (Xero.Api.Infrastructure.Exceptions.ValidationException e)
            {
                Assert.AreEqual("The file couldn't be uploaded because the API limit of 10 attachments has been exceeded for the document.", e.ValidationErrors[0].Message);
                return null;
            }
            Assert.Fail("Add attachment to account request didn't throw expected exception on adding 11 files to 1 account");
            return null;
        }
    }
}
