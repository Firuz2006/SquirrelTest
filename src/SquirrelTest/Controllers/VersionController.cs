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
    
    [HttpGet("check")]
    public async Task<IActionResult> CheckForUpdate()
    {
        var updateUrl = configuration["Squirrel:GithubUpdateUrl"] ??
                        throw new ArgumentNullException();
        var updater = await UpdateManager.GitHubUpdateManager(updateUrl);
        var updateInfo = await updater.CheckForUpdate();

        return Ok(updateInfo);
    }
    
    [HttpGet("download")]
    public async Task<IActionResult> DownloadUpdate()
    {
        var updateUrl = configuration["Squirrel:GithubUpdateUrl"] ??
                        throw new ArgumentNullException();
        var updater = await UpdateManager.GitHubUpdateManager(updateUrl);
        var updateInfo = await updater.CheckForUpdate();

        if (updateInfo.ReleasesToApply.Count == 0)
        {
            return NotFound("No updates available.");
        }

        await updater.DownloadReleases(updateInfo.ReleasesToApply);

        return Ok("Update downloaded successfully.");
    }
    
    [HttpGet("apply")]
    public async Task<IActionResult> ApplyUpdate()
    {
        var updateUrl = configuration["Squirrel:GithubUpdateUrl"] ??
                        throw new ArgumentNullException();
        var updater = await UpdateManager.GitHubUpdateManager(updateUrl);
        var updateInfo = await updater.CheckForUpdate();

        if (updateInfo.ReleasesToApply.Count == 0)
        {
            return NotFound("No updates available.");
        }

        await updater.ApplyReleases(updateInfo);

        return Ok("Update applied successfully.");
    }
    
    [HttpGet("uninstall")]
    public async Task<IActionResult> Uninstall()
    {
        var updateUrl = configuration["Squirrel:GithubUpdateUrl"] ??
                        throw new ArgumentNullException();
        var updater = await UpdateManager.GitHubUpdateManager(updateUrl);
        await updater.FullUninstall();

        return Ok("Uninstalled successfully.");
    }
    
    [HttpGet("install")]
    public async Task<IActionResult> Install()
    {
        var updateUrl = configuration["Squirrel:GithubUpdateUrl"] ??
                        throw new ArgumentNullException();
        var updater = await UpdateManager.GitHubUpdateManager(updateUrl);
        await updater.FullInstall(false);

        return Ok("Installed successfully.");
    }
}