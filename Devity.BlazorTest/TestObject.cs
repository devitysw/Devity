using System.ComponentModel.DataAnnotations;
using Devity.Blazor;

namespace Devity.BlazorTest;

public class TestObject
{
    [Display(Name = "Testing name")]
    public string? Text { get; set; }

    [Display(Name = "Toast type to show")]
    public Toasts.ToastType? SelectedOption { get; set; }
}
