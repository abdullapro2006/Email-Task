using Pustok.Database.DomainModels;
using System;

namespace Pustok.ViewModels
{
    public class UserNotificationViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public User User { get; set; }
        public Order UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
