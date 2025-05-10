using Domain.Data;

namespace Application.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
