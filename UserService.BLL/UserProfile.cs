using AutoMapper;
using UserService.Contracts.DTOs;
using UserService.DAL.Entities;

namespace UserService.BLL
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();

            CreateMap<RegisterUserDTO, User>();

            CreateMap<PatchUserDTO, User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); ;
        }

    }
}
