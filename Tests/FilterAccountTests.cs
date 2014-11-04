using NUnit.Framework;
using System;
using System.Linq;
using System.Runtime.Serialization;
using Xero.Api.Core.Model;
using Xero.Api.Core.Model.Types;
using XeroTestJob.Helpers;

namespace XeroTestJob.Tests
{
    [TestFixture]
    class FilterAccountTests : ApiWrapperTest
    {
        /// <summary>
        /// Reads EnumMemberAttribute parameter values of given Enum
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="element">Enum element</param>
        /// <returns>String value of EnumMemberAttribute parameter</returns>
        private static string ToEnumString<T>(T element)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, element);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }

        /// <summary>
        /// Find all accounts with non-empty SystemAccount attribute. Select random one. Verify its SystemAccountType value is in SystemAccount enum value
        /// </summary>
        /// <returns></returns>
        [Test]
        public Account FindSystemAccounts()
        {
            var accounts = Api.Accounts
                .Where("SystemAccount!=NULL").Find().ToList();
            var account = accounts[new System.Random().Next(accounts.Count)];

            Assert.True(Enum.IsDefined(typeof(SystemAccountType), account.SystemAccount));

            return account;
        }

        /// <summary>
        /// Create new Account, filter accounts by all created account fields, verify only one account is found and that created and filtered accounts are the same;
        /// </summary>
        /// <returns></returns>
        [Test]
        public Account FindCreatedAccount()
        {
            var accountData = new CreateAccountTests().GenerateAccountData();
            var createdAccount = Api.Accounts.Create(accountData);

            var filteredAccounts = Api.Accounts.
                Where("Code == \"" + accountData.Code + "\"" +
                " AND Name == \"" + accountData.Name + "\"" +
                " AND Type == \"" + ToEnumString<AccountType>(accountData.Type) + "\"" +
                " AND Description == \"" + accountData.Description + "\"" +
                " AND TaxType == \"" + accountData.TaxType + "\"" +
                " AND EnablePaymentsToAccount == " + accountData.EnablePaymentsToAccount +
                " AND ShowInExpenseClaims == " + accountData.ShowInExpenseClaims)
                .Find()
                .ToList();

            Assert.AreEqual(1, filteredAccounts.Count, "List of filtered accounts contains wrong number of elements");
            AccountHelper.Compare(accountData, filteredAccounts.First());

            return filteredAccounts.First();
        }

    }
}
