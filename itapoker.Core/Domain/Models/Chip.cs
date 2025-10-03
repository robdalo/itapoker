
namespace itapoker.Core.Domain.Models;

public class Chip
{
    public int Quantity { get; set; }
    public int Value { get; set; }
    public bool Visible { get; set; }
    public string Colour => GetColour();
    public string Title => $"{this.Colour} Chip - {this.Quantity}";
    public string Url => $"images/chips/{this.Colour}.png".Replace(" ", "-").ToLower();
    public int Total => this.Quantity * this.Value;

    public Chip()
    {
    }

    public Chip(int value)
    {
        this.Value = value;
    }

    private string GetColour()
    {
        switch (this.Value)
        {
            case 5: return "Light Blue";
            case 10: return "Red";
            case 25: return "Dark Red";
            case 50: return "Blue";

            default: return "";
        }
    } 
}