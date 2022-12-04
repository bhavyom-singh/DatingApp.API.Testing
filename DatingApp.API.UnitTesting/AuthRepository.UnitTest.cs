using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;


namespace DatingApp.API.UnitTesting
{
    public class AuthRepositoryTest
    {
        [Theory]
        [InlineData("lola", true)]
        [InlineData("ra", false)]
        public async Task UserExistsTest(string username, bool usernameExists)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;
            var dbContext = new DataContext(options);

            Seed.SeedUsers(dbContext);

            //Act
            var query = new AuthRepository(dbContext);
            var result = await query.UserExists(username);

            //Assert
            Assert.True(usernameExists == result);
        }
    }
}
