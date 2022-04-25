using Minibank.Core.Domain.BankAccounts.BankAccountValidators;
using Minibank.Core.Domain.MoneyTransfersHistory.Repositories;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Core.Domain.BankAccounts.Services;
using Minibank.Core.Domain.Users.UserValidators;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domain.Currency.Services;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domain.Exceptions;
using Minibank.Core.Domains.Users;
using System.Threading;
using System.Linq;
using System;
using Xunit;
using Moq;

namespace Minibank.Core.Tests
{
    public class BankAccountServiceTests
    {
        private readonly IBankAccountService bankAccountService;

        private readonly FluentValidation.IValidator<User> userValidator;

        private readonly FluentValidation.IValidator<BankAccount> bankAccountValidator;

        private readonly Mock<IUnitOfWork> fakeUnitOfWork;

        private readonly Mock<IUserRepository> fakeUserRepository;

        private readonly Mock<IDateTimeProvider> fakeDateTimeProvider;

        private readonly Mock<ICurrencyConverterService> fakeConverter;

        private readonly Mock<IBankAccountRepository> fakeBankAccountRepository;

        private readonly Mock<IMoneyTransferHistoryRepository> fakeMoneyTransferHistoryRepository;

        public BankAccountServiceTests()
        {
            fakeUnitOfWork = new Mock<IUnitOfWork>();
            fakeUserRepository = new Mock<IUserRepository>();
            fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            fakeConverter = new Mock<ICurrencyConverterService>();
            fakeBankAccountRepository = new Mock<IBankAccountRepository>();
            fakeMoneyTransferHistoryRepository = new Mock<IMoneyTransferHistoryRepository>();

            bankAccountValidator = new BankAccountValidator();
            
            userValidator = new UserValidator(fakeUserRepository.Object, fakeBankAccountRepository.Object);

            bankAccountService = new BankAccountService(
               fakeUnitOfWork.Object,
               fakeDateTimeProvider.Object,
               userValidator,
               bankAccountValidator,
               fakeMoneyTransferHistoryRepository.Object,
               fakeBankAccountRepository.Object,
               fakeUserRepository.Object,
               fakeConverter.Object
            );
        }

        [Fact]
        public async void CreateBankAccountAsync_WithInvalidUserId_ShouldThrowException()
        {
            // ARRANGE
            fakeUserRepository
                .Setup(repository => repository.GetUserByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((User)null);

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await bankAccountService.CreateBankAccountAsync("", "rub", CancellationToken.None)
            );

            Assert.Equal("Пользователь не существует", exception.Message);
        }

        [Fact]
        public async void CreateBankAccountAsync_WithInvalidCurrencyCode_ShouldThrowException()
        {
            // ARRANGE
            fakeUserRepository
                .Setup(repository => repository.GetUserByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new User { Id = "", Login = "mock", Email = "mock"});

            var currencyCode = "invalid";

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
                await bankAccountService.CreateBankAccountAsync("", currencyCode, CancellationToken.None)
            );

            Assert.Equal($"{currencyCode} - Недопустимый валютный код", exception.Message);
        }

