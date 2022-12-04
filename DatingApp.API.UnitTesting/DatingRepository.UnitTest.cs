using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace DatingApp.API.UnitTesting
{
    public class DatingRepositoryTest
    {
        [Fact]
        public async Task GetUserCountTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("UnitTestDB")
                .Options;
            var dbContext = new DataContext(options);

            Seed.SeedUsers(dbContext); // we are seeding only 10 users.            

            //Act
            var query = new DatingRepository(dbContext);
            var result = await query.GetUserCount();

            //Assert
            Assert.Equal(10, result);
        }
        
    }
}
