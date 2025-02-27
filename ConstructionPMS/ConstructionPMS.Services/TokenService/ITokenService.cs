using ConstructionPMS.Domain.Entities;
using System;
using System.Security.Claims;

namespace ConstructionPMS.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        string GenerateToken(ClaimsPrincipal principal);
        string GenerateToken(ClaimsIdentity identity);
    }
}