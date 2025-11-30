using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;
using UserService.DAL.Entities;

namespace Infrastructure.Tests
{
    [TestClass()]
    public class JwtProviderTests
    {
        private JwtOptions jwtOptions;
        private JwtProvider jwtProvider;
        private User user;

        [TestInitialize]
        public void Initialize()
        {
            user = new User()
            {
                Name = "user1",
                Email = "example@test.com",
                RoleId = 1,
                Role = new Role() { Name = "User" }
            };

            jwtOptions = new JwtOptions()
            {
                ExpireHours = 192,
                ExpireMinutesForEmailConfirmation = 30,
                ExpireMinutesForPasswordConfirmation = 30,
                SecretKey = "ssssssssssssssssssssssssssssssssssssssssssssssssssssssfsfafasffafafa",
                SecretKeyForEmailConfirmation = "sssssssssssssssssssssssssssfsssaffssssssssssssssssssssssssfsfafasffafafa",
                SecretKeyForPasswordConfirmation = "ssssss2fsafssssassssssssssssssfsssaffssssssssssssssssssssssssssfsfafasffafafa",
            };

            jwtProvider = new JwtProvider(new OptionsWrapper<JwtOptions>(jwtOptions));
        }

        [TestMethod()]
        public async Task ValidateTokenAsyncTest_ValidToken_ReturnsValidResult()
        {
            Debug.WriteLine(user.Name + " " + user.Email);
            Debug.WriteLine(jwtOptions.SecretKey);

            string token = jwtProvider.GenerateToken(user.Email, user.Id.ToString(), user.Role.Name);

            var result = await jwtProvider.ValidateTokenAsync(token);

            Assert.IsTrue(result?.IsValid);
        }

        [TestMethod()]
        public async Task ValidateTokenAsyncTest_ExpiredToken_ReturnsInvalidResult()
        {
            jwtOptions.ExpireHours = -20;
            jwtProvider = new JwtProvider(new OptionsWrapper<JwtOptions>(jwtOptions));

            string token = jwtProvider.GenerateToken(user.Email, user.Id.ToString(), user.Role.Name);

            var result = await jwtProvider.ValidateTokenAsync(token);

            Debug.WriteLine(result?.Exception.Message);
            Assert.IsFalse(result.IsValid, "Expired token should not be valid");
        }

        [TestMethod()]
        public async Task ValidateTokenAsyncTest_InvalidSignature_ReturnsInvalidResult()
        {
            string token = jwtProvider.GenerateToken(user.Email, user.Id.ToString(), user.Role.Name);

            jwtOptions.SecretKey = "wrong_ssssss2fsafssssassssssssssssssfsssaffssssssssssssssssssssssssssfsfafasffafafa";
            jwtProvider = new JwtProvider(new OptionsWrapper<JwtOptions>(jwtOptions));

            var result = await jwtProvider.ValidateTokenAsync(token);

            Assert.IsFalse(result.IsValid);
        }
    }
}