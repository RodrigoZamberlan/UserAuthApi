using System.Security.Claims;

namespace UserAuthApi.Helpers;

public static class UserClaims {
    
    public static (int id, string role) GetUserClaimsInfo(ClaimsPrincipal user) {
        var id = int.Parse(user.FindFirst("sub")?.Value ?? "0");
        var role = user.FindFirst("role")?.Value ?? "default";

        return (id, role);
    }

    public static bool IsAuthorizedOrAdmin(ClaimsPrincipal userLogged, int targetUserId) {
        var (userLoggedId, userLoggedRole) = GetUserClaimsInfo(userLogged);
        return userLoggedId == targetUserId || userLoggedRole == "admin";
    }
}