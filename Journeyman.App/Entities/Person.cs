
namespace Journeyman.App.Entities
{
    public class Person
    {
        public int Id { get; }
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public int Warnings { get; set; }
        public bool IsBanned { get; set; }
    }
}
