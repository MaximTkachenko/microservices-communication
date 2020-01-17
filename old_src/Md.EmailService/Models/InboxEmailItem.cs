using System;

namespace Md.EmailService.Models
{
    public class InboxEmailItem
    {
        public string Title => $"{nameof(InboxEmailItem)}_{Guid.NewGuid()}";
        public DateTime Received => DateTime.UtcNow;
        public string Author => $"{Guid.NewGuid().ToString().Substring(0, 5)}@{Guid.NewGuid().ToString().Substring(0, 5)}.com";
    }
}
