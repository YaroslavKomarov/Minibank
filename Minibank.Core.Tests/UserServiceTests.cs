using Minibank.Core.Domains.Users.Services;
using Xunit;

namespace Minibank.Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CreateUser_WithNullLogin_ShouldThrowException()
        {
            //var service = new UserService();
        }

        [Fact]
        public void CreateUser_WithNullEmail_ShouldThrowException()
        {

        }

        [Fact]
        public void CreateUser_WithNonUniqueLogin_ShouldThrowException()
        {

        }

        [Fact]
        public void CreateUser_WithNonUniqueEmail_ShouldThrowException()
        {

        }
    }
}
