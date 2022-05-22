
namespace Journeyman.Services.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public int Warnings { get; set; }
        public bool IsBanned { get; set; }
    }
}
