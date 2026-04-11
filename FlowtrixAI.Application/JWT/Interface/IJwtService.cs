using FlowtrixAI.Domain.Entities;

namespace FlowtrixAI.Application.JWT.Interface;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(AppUser user);
}
