using AutoMapper;
using EzTech.Data;
using EzTech.Data.ApiModels.WebsiteInfoApiModels;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class WebsiteInfoController : BaseAdminController
{
    [HttpPost]
    [Route("add-info-text")]
    public async Task<IActionResult> AddInfoText([FromBody] AddWebsiteInfoTextRequest request)
    {
        var websiteInfo = await DbContext.WebsiteInfos
            .OrderBy(u => u.Id)
            .Include(x => x.WebsiteInfoFields)
            .FirstOrDefaultAsync();
        if (websiteInfo == null) return NotFound();
        var websiteInfoField = new WebsiteInfoText
        {
            Title = request.Title,
            Description = request.Description,
            Type = Enum.Parse<WebsiteInfoFieldType>(request.Topic)
        };
        websiteInfo.WebsiteInfoFields.Add(websiteInfoField);
        await DbContext.SaveChangesAsync();
        return Ok(websiteInfoField);
    }

    [HttpPatch]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateWebsiteInfo(int id, [FromBody] UpdateWebsiteInfoRequest request)
    {
        var websiteInfo = await DbContext.WebsiteInfos
            .OrderBy(u => u.Id)
            .FirstOrDefaultAsync();
        if (websiteInfo == null) return NotFound();
        websiteInfo.Update(request);
        await DbContext.SaveChangesAsync();
        return Ok(websiteInfo);
    }

    [HttpPatch]
    [Route("update-info-text/{id:int}")]
    public async Task<IActionResult> UpdateInfoText(int id, [FromBody] UpdateWebsiteInfoTextRequest request)
    {
        var websiteInfo = await DbContext.WebsiteInfos
            .OrderBy(u => u.Id)
            .Include(x => x.WebsiteInfoFields)
            .FirstOrDefaultAsync();
        if (websiteInfo == null) return NotFound();
        var websiteInfoField = websiteInfo.WebsiteInfoFields.FirstOrDefault(x => x.Id == id);
        if (websiteInfoField == null) return NotFound();
        websiteInfoField.Update(request);
        await DbContext.SaveChangesAsync();
        return Ok(websiteInfoField);
    }

    [HttpDelete]
    [Route("delete-info-text/{id:int}")]
    public async Task<IActionResult> DeleteInfoText(int id)
    {
        var websiteInfo = await DbContext.WebsiteInfos
            .OrderBy(u => u.Id)
            .Include(x => x.WebsiteInfoFields)
            .FirstOrDefaultAsync();
        if (websiteInfo == null) return NotFound();
        var websiteInfoField = websiteInfo.WebsiteInfoFields
            .OrderBy(u => u.Id)
            .FirstOrDefault(x => x.Id == id);
        if (websiteInfoField == null) return NotFound();
        websiteInfo.WebsiteInfoFields.Remove(websiteInfoField);
        await DbContext.SaveChangesAsync();
        return Ok("Deleted");
    }

    public WebsiteInfoController(IMapper mapper, EzTechDbContext dbContext) : base(mapper, dbContext)
    {
    }
}