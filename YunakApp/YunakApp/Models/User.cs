using System.Collections.Generic;

namespace YunakApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public GeneralInformation GeneralInformation { get; set; }
        public ICollection<Operation> Operations { get; set; }

        public User()
        {
            Operations = new List<Operation>();
        }
    }
}
