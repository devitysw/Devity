using System.ComponentModel.DataAnnotations;

namespace Devity.BlazorTest;

public class TestObject
{
    [Display(Name = "Testing name")]
    public string? Text { get; set; }
}
