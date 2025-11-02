using Portal.Usuario.Core.Entities;

namespace Portal.Usuario.Application.OutputModels
{
    public class UserOutput
    {
        public string Name { get; set; } 
        public string Email { get; set; }
        public string BirthDay { get; set; }
        public string DateCreated { get; set; }

        public static implicit operator UserOutput(User entity)
        {
            return new UserOutput
            {
                BirthDay = entity.BirthDay.ToString("dd/MM/yyyy"),
                Email = entity.Email,
                DateCreated = entity.DateCreated.ToString("dd/MM/yyyy"),
                Name = entity.Name
            };
        }
    }
}
