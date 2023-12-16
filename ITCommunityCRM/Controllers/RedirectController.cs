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
        public ActionResult Index()
        {
            var queries = HttpContext.Request.QueryString.ToString();
            if (string.IsNullOrEmpty(queries))
            {
                return NotFound();
            }

            var redirectTo = linkService.GetOriginalUrl(queries);
            linkService.TrackRedirect(queries);

            return Redirect(redirectTo);
        }

    }
}
