using AutoMapper;
using TradeAppEntity;
using TradeAppSharedKernel.Application;

namespace TradeAppApplication.Responses.User
{
    public class LoginUserResponse:IMapFrom<Users>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? MiddleName { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<Roles> Roles { get; set; }
        public List<UserPasswords> Passwords { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Users,LoginUserResponse>()
             .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)))
             .ForMember(dest => dest.Passwords,opt => opt.MapFrom(src => src.Passwords));
        }

    }
}
