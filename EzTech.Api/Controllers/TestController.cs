using AutoMapper;
using EzTech.Api.Authentication;
using EzTech.Api.Services;
using EzTech.Data;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers;

/// <summary>
/// This controller is used to test the API and is not used in production, it is for my examination
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TestController : BaseController
{
    private readonly IBlobService _blobService;

    // An array with the names of the categories, used to create test data
    private readonly List<string> _categoryNames = new()
    {
        "Food",
        "Drinks",
        "Electronics",
        "Clothing",
        "Toys",
        "Books"
    };

    [HttpGet]
    [Route("start-up-for-exam")]
    public async Task<ActionResult> Start()
    {
        // Resets all data in sql database
        await Wipe();

        // Creates test data
        var websiteInfoCreated = await CreateWebsiteInfo();
        if (!websiteInfoCreated) return BadRequest("Failed to create website info");

        var adminCreated = await CreateAdminUserAndTestUser();
        if (!adminCreated) return BadRequest("Failed to create admin user");

        var categoriesCreated = await CreateTestCategories();
        if (!categoriesCreated) return BadRequest("Failed to create categories");

        var productsCreated = await CreateTestProducts();
        if (!productsCreated) return BadRequest("Failed to create products");

        var faqsCreated = await CreateTestFaq();
        if (!faqsCreated) return BadRequest("Failed to create faqs");

        var aboutUsCreated = await CreateTestAboutUs();
        if (!aboutUsCreated) return BadRequest("Failed to create about us");

        var promotionsCreated = await CreateTestPromotions();
        if (!promotionsCreated) return BadRequest("Failed to create promotions");

        return Ok("All test data created");
    }

    [HttpGet]
    [Route("start-up-production")]
    public async Task<ActionResult> StartRaw()
    {
        // Resets all data in sql database
        await Wipe();
        // Creates test data
        var startUpDataCreated = await StartupData();
        if (!startUpDataCreated) return BadRequest("Failed to create website info");
        return Ok("Test data created");
    }

    [HttpGet]
    [Route("wipe")]
    public async Task<ActionResult> Wipe()
    {
        // Resets all data in sql database
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.Database.EnsureCreatedAsync();
        // Resets all data in blob storage
        await _blobService.Wipe();
        return Ok("Database wiped");
    }

    private async Task<bool> StartupData()
    {
        // Because the frontend relies on the website info, it is important that it is created first
        // An administrator user is created, so that the frontend can login, and manage the website

        // Creating the administrator
        var admin = new User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Smith",
            Salt = PasswordHelper.GenerateSalt(),
            IsEmailVerified = true,
            VerificationToken = "1234",
            Role = UserRoles.Admin,
            PhoneNumber = "00000000"
        };
        admin.Hash = PasswordHelper.HashPassword("password", admin.Salt);
        await DbContext.Users.AddAsync(admin);
        await DbContext.SaveChangesAsync();

        // Creating the website info
        // Opening Hours
        var openingHours = new List<OpeningHours>();

        // Monday - Friday
        for (var i = 1; i < 6; i++)
        {
            var openingHour = new OpeningHours
            {
                DayOfWeek = (DayOfWeek)i,
                OpenTime = new TimeSpan(8, 0, 0),
                CloseTime = new TimeSpan(16, 0, 0)
            };
            openingHours.Add(openingHour);
        }

        // Saturday
        openingHours.Add(new OpeningHours
        {
            DayOfWeek = DayOfWeek.Saturday,
            OpenTime = new TimeSpan(10, 0, 0),
            CloseTime = new TimeSpan(14, 0, 0)
        });
        // Sunday
        openingHours.Add(new OpeningHours
        {
            DayOfWeek = DayOfWeek.Sunday,
        });
        var websiteInfo = new WebsiteInfo
        {
            Name = "EzTech",
            CompanyName = "EzTech",
            Email = "user@example.com",
            Cvr = "00000000",
            City = "City",
            PostalCode = "0000",
            Address = "Address 0000",
            Country = "Country",
            PhoneNumber = "+45 00 00 00 00",
            Facebook = "#",
            Instagram = "#",
            TrustPilot = "#",
            Website = "#",
            FooterInfo = "...",
            WeeklyOpeningHours = openingHours,
        };
        await DbContext.WebsiteInfos.AddAsync(websiteInfo);
        await DbContext.SaveChangesAsync();
        return true;
    }


    // Below are the methods used to create test data, they are not used in production
    private async Task<bool> CreateTestPromotions()
    {
        var promotions = new List<Promotion>();
        for (var i = 0; i < 7; i++)
        {
            var promotion = new Promotion
            {
                Title = $"Promotion {i}",
                Description = $"Promotion {i} description",
                StartDate = DateTime.Now.AddDays(i),
                EndDate = DateTime.Now.AddDays(i + 7)
            };
            promotions.Add(promotion);
        }

        await DbContext.Promotions.AddRangeAsync(promotions);
        await DbContext.SaveChangesAsync();
        return true;
    }

    private async Task<bool> CreateWebsiteInfo()
    {
        // Opening Hours
        // Opening Hours
        var openingHours = new List<OpeningHours>();

        // Monday - Friday
        for (var i = 1; i < 6; i++)
        {
            var openingHour = new OpeningHours
            {
                DayOfWeek = (DayOfWeek)i,
                OpenTime = new TimeSpan(8, 0, 0),
                CloseTime = new TimeSpan(16, 0, 0)
            };
            openingHours.Add(openingHour);
        }

        // Saturday
        openingHours.Add(new OpeningHours
        {
            DayOfWeek = DayOfWeek.Saturday,
            OpenTime = new TimeSpan(10, 0, 0),
            CloseTime = new TimeSpan(14, 0, 0)
        });
        // Sunday
        openingHours.Add(new OpeningHours
        {
            DayOfWeek = DayOfWeek.Sunday,
        });

        // Special Days
        var specialDays = new List<OpeningHoursSpecial>();
        specialDays.Add(new OpeningHoursSpecial
        {
            Title = "Christmas",
            Description = "We are closed on Christmas",
            Date = new DateTime(2021, 12, 24),
            EndDate = new DateTime(2021, 12, 26),
            OpenTime = TimeSpan.Zero,
            CloseTime = TimeSpan.Zero,
        });
        specialDays.Add(new OpeningHoursSpecial
        {
            Title = "New Years",
            Description = "We are closed on New Years",
            Date = new DateTime(2022, 1, 1),
            EndDate = new DateTime(2022, 1, 1),
            OpenTime = TimeSpan.Zero,
            CloseTime = TimeSpan.Zero,
        });


        var websiteInfo = new WebsiteInfo
        {
            Name = "EzTech",
            CompanyName = "EzTech",
            Email = "user@example.com",
            Cvr = "12344321",
            City = "Copenhagen",
            PostalCode = "2000",
            Address = "Fredevej 0000",
            Country = "Denmark",
            PhoneNumber = "+45 00 00 00 00",
            Facebook = "#",
            Instagram = "#",
            TrustPilot = "#",
            Website = "#",
            FooterInfo = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla euismod, nisl eget ultrices aliquam, nunc nisl aliquet nunc, quis aliquam nisl nunc quis nisl.",
            WeeklyOpeningHours = openingHours,
            SpecialOpeningHours = specialDays,
        };
        await DbContext.WebsiteInfos.AddAsync(websiteInfo);
        await DbContext.SaveChangesAsync();
        return true;
    }

    private async Task<bool> CreateAdminUserAndTestUser()
    {
        const string startUpPassword = "password";
        var user = new User
        {
            Email = "user2@example.com",
            FirstName = "John",
            LastName = "Tester",
            Salt = PasswordHelper.GenerateSalt(),
            IsEmailVerified = true,
            VerificationToken = "1234",
            Role = UserRoles.Admin,
            PhoneNumber = "87654321"
        };
        user.Hash = PasswordHelper.HashPassword(startUpPassword, user.Salt);
        var admin = new User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Smith",
            Salt = PasswordHelper.GenerateSalt(),
            IsEmailVerified = true,
            VerificationToken = "4321",
            Role = UserRoles.Admin,
            PhoneNumber = "12345678"
        };
        admin.Hash = PasswordHelper.HashPassword(startUpPassword, admin.Salt);
        await DbContext.Users.AddAsync(user);
        await DbContext.Users.AddAsync(admin);
        await DbContext.SaveChangesAsync();
        return true;
    }

    private async Task<bool> CreateTestCategories()
    {
        // An array with the names of the categories
        var categoryNames = new List<string> { "Food", "Drinks", "Electronics", "Clothing", "Toys", "Books" };
        var categories = new List<Category>();
        categoryNames.ForEach((name) =>
        {
            var category = new Category
            {
                Name = name,
                Description = $"{name} Category, containing all {name} items",
            };
            categories.Add(category);
        });
        await DbContext.Categories.AddRangeAsync(categories);
        await DbContext.SaveChangesAsync();
        return true;
    }

    private async Task<bool> CreateTestProducts()
    {
        await CreateTestProductTask(_categoryNames[0]);
        await CreateTestProductTask(_categoryNames[1]);
        await CreateTestProductTask(_categoryNames[2]);
        await CreateTestProductTask(_categoryNames[3]);
        await CreateTestProductTask(_categoryNames[4]);
        await CreateTestProductTask(_categoryNames[5]);
        return true;
    }

    private async Task CreateTestProductTask(string name)
    {
        var category = await DbContext.Categories.FirstOrDefaultAsync(e => e.Name == name);
        if (category == null) return;
        var products = new List<Product>();
        for (var i = 0; i < 20; i++)
        {
            var product = new Product
            {
                Name = $"{name} {i}",
                Description = $"A {name} item",
                Price = i + 100,
                Stock = 50,
            };
            product.Categories.Add(category);
            products.Add(product);
        }

        await DbContext.Products.AddRangeAsync(products);
        await DbContext.SaveChangesAsync();
    }

    private async Task<bool> CreateTestFaq()
    {
        var faqs = new List<Faq>();
        for (var i = 0; i < 7; i++)
        {
            var faq = new Faq
            {
                Question = $"Question {i}",
                Answer =
                    $"Answer {i}, Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla euismod, nisl eget ultrices aliquam, nunc nisl aliquet nunc, quis aliquam nisl nunc quis nisl.",
            };
            faqs.Add(faq);
        }

        await DbContext.Faqs.AddRangeAsync(faqs);
        await DbContext.SaveChangesAsync();
        return true;
    }

    private async Task<bool> CreateTestAboutUs()
    {
        var websiteInfo = await DbContext.WebsiteInfos
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();
        if (websiteInfo == null) return false;
        const string aboutUsText =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla euismod, nisl eget ultrices aliquam, nunc nisl aliquet nunc, quis aliqua";
        for (var i = 5; i < 8; i++)
        {
            for (var j = 2; j < 5; j++)
            {
                var counter = i;
                var text = "";
                while (counter > 0)
                {
                    text += aboutUsText;
                    counter--;
                }

                var websiteInfoText = new WebsiteInfoText
                {
                    Title = $"{i}.{j} Title",
                    Description = $"{i}.{j} {text}",
                };
                websiteInfo.WebsiteInfoFields.Add(websiteInfoText);
            }
        }

        DbContext.WebsiteInfos.Update(websiteInfo);
        await DbContext.SaveChangesAsync();
        return true;
    }

    [HttpGet]
    [Route("get-blob-count")]
    public async Task<ActionResult> GetBlobCount()
    {
        var count = _blobService.GetBlobCount();
        return Ok(count);
    }

    public TestController(IMapper mapper, EzTechDbContext dbContext, IBlobService blobService) : base(mapper, dbContext)
    {
        _blobService = blobService;
    }
}