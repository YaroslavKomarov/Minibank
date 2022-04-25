using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domain.Users.UserValidators;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Domain.Exceptions;
using Minibank.Core.Domains.Users;
using System.Threading;
using System.Linq;
using Xunit;
using Moq;

namespace Minibank.Core.Tests
{
    public class UserServiceTests
    {
        private readonly IUserService userService;

        private readonly FluentValidation.IValidator<User> userValidator;

        private readonly Mock<IUnitOfWork> mockUnitOfWork;

        private readonly Mock<IUserRepository> mockUserRepository;

        private readonly Mock<IBankAccountRepository> mockBankAccountRepository;

        public UserServiceTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUserRepository = new Mock<IUserRepository>();
            mockBankAccountRepository = new Mock<IBankAccountRepository>();

            userValidator = new UserValidator(mockUserRepository.Object, mockBankAccountRepository.Object);

            userService = new UserService(mockUnitOfWork.Object, mockUserRepository.Object, userValidator);
        }

        private void ConfigureValidatorDependencies(bool isLoginUnique, bool isEmailUnique, bool doesAccountExist)
        {
            mockUserRepository
                .Setup(repository => repository.CheckIsLoginUniqueAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(isLoginUnique);
            mockUserRepository
                .Setup(repository => repository.CheckIsEmailUniqueAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(isEmailUnique);

            mockBankAccountRepository
               .Setup(repository => repository.CheckDoesNotBankAccountExistByUserIdAsync(It.IsAny<string>(), CancellationToken.None))
               .ReturnsAsync(doesAccountExist);
        }

        [Fact]
        public async void CreateUserAsync_WithNullUser_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, true, true);

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await userService.CreateUserAsync(null, CancellationToken.None)
            );

            Assert.Equal("ѕользователь не существует", exception.Message);
        }

        [Fact]
        public async void CreateUserAsync_WithNullLogin_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, true, true);

            var user = new User { Login = null, Email = "mock" };

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await userService.CreateUserAsync(user, CancellationToken.None)
            );

            Assert.Equal("Login не должен быть пустым", exception.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void CreateUserAsync_WithNullEmail_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, true, true);

            var user = new User { Login = "mock", Email = null };

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await userService.CreateUserAsync(user, CancellationToken.None)
            );

            Assert.Equal("Email не должен быть пустым", exception.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void CreateUserAsync_WithEmptyEmail_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, true, true);

            var user = new User { Login = "mock", Email = "    " };

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await userService.CreateUserAsync(user, CancellationToken.None)
            );

            Assert.Equal("Email не должен быть пустым", exception.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void CreateUserAsync_WithEmptyLogin_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, true, true);

            var user = new User { Login = " ", Email = "mock" };

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await userService.CreateUserAsync(user, CancellationToken.None)
            );

            Assert.Equal("Login не должен быть пустым", exception.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void CreateUserAsync_WithTooLongLogin_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, true, true);

            var user = new User 
            { 
                Login = "".PadLeft(257, '-'), 
                Email = "mock" 
            };

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await userService.CreateUserAsync(user, CancellationToken.None)
            );

            Assert.Equal("Login не должен превышать 256 символов", exception.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void CreateUserAsync_WithTooLongEmail_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, true, true);

            var user = new User
            {
                Login = "mock",
                Email = "".PadLeft(257, '-'),
            };

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await userService.CreateUserAsync(user, CancellationToken.None)
            );

            Assert.Equal("Email не должен превышать 256 символов", exception.Errors.Last().ErrorMessage);
        }


        [Fact]
        public async void CreateUser_WithNonUniqueLogin_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(false, true, true);

            var user = new User { Login = "mock", Email = "mock" };

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await userService.CreateUserAsync(user, CancellationToken.None)
            );

            Assert.Equal("ѕользователь с таким логином уже зарегистрирован", exception.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void CreateUser_WithNonUniqueEmail_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, false, true);

            var user = new User { Login = "mock", Email = "mock" };

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await userService.CreateUserAsync(user, CancellationToken.None)
            );

            Assert.Equal("ѕользователь с такой почтой уже зарегистрирован", exception.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void CreateUser_WithCorrectArgs_MustNotThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, true, true);

            var expectedId = "id";

            mockUserRepository
                .Setup(repository => repository.CreateUserAsync(It.IsAny<User>(), CancellationToken.None))
                .ReturnsAsync(expectedId);

            var user = new User { Login = "mock", Email = "mock" };

            // ACT, ASSERT
            Assert.Equal(expectedId, await userService.CreateUserAsync(user, CancellationToken.None));
        }

        [Fact]
        public async void DeleteUserByIdAsync_WithOpenBankAccount_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, true, false);

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await userService.DeleteUserByIdAsync("", CancellationToken.None)
            );

            Assert.Equal("Ќевозможно удалить пользовател€ с открытым аккаунтом", exception.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void DeleteUserByIdAsync_WithInvalidUserId_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, true, true);

            mockUserRepository
                .Setup(repository => repository.DeleteUserByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(false);

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
                await userService.DeleteUserByIdAsync("", CancellationToken.None)
            );

            Assert.Equal("ѕользователь с переданным идентификатором не существует", exception.Message);
        }


        [Fact]
        public async void UpdateUserAsync_WithInvalidUserId_ShouldThrowException()
        {
            // ARRANGE
            ConfigureValidatorDependencies(true, true, true);

            mockUserRepository
                .Setup(repository => repository.UpdateUserAsync(It.IsAny<User>(), CancellationToken.None))
                .ReturnsAsync(false);

            var user = new User { Login = "mock", Email = "mock" };

            // ACT, ASSERT
            var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
                await userService.UpdateUserAsync(user, CancellationToken.None)
            );

            Assert.Equal("ѕользователь с переданным идентификатором не существует", exception.Message);
        }
    }
}
