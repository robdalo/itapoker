using itapoker.Core.Domain.Models;

namespace itapoker.Core.Services.Interfaces;

public interface ISecurityService
{
    void Redact(Game game);
}