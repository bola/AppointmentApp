using BlitzPrevair.Core.Entities;

namespace BlitzPrevair.API.Services;

public interface ITokenService
{
    string GenerateToken(Customer customer);
    DateTime GetTokenExpiration();
}
