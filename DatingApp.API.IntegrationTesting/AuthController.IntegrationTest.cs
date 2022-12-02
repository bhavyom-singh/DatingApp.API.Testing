using DatingApp.API.Dtos;
using FluentAssertions;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DatingApp.API.IntegrationTesting
{
    public class AuthControllerIntegrationTest : IntegrationTest
    {
        
        [Theory]
        [InlineData("ra", "ra", "male", "pasedena", "USA", "2000-01-01", "Ra@123")]
        public async Task RegisterUser(string username, string knownAs, string gender, string city, string country, DateTime dob, string password)
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
            //Act
            var response = await Register(userDetail);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Theory]
        [InlineData("ra", "ra", "male", "pasedena", "USA", "2000-01-01", "Ra@123", "ra", "Ra@123")]
        [InlineData("ra", "ra", "male", "pasedena", "USA", "2000-01-01", "Ra@123", "ra1", "Ra@123")]
        public async Task LoginTest(string username, string knownAs, string gender, string city, string country, DateTime dob, string password, string usernameForLogin, string passwordForLogin)
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

            //Act
            await AuthenticateAsync(usernameForLogin, passwordForLogin);

            //Assert
            Assert.NotEmpty(TestClient.DefaultRequestHeaders.Authorization.ToString().Split(" ")[1]);
        }       
        
    }
}
