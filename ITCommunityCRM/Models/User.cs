using Microsoft.AspNetCore.Identity;

namespace ITCommunityCRM.Models;

public class User : IdentityUser
{
    public string? Telegram { get; set; }
}