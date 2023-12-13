using ITCommunityCRM.Data.Models.Consts;
using ITCommunityCRM.Models.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace ITCommunityCRM.Controllers
{
    public class ExternalAuthorizationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly AppSettings _appSettings;
        public ExternalAuthorizationController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<AppSettings> appSettings
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public async Task<IActionResult> TelegramLogin(
            string id,
            string first_name,
            string username,
            string hash)
        {
            var secretKey = ShaHash(_appSettings.TelegramToken);

            var myHash = HashHmac(secretKey, Encoding.UTF8.GetBytes(BuildKeyString()));

            var myHashStr = String.Concat(myHash.Select(i => i.ToString("x2")));
            var providerKey = id;
            if (myHashStr == hash)
            {
                var info = new UserLoginInfo(UserLoginInfoConst.TelegramLoginProvider, providerKey, UserLoginInfoConst.TelegramLoginProvider);
                var user_tel = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (user_tel == null)
                {
                    user_tel = new IdentityUser(username ?? first_name);
                    await _userManager.CreateAsync(user_tel);
                }
                await _userManager.AddLoginAsync(user_tel, info);
                await _signInManager.SignInAsync(user_tel, true);
                return RedirectToAction("Index", "Home");

            }
            return Redirect("/Identity/Account/Login");



            byte[] ShaHash(String value)
            {
                using (var hasher = SHA256.Create())

                { return hasher.ComputeHash(Encoding.UTF8.GetBytes(value)); }
            }

            byte[] HashHmac(byte[] key, byte[] message)
            {

                var hash = new HMACSHA256(key);
                return hash.ComputeHash(message);
            }


            string BuildKeyString()
            {
                return string.Join("\n", HttpContext.Request.Query
                    .Where(x => !string.Equals(x.Key, "hash", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(x => x.Key)
                    .Select(x => $"{x.Key}={x.Value}"));       
            }
        }
    }
}
