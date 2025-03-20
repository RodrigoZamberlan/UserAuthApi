using System.Security.Claims;

namespace UserAuthApi.Helpers;

public record UserClaimsInfo(int Id, string Name, string Email, string Role) {}

public static class UserClaims
{
    public static UserClaimsInfo GetUserClaimsInfo(ClaimsPrincipal user)
    {
        var id = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var name = user.FindFirst(ClaimTypes.Name)?.Value ?? ""; 
        var email = user.FindFirst(ClaimTypes.Email)?.Value ?? ""; 
        var role = user.FindFirst(ClaimTypes.Role)?.Value ?? "default";
        return new UserClaimsInfo(id, name, email, role);
    }

    public static bool IsAuthorizedOrAdmin(ClaimsPrincipal userLogged, int targetUserId)
    {
        var userClaims = GetUserClaimsInfo(userLogged);
        return userClaims.Id == targetUserId || userClaims.Role == "admin";
    }

    public static bool IsAdmin(ClaimsPrincipal userLogged) {
        var userClaims = GetUserClaimsInfo(userLogged);
        return userClaims.Role == "admin";
    }
}