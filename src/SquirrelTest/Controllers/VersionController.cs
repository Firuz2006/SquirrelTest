using Microsoft.AspNetCore.Mvc;
using Squirrel;

namespace SquirrelTest.Controllers;

[Controller]
[Route("api/[controller]")]
public class VersionController(IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetVersion()
    {
        var updateUrl = configuration["Squirrel:GithubUpdateUrl"] ??
                        throw new ArgumentNullException();
        var updater = await UpdateManager.GitHubUpdateManager(updateUrl);
        var currentlyInstalledVersion = updater.CurrentlyInstalledVersion();

        return Ok(currentlyInstalledVersion.Version);
    }
}