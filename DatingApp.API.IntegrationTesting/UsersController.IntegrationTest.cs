using System;
using DatingApp.API.Dtos;
using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DatingApp.API.IntegrationTesting
{
    public class UsersControllerIntegrationTest : IntegrationTest
    {
        private string getUserUrl = "https://localhost:44366/api/users?orderBy=lastActive";
        
        [Theory]
        [InlineData("ra", "ra", "male", "pasedena", "USA", "2000-01-01", "Ra@123")]
        public async Task GetUsers(string username, string knownAs, string gender, string city, string country, DateTime dob, string password)
        {
            //Arrange
            var userDetail = new UserForRegisterDto
            {
                City = city,
                Country = country,
                DateOfBirth = dob,
                Gender = gender,
                KnownAs = knownAs,
                Password = password,
                Username = username
            };
            await Register(userDetail);
            await AuthenticateAsync(username, password);

            //Act
            var response = await TestClient.GetAsync(getUserUrl);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).Should().BeEquivalentTo("[]");
        }
    }
}
