using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.Api.Core.Model;

namespace XeroTestJob.Helpers
{
    class AttachmentHelper
    {
        public static void CompareFileAndCreatedAttachment(Attachment attachmentFile, Attachment createdAttachment)
        {
            Assert.AreEqual(attachmentFile.ContentLength, createdAttachment.ContentLength);
            Assert.AreEqual(attachmentFile.FileName, createdAttachment.FileName);
            Assert.AreEqual(attachmentFile.IncludeOnline, createdAttachment.IncludeOnline);
            Assert.AreEqual(attachmentFile.MimeType, createdAttachment.MimeType);
        }

        /// <summary>
        /// For debug purposes
        /// </summary>
        /// <param name="attachment"></param>
        public static void ToString(Attachment attachment)
        {
            Console.Out.WriteLine("Content length: " + attachment.ContentLength);
            Console.Out.WriteLine("File Name: " + attachment.FileName);
            Console.Out.WriteLine("Id: " + attachment.Id);
            Console.Out.WriteLine("Include online: " + attachment.IncludeOnline);
            Console.Out.WriteLine("Mime type: " + attachment.MimeType);            
        }
    }
}

