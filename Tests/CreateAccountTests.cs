using NUnit.Framework;
using System;
using Xero.Api.Core.Model;
using Xero.Api.Core.Model.Types;
using XeroTestJob.Helpers;

namespace XeroTestJob.Tests
{
    [TestFixture]
    public class CreateAccountTests : ApiWrapperTest
    {
        private AccountType getRandomNonBankAccountType()
        {
            Array accountTypes = Enum.GetValues(typeof(AccountType));
            var random = new System.Random();
            var accountType = (AccountType)accountTypes.GetValue(random.Next(accountTypes.Length));

            // these account types throw 'is not a valid account type' error. Not sure if this is expected behaviour or not, couldn't find anything in the documentation
            if (accountType.Equals(AccountType.WagesExpense) || accountType.Equals(AccountType.SuperannuationExpense) || accountType.Equals(AccountType.SuperannuationLiability))
                return AccountType.Current;
            else if (accountType.Equals(AccountType.Bank))
                return AccountType.Current;
            else
                return accountType;
        }

        public Account GenerateAccountData()
        {
            var random = new System.Random();
            var account = new Account
            {
                Code = RandomHelper.GetRandomString(10),
                Name = "Test name " + RandomHelper.GetRandomString(10),
                Type = getRandomNonBankAccountType(),
                Description = "Test description " + RandomHelper.GetRandomString(10),
                TaxType = "NONE",
                EnablePaymentsToAccount = (random.Next(2) == 0),
                ShowInExpenseClaims = (random.Next(2) == 0)
            };

            return account;
        }

        [Test]
        public Account CreateAccount()
        {
            var account = GenerateAccountData();
            Account createdAccount = Api.Create(account);
            AccountHelper.Compare(account, createdAccount);

            return createdAccount;
        }

        [Test]
        public Account CreateBankAccount()
        {
            Account account = new Account
            {
                Code = RandomHelper.GetRandomString(10),
                Type = AccountType.Bank,
                BankAccountNumber = "1234-1234-1234567",
                Name = "Test name " + RandomHelper.GetRandomString(10)
            };

            Account createdAccount = Api.Create(account);

            AccountHelper.Compare(account, createdAccount);

            Assert.IsNullOrEmpty(createdAccount.Description, "Description must be empty for the bank account");
            Assert.IsNotNullOrEmpty(createdAccount.CurrencyCode, "Currency code is empty for the bank account");
            Assert.IsNotNullOrEmpty(createdAccount.ReportingCode, "Reporting code is empty for the bank account");
            Assert.IsNotNullOrEmpty(createdAccount.ReportingCodeName, "Reporting code name is empty for the bank account");

            return createdAccount;
        }

        [Test]
        public Account CreateEmptyAccount()
        {
            try
            {
                Account account = new Account();
                Api.Create(account);
            }
            catch (Xero.Api.Infrastructure.Exceptions.ValidationException e)
            {
                Assert.AreEqual("An account TYPE must be given.", e.ValidationErrors[0].Message);
                Assert.AreEqual(" is not a valid account type", e.ValidationErrors[1].Message);
                return null;
            }
            Assert.Fail("Add new account request didn't throw expected exception when mandatory fields were not provided");
            return null;
        }

        [Test]
        public Account CreateAccountSetAccountId()
        {
            try
            {
                Account account = new Account
                {
                    Code = RandomHelper.GetRandomString(10),
                    Type = AccountType.Overheads,
                    Description = "Consultant charges",
                    Name = "Consultation " + RandomHelper.GetRandomString(10),
                    Id = new System.Guid("12345678-1234-1234-1234-123456789012")
                };
                Api.Create(account);
            }
            catch (Xero.Api.Infrastructure.Exceptions.ValidationException e)
            {
                Assert.AreEqual("AccountID cannot be supplied", e.ValidationErrors[0].Message);
                return null;
            }
            Assert.Fail("PUT account request didn't throw expected exception on set AccountID");
            return null;
        }

        [Test]
        public Account CreateSystemAccount()
        {
            try
            {
                Account account = new Account
                {
                    Code = RandomHelper.GetRandomString(10),
                    SystemAccount = SystemAccountType.BankCurrencyGain,
                    Description = "Consultant charges",
                    Name = "Consultation " + RandomHelper.GetRandomString(10),
                };
                Api.Create(account);
            }
            catch (Xero.Api.Infrastructure.Exceptions.ValidationException e)
            {
                Assert.AreEqual("System accounts may not be created", e.ValidationErrors[0].Message);
                return null;
            }
            Assert.Fail("Method CreateSystemAccount didn't throw expected exception");
            return null;
        }

    }

}