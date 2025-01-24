using API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	// What should happen now is any time that we call an API request, the user looks logged in.
	// They're going to have their last active property updated
	[ServiceFilter(typeof(LogUserActivityHelper))]
	[Route("api/[controller]")]
	[ApiController]
	public class BaseAPIController : ControllerBase
	{
	}
}
