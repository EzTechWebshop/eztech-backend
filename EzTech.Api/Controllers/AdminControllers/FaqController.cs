using AutoMapper;
using EzTech.Data;
using EzTech.Data.ApiModels.FaqApiModels;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class FaqController : BaseAdminController
{
    [HttpPost]
    public async Task<IActionResult> CreateFaq([FromBody] CreateFaqRequest request)
    {
        var oldFaq = await DbContext.Faqs.AnyAsync(x => x.Question == request.Question);
        if (oldFaq)
        {
            return Conflict("Faq already exists");
        }

        var newFaq = new Faq
        {
            Question = request.Question,
            Answer = request.Answer
        };
        await DbContext.Faqs.AddAsync(newFaq);
        await DbContext.SaveChangesAsync();
        return Ok(newFaq);
    }

    [HttpPatch]
    [Route("{faqId:int}")]
    public async Task<IActionResult> UpdateFaq(int faqId, [FromBody] UpdateFaqRequest request)
    {
        var faq = await DbContext.Faqs.FindAsync(faqId);
        if (faq == null) return NotFound();
        faq.Question = request.Question ?? faq.Question;
        faq.Answer = request.Answer ?? faq.Answer;
        await DbContext.SaveChangesAsync();
        return Ok(faq);
    }

    [HttpDelete]
    [Route("{faqId:int}")]
    public async Task<IActionResult> DeleteFaq(int faqId)
    {
        var faq = await DbContext.Faqs.FindAsync(faqId);
        if (faq == null) return NotFound();
        DbContext.Faqs.Remove(faq);
        await DbContext.SaveChangesAsync();
        return Ok("Faq deleted");
    }

    public FaqController(IMapper mapper, EzTechDbContext dbContext) : base(mapper, dbContext)
    {
    }
}