using Microsoft.AspNetCore.Components;

namespace PrinterUI.Source.Shared.UI;

public sealed partial class Button : ComponentBase
{
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> Attributes { get; set; } = new();

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public ButtonVariant Variant { get; set; } = ButtonVariant.Default;
    [Parameter] public ButtonSize Size { get; set; } = ButtonSize.Default;
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public ButtonType Type { get; set; } = ButtonType.Button;
    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public bool Disabled { get; set; }

    private string ButtonClasses => $"inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm" +
                                    $" font-medium ring-offset-background transition-colors focus-visible:outline-none" +
                                    $" focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2" +
                                    $" disabled:pointer-events-none disabled:opacity-5" +
                                    $" {VariantClasses} {SizeClasses} {Class}";

    private string VariantClasses => Variant switch
    {
        ButtonVariant.Default => "bg-primary text-primary-foreground hover:bg-primary/90",
        ButtonVariant.Destructive => "bg-destructive text-destructive-foreground hover:bg-destructive/90",
        ButtonVariant.Outline => "border border-input bg-background hover:bg-accent hover:text-accent-foreground",
        ButtonVariant.Secondary => "bg-secondary text-secondary-foreground hover:bg-secondary/80",
        ButtonVariant.Ghost => "hover:bg-accent hover:text-accent-foreground",
        ButtonVariant.Link => "text-primary underline-offset-4 hover:underline",
        _ => string.Empty
    };

    private string SizeClasses => Size switch
    {
        ButtonSize.Default => "h-9 px-4 py-2",
        ButtonSize.Small => "h-8 rounded-md px-3 text-xs",
        ButtonSize.Large => "h-10 rounded-md px-8",
        ButtonSize.Full => "size-full",
        ButtonSize.Icon => "h-9 w-9",
        _ => string.Empty
    };

    private string HtmlType => Type switch
    {
        ButtonType.Reset => "reset",
        ButtonType.Submit => "submit",
        _ => "button"
    };
}

public enum ButtonVariant
{
    Default,
    Destructive,
    Outline,
    Secondary,
    Ghost,
    Link
}

public enum ButtonSize
{
    Default,
    Small,
    Large,
    Full,
    Icon
}

public enum ButtonType
{
    Button,
    Reset,
    Submit
}