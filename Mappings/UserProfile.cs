using AutoMapper;
using UserAuthApi.DTOs;
using UserAuthApi.Models;

namespace UserAuthApi.Mappings;
public class UserProfile: Profile {
    public UserProfile() {
        CreateMap<User, UserDTO>();
    }
}