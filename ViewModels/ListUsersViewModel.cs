using System.Collections.Generic;

namespace TMOffersClients
{
    public class ListUsersViewModel
    {
        public List<User> Users { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
