using ElectronicInventoryWeb.Server.Data;

namespace ElectronicInventoryWeb.Server.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
