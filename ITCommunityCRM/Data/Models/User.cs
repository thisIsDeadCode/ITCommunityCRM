using Microsoft.AspNetCore.Identity;

namespace ITCommunityCRM.Data.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }


    public User(string userName, string firstName) : base(userName)
    {
        FirstName = firstName;
    }
}