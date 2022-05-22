using System;
using System.Collections.Generic;
using System.Text;

namespace Journeyman.Services.Entities
{
    public class ChatAgreement
    { 
        public int Id { get; set; }
        public string Text { get; set; }
        public long ChatId { get; set; }
    }
}
