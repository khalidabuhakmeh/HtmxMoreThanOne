using System.Security.Cryptography;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HtmxMoreThanOne.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        return Request.IsHtmx()
            ? Partial("Normal", this)
            : Page();
    }

    public PartialViewResult OnGetOutOfBand()
    {
        return Partial("OutOfBandColors");
    }

    public PartialViewResult OnGetColor(string color)
    {
        var value = color switch
        {
            "red" => Color.Red,
            "green" => Color.Green,
            "blue" => Color.Blue,
            _ => new Color(0, 0, 0)
        };

        return Partial("Color", Color.Random(value));
    }
}

public record Color(int R, int G, int B)
{
    public static readonly Color Red = new(255, 0, 0);
    public static readonly Color Green = new(0, 255, 0);
    public static readonly Color Blue = new(0, 0, 255);

    public static Color Random(Color? color = null)
        => (color ??= new Color(0, 0, 0))
            with
            {
                R = Math.Clamp(color.R + RandomNumberGenerator.GetInt32(0, 255), 0, 255),
                G = Math.Clamp(color.G + RandomNumberGenerator.GetInt32(0, 255), 0, 255),
                B = Math.Clamp(color.B + RandomNumberGenerator.GetInt32(0, 255), 0, 255)
            };
}