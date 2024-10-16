using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckListApp.Model
{
    [Table("Users")]
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        public int UserID { get; set; } = 1;
        public string Username { get; set; } = string.Empty;
        public string FirstName {  get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; } = null;
        public  string Email { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
