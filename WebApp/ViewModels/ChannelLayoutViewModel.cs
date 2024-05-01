namespace WebApp.ViewModels;

public class ChannelLayoutViewModel
{
    public IEnumerable<App.BLL.DTO.Channel> Channels { get; set; } = default!;
    public App.BLL.DTO.Channel? ActiveChannel { get; set; }
    public IEnumerable<App.BLL.DTO.Recipient>? ReceivedMessages { get; set; }
}