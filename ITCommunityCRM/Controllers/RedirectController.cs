using Microsoft.AspNetCore.Mvc;
using ITCommunityCRM.Data;
using ITCommunityCRM.Services;

namespace ITCommunityCRM.Controllers
{
    public class RedirectController : ControllerBase
    {
        private readonly RedirectLinkService linkService;

        public RedirectController(RedirectLinkService linkService)
        {
            this.linkService = linkService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(CancellationToken token)
        {
            var queries = HttpContext.Request.QueryString.ToString();
            if (string.IsNullOrEmpty(queries))
            {
                return NotFound();
            }

            var redirectTo = linkService.GetOriginalUrl(queries);
            await linkService.TrackRedirectAsync(queries, token);

            return Redirect(redirectTo);
        }

    }
}
