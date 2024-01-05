using AutoMapper;
using EzTech.Api.Services;
using EzTech.Data;
using Microsoft.AspNetCore.Mvc;

namespace EzTech.Api.Controllers.PublicControllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : BaseController
{
    private readonly IBlobService _blobService;

    [HttpGet]
    [Route("{fileName}")]
    public async Task<ActionResult> GetImage(string fileName)
    {
        var image = await _blobService.GetImage(fileName);
        return File(image, "image/jpeg");
    }

    public ImageController(IMapper mapper, EzTechDbContext dbContext, IBlobService blobService) : base(mapper,
        dbContext)
    {
        _blobService = blobService;
    }
}