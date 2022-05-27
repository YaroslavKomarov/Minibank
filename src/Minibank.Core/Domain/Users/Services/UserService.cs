using ValidationException = Minibank.Core.Domain.Exceptions.ValidationException;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domain.Exceptions;
using System.Threading.Tasks;
using FluentValidation;
using System.Threading;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IUserRepository userRepository;

        private readonly IValidator<User> userValidator;

        public UserService(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IValidator<User> userValidator)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.userValidator = userValidator;
        }

        public async Task<string> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            userValidator.ValidateAndThrow(user, options => options.IncludeRuleSets("Create"));

            var userId = await userRepository.CreateUserAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync();
            return userId;
        }

        public async Task DeleteUserByIdAsync(string id, CancellationToken cancellationToken)
        {
            userValidator.ValidateAndThrow(
                new User { Id = id },
                options => options.IncludeRuleSets("Delete"));

            if (!await userRepository.DeleteUserByIdAsync(id, cancellationToken))
            {
                throw new ValidationException("Пользователь с переданным идентификатором не существует");
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            if (!await userRepository.UpdateUserAsync(user, cancellationToken))
            {
                throw new ValidationException("Пользователь с переданным идентификатором не существует");
            }

            await unitOfWork.SaveChangesAsync();
        }
    }
}
