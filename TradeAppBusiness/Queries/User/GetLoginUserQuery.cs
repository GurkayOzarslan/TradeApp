using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TradeAppApplication.Responses.User;
using TradeAppSharedKernel.Application;
using TradeAppSharedKernel.Infrastructure.Helpers;

namespace TradeAppApplication.Queries.User
{
    public class GetLoginUserQuery : QueryBase<LoginUserResponse>
    {
        public string Email { get; }
        public string Password { get; }
        public GetLoginUserQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
    public class GetLoginUserQueryHandler : IQueryHandler<GetLoginUserQuery, LoginUserResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public GetLoginUserQueryHandler(IAppDbContext context, IPasswordHasher passwordHasher, IMapper mapper, IConfiguration configuration, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _configuration = configuration;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginUserResponse> Handle(GetLoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Passwords)
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .Where(x => x.Email == request.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return null;

            var password = user.Passwords.Where(x => x.DeletedAt == null).OrderByDescending(p => p.CreatedAt).FirstOrDefault();
            var isValid = _passwordHasher.Verify(request.Password, password?.PasswordHash, password?.Salt);
            if (!isValid)
                return null;

            var response = _mapper.Map<LoginUserResponse>(user);

            var roleNames = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();
            var secretKey = _configuration["Jwt:SecretKey"];

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.Username, user.Name, user.MiddleName ?? "", user.Surname, roleNames);


            response.Token = token;

            return response;
        }
    }
}