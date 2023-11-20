using WebshopAPI.DTOs;

namespace WebshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;

        private readonly IJwtUtils _jwtUtils;

        public LoginController(ILoginRepository loginRepository, IJwtUtils jwtUtils)
        {
            _loginRepository = loginRepository;
            _jwtUtils = jwtUtils;
  
        }

        private static LoginResponse MapLoginToLoginResponse(Login login)
        {
            LoginResponse response = new LoginResponse
            {
                Id = login.Id,
                UserName = login.UserName,
                Email = login.Email,
                Role = login.Role
            };
            if (login.Customer != null)
            {
                response.Customer = new LoginCustomerResponse
                {
                    Id = login.Customer.Id,
                    Name = login.Customer.Name,
                    PhoneNr = login.Customer.PhoneNr,
                    AddressId = login.Customer.AddressId,
                    LoginId = login.Customer.LoginId,
                };
                if (login.Customer.Address != null)
                {
                    response.Address = new LoginAddressResponse
                    {
                        Id = login.Customer.Address.Id,
                        StreetName = login.Customer.Address.StreetName,
                        Country = login.Customer.Address.Country,
                        ZipCodeId = login.Customer.Address.ZipCodeId
                    };
                    if (login.Customer.Address.ZipCode != null)
                    {
                        response.Address = new LoginAddressResponse
                        {
                            Id = login.Customer.Address.Id,
                            StreetName = login.Customer.Address.StreetName,
                            Country = login.Customer.Address.Country,
                            ZipCodeId = login.Customer.Address.ZipCodeId,
                            City = login.Customer.Address.ZipCode.City
                        };
                    }
                }
            }
            return response;
        }

        public static UserResponse MapUserTouserResponse(Login login)
        {
            return new UserResponse
            {
                Id = login.Id,
                Role = login.Role
            };
        }

        private static Login MapLoginRequestToLogin(LoginRequest loginRequest)
        {
            Login login = new Login
            {
                UserName = loginRequest.UserName,
                Email = loginRequest.Email,
                Role = loginRequest.Role
            };
            if(loginRequest.Password != null && loginRequest.Password != "")
            {
                login.Password = BCrypt.Net.BCrypt.HashPassword(loginRequest.Password);
            }
            return login;
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var logins = await _loginRepository.GetAllAsync();
                List<LoginResponse> loginResponse = logins.Select(
                    login => MapLoginToLoginResponse(login)).ToList();

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        [Route("{loginId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int loginId)
        {
            try
            {
                var login = await _loginRepository.FindByIdAsync(loginId);
                if (login == null)
                {
                    return NotFound();
                }
                return Ok(MapLoginToLoginResponse(login));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerRegisterRequest customerRegisterRequest)
        {
            try
            {
                Login CreateNewCustomer = new Login
                {
                    UserName = customerRegisterRequest.UserName,
                    Email = customerRegisterRequest.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(customerRegisterRequest.Password),
                    Role = Role.User,
                    Customer = new Customer()
                    {
                        Name = customerRegisterRequest.Name,
                        PhoneNr = customerRegisterRequest.PhoneNr,
                        Address = new Address()
                        {
                            StreetName = customerRegisterRequest.StreetName,
                            Country = customerRegisterRequest.Country,
                            ZipCodeId = customerRegisterRequest.ZipCode
                        }
                    }
                };
                var login = await _loginRepository.CreateAsync(CreateNewCustomer);
                LoginResponse loginResponse = MapLoginToLoginResponse(login);


                return Ok(loginResponse);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] LoginRequest loginRequest)
        {
            try
            {
                Login newLogin = MapLoginRequestToLogin(loginRequest);

                var login = await _loginRepository.CreateAsync(newLogin);

                LoginResponse loginResponse = MapLoginToLoginResponse(login);

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // authenticate endpoint || login
        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest login)
        {
            try
            {
                Login user = await _loginRepository.GetByEmail(login.Email);
                if (user == null)
                {
                    return Unauthorized();
                }


                bool PasswordCheck = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
                if (PasswordCheck)
                {
                    SignInResponse SignInResponse = new()
                    {
                        LoginId = user.Id,
                        Role = user.Role,
                        Token = _jwtUtils.GenerateJwtToken(user)
                        
                    };
                    if(user.Customer != null)
                    {
                        SignInResponse.CustomerId = user.Customer.Id;
                        if( user.Customer.Address != null)
                        {
                            SignInResponse.AddressId = user.Customer.Address.Id;

                        };
                    }
                    return Ok(SignInResponse);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut]
        [Route("{loginId}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] int loginId, [FromBody] LoginRequest loginRequest)
        {
            try
            {
                var updateLogin = MapLoginRequestToLogin(loginRequest);

                var login = await _loginRepository.UpdateByIdAsync(loginId, updateLogin);

                if (login == null)
                {
                    return NotFound();
                }

                return Ok(MapLoginToLoginResponse(login));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpDelete]
        [Route("{loginId}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int loginId)
        {
            try
            {
                var login = await _loginRepository.DeleteByIdAsync(loginId);
                if (login == null)
                {
                    return NotFound();
                }
                return Ok(MapLoginToLoginResponse(login));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
