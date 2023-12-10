using ITCommunityCRM.Models.Configuration;
using ITCommunityCRM.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly SignInManager<IdentityUser> _loginManager;
        //private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly AppSettings _appSettings;
        public ExternalAuthorizationController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            //IMapper mapper,
            IEmailSender emailSender,
            IOptions<AppSettings> appSettings
            )
        {
            _userManager = userManager;
            _loginManager = signInManager;
            //_mapper = mapper;
            _emailSender = emailSender;
            _appSettings = appSettings.Value;
        }

        [HttpPost("TelegramLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> TelegramLogin([FromBody] UserTelegramDto user, string returnUrl = null)
        {
            // place bot token of your bot here
            //https://xl-01.ru/telelram-callback?id=5384361370&first_name=Igor&username=igor_01ts&auth_date=1663303742&hash=bd734108e0d695bd89a6eaf56e50b3cdc2a5adc8dc4e98a8c777bc66545bebbf
            //https://gist.github.com/anonymous/6516521b1fb3b464534fbc30ea3573c2
            // https://codex.so/telegram-auth?ysclid=l89qryxyrb334407760

            var secretKey = ShaHash(_appSettings.TelegramToken);

            var myHash = HashHmac(secretKey, Encoding.UTF8.GetBytes(user.ToString()));

            //php   hash   https://php.ru/manual/function.hash.html?ysclid=l87m3tbcl2736101629
            //php   hash_hmac   https://www.php.net/manual/ru/function.hash-hmac.php
            //php    time()  https://www.php.net/manual/ru/function.time.php

            var myHashStr = String.Concat(myHash.Select(i => i.ToString("x2")));
            var providerKey = user.Id;
            if (myHashStr == user.Hash)
            {
                var info = new UserLoginInfo("Telegram", providerKey, "Telegram");
                var info1 = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }

                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    // If the user does not have an account, then ask the user to create an account.
                    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }
            }

            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });




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
        }
    }
}
