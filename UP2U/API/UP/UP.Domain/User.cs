using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int TipoUsuarioId { get; set; }
        public TypeUser TypeUser { get; set; }
        public string Senha { get; set; }


        public User()
        {
            TypeUser = new TypeUser();
        }
    }
}
