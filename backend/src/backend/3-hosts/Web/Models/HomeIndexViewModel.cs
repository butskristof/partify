using System.Security.Claims;

namespace Web.Models;

public class HomeIndexViewModel
{
    public bool IsAuthenticated { get; set; }
    public IEnumerable<Claim>? Claims { get; set; }
}