        [Fact]
        public async void CreateBankAccountAsync_WithLowerCurrencyCode_MustNotThrowException()
        {
            // ARRANGE
            fakeUserRepository
                .Setup(repository => repository.GetUserByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new User { Id = "", Login = "mock", Email = "mock" });

            fakeBankAccountRepository
                .Setup(repository => repository.CreateBankAccountAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync("1");

            var currencyCode = "rub";

            // ACT, ASSERT
            Assert.Equal("1", await bankAccountService.CreateBankAccountAsync("", currencyCode, CancellationToken.None));
        }

        [Fact]
        public async void CloseBankAccountAsync_WithInvalidAccountId_ShouldThrowException()
        {
            // ARRANGE
            fakeBankAccountRepository
                .Setup(repository => repository.GetBankAccountByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((BankAccount)null);

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await bankAccountService.CloseBankAccountByIdAsync("", CancellationToken.None)
            );

            Assert.Equal("Банковский аккаунт не существует", exception.Message);
        }

        [Fact]
        public async void CloseBankAccountAsync_WithNonZeroAmunt_ShouldThrowException()
        {
            // ARRANGE
            fakeBankAccountRepository
                .Setup(repository => repository.GetBankAccountByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new BankAccount { Amount = 100 });

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await bankAccountService.CloseBankAccountByIdAsync("", CancellationToken.None)
            );

            Assert.Equal("Банковский аккаунт должен иметь нулевой баланс для закрытия", exception.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void CloseBankAccountAsync_WithFailedUpdate_ShouldThrowException()
        {
            // ARRANGE
            fakeBankAccountRepository
                .Setup(repository => repository.UpdateBankAccountAsync(It.IsAny<BankAccount>(), CancellationToken.None))
                .ReturnsAsync(false);

            fakeBankAccountRepository
                .Setup(repository => repository.GetBankAccountByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new BankAccount { Amount = 0 });

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
                await bankAccountService.CloseBankAccountByIdAsync("", CancellationToken.None)
            );

            Assert.Equal("Не удалось закрыть банковский аккаунт", exception.Message);
        }

        [Fact]
        public async void CloseBankAccountAsync_WithCorrectArgs_ShouldSetClosingDate()
        {
            // ARRANGE
            var expectedDate = new DateTime(200, 1, 1);
            var expectedAccount = new BankAccount { Amount = 0 };

            fakeBankAccountRepository
                .Setup(repository => repository.UpdateBankAccountAsync(It.IsAny<BankAccount>(), CancellationToken.None))
                .ReturnsAsync(true);

            fakeBankAccountRepository
                .Setup(repository => repository.GetBankAccountByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(expectedAccount);

            fakeDateTimeProvider
                .Setup(provider => provider.Now)
                .Returns(expectedDate);

            // ACT
            await bankAccountService.CloseBankAccountByIdAsync("", CancellationToken.None);

            // ASSERT
            Assert.Equal(expectedDate, expectedAccount.ClosingDate);
        }

        [Fact]
        public async void GetTransferCommissionAsync_WithInvalidAmount_ShouldThrowException()
        {
            // ARRANGE
            fakeBankAccountRepository
                .Setup(repository => repository.GetBankAccountByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new BankAccount());

            decimal amount = -1;

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
                await bankAccountService.GetTransferCommissionAsync(amount, "", "", CancellationToken.None)
            );

            Assert.Equal("Невалидное значение суммы", exception.Message);
        }

        [Fact]
        public async void GetTransferCommissionAsync_WithInvalidIdStrings_ShouldThrowException()
        {
            // ARRANGE
            fakeBankAccountRepository
                .Setup(repository => repository.GetBankAccountByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((BankAccount)null);

            decimal amount = 100;

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await bankAccountService.GetTransferCommissionAsync(amount, "", "", CancellationToken.None)
            );

            Assert.Equal("Банковский аккаунт не существует", exception.Message);
        }

        [Fact]
        public async void GetTransferCommissionAsync_WithEqualUserId_ShouldReturnZeroCommission()
        {
            // ARRANGE
            fakeBankAccountRepository
                .Setup(repository => repository.GetBankAccountByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new BankAccount { UserId = "id", CurrencyCode = "RUB" });

            decimal amount = 100;

            // ACT
            var actual = await bankAccountService.GetTransferCommissionAsync(amount, "", "", CancellationToken.None);

            // ASSERT
            Assert.Equal("0 RUB", actual);
        }


        [Fact]
        public async void GetTransferCommissionAsync_WithDifferentUserId_ShouldReturnCommission()
        {
            // ARRANGE
            fakeBankAccountRepository
                .SetupSequence(repository => repository.GetBankAccountByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new BankAccount { UserId = "id1", CurrencyCode = "RUB" })
                .ReturnsAsync(new BankAccount { UserId = "id2", CurrencyCode = "RUB" });

            decimal amount = 100;

            // ACT
            var actual = await bankAccountService.GetTransferCommissionAsync(amount, "", "", CancellationToken.None);

            // ASSERT
            Assert.Equal("2 RUB", actual);
        }

        [Fact]
        public async void GetTransferCommissionAsync_WithDifferentCurrency_ShouldRoundToHundredths()
        {
            // ARRANGE
            fakeBankAccountRepository
                .SetupSequence(repository => repository.GetBankAccountByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new BankAccount { UserId = "id1", CurrencyCode = "RUB" })
                .ReturnsAsync(new BankAccount { UserId = "id2", CurrencyCode = "RUB" });

            decimal amount = 102;

            // ACT
            var actual = await bankAccountService.GetTransferCommissionAsync(amount, "", "", CancellationToken.None);

            // ASSERT
            Assert.Equal("2,04 RUB", actual);
        }

        [Fact]
        public async void UpdateFundsTransferAsync_WithInvalidAmount_ShouldThrowException()
        {
            // ARRANGE
            fakeBankAccountRepository
               .Setup(repository => repository.GetBankAccountByIdAsync(It.IsAny<string>(), CancellationToken.None))
               .ReturnsAsync(new BankAccount());

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
                await bankAccountService.UpdateFundsTransferAsync(null, "", "", CancellationToken.None)
            );

            Assert.Equal("Невалидное значение суммы", exception.Message);
        }

        [Fact]
        public async void UpdateFundsTransferAsync_WithInvalidIdStrings_ShouldThrowException()
        {
            // ARRANGE
            fakeBankAccountRepository
                .Setup(repository => repository.GetBankAccountByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((BankAccount)null);

            decimal amount = 100;

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await bankAccountService.UpdateFundsTransferAsync(amount, "", "", CancellationToken.None)
            );

            Assert.Equal("Банковский аккаунт не существует", exception.Message);
        }
    }
}
