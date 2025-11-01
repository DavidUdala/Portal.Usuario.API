using Portal.Usuario.Core.Entities;

namespace Portal.Usuario.Application.OutputModels
{
    public class UserOutput
    {
        public string Name { get; set; } 
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime DateCreated { get; set; }

        public static implicit operator UserOutput(User entity)
        {
            return new UserOutput
            {
                BirthDay = entity.BirthDay,
                Email = entity.Email,
                DateCreated = entity.DateCreated,
                Name = entity.Name
            };
        }
    }
}
