using AutoMapper;
using BusinessObject.Mapping;
using BusinessObject.Models.JwtTokenModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataAccess
{
    public class AuthDAO
    {
        private readonly IMapper _mapper;
        private readonly JwtConfig _jwtConfig;

        private static AuthDAO instance;
        private static readonly object instancelock = new object();
        private AuthDAO(IMapper mapper,JwtConfig jwtConfig)
        {
            _mapper = mapper;
            _jwtConfig = jwtConfig;
        }

        public static AuthDAO Instance
        {
            get
            {
                lock (instancelock)
                {
                    if (instance == null)
                    {
                        var mapper = GetMapper();

                        var jwtConfig = GetJwtConfig();

                        instance = new AuthDAO(mapper, jwtConfig);
                    }
                    return instance;
                }
            }
        }

        private static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingConfig>());
            return config.CreateMapper();
        }

        private static JwtConfig GetJwtConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();

            var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>();
            return jwtConfig;
        }

        public string GenerateToken(TokenModels model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(CustomerClaimTypes.UserID, model.UserID.ToString()),
                new Claim(CustomerClaimTypes.UserName, model.UserName),
                new Claim(CustomerClaimTypes.Email, model.Email),
                new Claim(CustomerClaimTypes.RoleID, model.RoleID.ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private TokenModels LoginAsync(LoginModel loginUser)
        {
            var db = new CarRentingDBContext();
            var user = db.Users.FirstOrDefault(x => x.Email == loginUser.Email && x.Password == loginUser.Password);
            if (user == null)
            {
                throw new KeyNotFoundException("User with this Email or Password not found");
            }
            var tokenizedData = _mapper.Map<TokenModels>(user);
            return tokenizedData;
        }

        public string Login(LoginModel model)
        {
            var tokenModel = LoginAsync(model);
            var token = GenerateToken(tokenModel);
            return token;
        } 
    }
}
