namespace dotnet_messenger.Models;

public class MessageModel
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public DateTime Date { get; set; }

    public bool Status { get; set; }
}

