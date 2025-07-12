using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Repository.Interface
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user,List<string> roles);
    }
}
