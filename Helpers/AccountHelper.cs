using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.Api.Core.Model;
using Xero.Api.Core.Model.Status;
using Xero.Api.Core.Model.Types;
using NUnit.Framework;

namespace XeroTestJob.Helpers
{
    class AccountHelper : Account
    {
        public static void Compare(Account accountModel, Account accountResponse)
        {
            Assert.AreEqual(accountModel.Code, accountResponse.Code, "Model account code and responce account code are not equal ");
            Assert.AreEqual(accountModel.Type, accountResponse.Type, "Model account type and responce account type are not equal ");
            Assert.AreEqual(accountModel.Description, accountResponse.Description, "Model account description and responce account description are not equal ");
            Assert.AreEqual(accountModel.Name, accountResponse.Name, "Model account name and responce account name are not equal ");

            CheckIntegrity(accountResponse);
        }

        /// <summary>
        /// Check that mandatory string fields are not empty
        /// </summary>
        /// <param name="accountResponse"></param>
        private static void CheckIntegrity(Account accountResponse) 
        {
            Assert.IsNotNullOrEmpty(accountResponse.Id.ToString(), "Account id of created account is null or empty");     
            Assert.IsNotNullOrEmpty(accountResponse.Code, "Code of created account is null or empty");
            Assert.IsNotNullOrEmpty(accountResponse.Name, "Name of created account is null or empty");
        }

        /// <summary>
        /// For debug purposes
        /// </summary>
        /// <param name="account"></param>
        public static void ToString(Account account)
        {
            Console.Out.WriteLine("Bank account number: " + account.BankAccountNumber);
            Console.Out.WriteLine("Class: " + account.Class.ToString());
            Console.Out.WriteLine("Code: " + account.Code);
            Console.Out.WriteLine("Currency code: " + account.CurrencyCode);
            Console.Out.WriteLine("Description: " + account.Description);
            Console.Out.WriteLine("Enable payments to account: " + account.EnablePaymentsToAccount.ToString());
            Console.Out.WriteLine("Has attachments: " + account.HasAttachments.ToString());
            Console.Out.WriteLine("Id: " + account.Id.ToString());
            Console.Out.WriteLine("Name: " + account.Name);
            Console.Out.WriteLine("Reporting code: " + account.ReportingCode);
            Console.Out.WriteLine("Reporting code name: " + account.ReportingCodeName);
            Console.Out.WriteLine("Show in expense claims: " + account.ShowInExpenseClaims.ToString());
            Console.Out.WriteLine("Status: " + account.Status.ToString());
            Console.Out.WriteLine("System account: " + account.SystemAccount.ToString());
            Console.Out.WriteLine("Tax type: " + account.TaxType);
            Console.Out.WriteLine("Type: " + account.Type.ToString());
        }
    }
}
