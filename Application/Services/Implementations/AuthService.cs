using Application.Services.Interfaces;
using Application.Settings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Data;
using Data.Repositories.Interfaces;
using Domain.Constants;
using Domain.Models.Authentications;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Implementations
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly AppSettings _appSettings;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
            _customerRepository = unitOfWork.Customer;
        }

        public async Task<IActionResult> CustomerAuthenticate(CertificateModel certificate)
        {
            try
            {
                var customer = await _customerRepository.Where(x => x.Username.Equals(certificate.Username)
                                                        && x.Password.Equals(certificate.Password)).FirstOrDefaultAsync();
                if (customer == null)
                {
                    return AppErrors.INVALID_CERTIFICATE.BadRequest();
                }
                if (customer.Status.Equals(CustomerStatuses.INACTIVE))
                {
                    return AppErrors.INVALID_USER_UNACTIVE.BadRequest();
                }
                var auth = _mapper.Map<AuthModel>(customer);
                auth.Role = UserRoles.CUSTOMER;
                var accessToken = GenerateJwtToken(auth);
                return new AuthViewModel
                {
                    AccessToken = accessToken,
                    User = new UserViewModel
                    {
                        Id = customer.Id,
                        Username = customer.Username,
                        Name = customer.Name,
                        Phone = customer.Phone,
                        Role = UserRoles.CUSTOMER,
                        Status = customer.Status,
                        CreateAt = customer.CreateAt,
                    }
                }.Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetCustomerInformation(Guid id)
        {
            try
            {
                var customer = await _customerRepository.Where(st => st.Id.Equals(id))
                                                .ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider)
                                                .FirstOrDefaultAsync();
                return customer != null ? customer.Ok() : AppErrors.USER_NOT_EXIST.NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AuthModel> GetUser(Guid id)
        {
            try
            {
                if (_customerRepository.Any(x => x.Id.Equals(id)))
                {
                    var customer = await _customerRepository
                        .Where(st => st.Id.Equals(id))
                        .ProjectTo<AuthModel>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                    if (customer != null)
                    {
                        customer.Role = UserRoles.CUSTOMER;
                        return customer;
                    }
                }
                return null!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // API use JWT token
        private string GenerateJwtToken(AuthModel auth)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            // Bo cai gi vao token de sau lay ra xac thuc
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", auth.Id.ToString()),
                    new Claim("role", auth.Role.ToString()),
                }),
                Expires = DateTime.Now.AddDays(7),
                // Lay secrect key tu appsetings.json de ma hoa token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
