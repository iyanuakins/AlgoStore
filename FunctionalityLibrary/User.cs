using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalityLibrary
{
    public abstract class User
    {
        protected User(int id, string name, string email, DateTime dateRegistered)
        {
            Id = id;
            Name = name;
            Email = email;
            DateRegistered = dateRegistered;
        }
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Email { get; private set; }
        public DateTime DateRegistered { get; private set; }
    }
}
