using System;

namespace LM.Core.Domain
{
    public class EmailCapturado
    {
        public EmailCapturado()
        {
            DataInclusao = DateTime.Now;
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime DataInclusao { get; set; }
    }
}
