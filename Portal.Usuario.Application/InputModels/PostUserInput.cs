using MediatR;
using Portal.Usuario.Application.OutputModels;
using Portal.Usuario.Core.Entities;

namespace Portal.Usuario.Application.InputModels
{
    public class PostUserInput : IRequest<RequestResult<bool>>
    {
        public PostUserInput(string name, string email, string password, DateTime birthDay)
        {
            Name = name;
            Email = email;
            Password = password;
            BirthDay = birthDay;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }


        public static implicit operator User(PostUserInput entity)
        {
            return new User
            {
                BirthDay = entity.BirthDay,
                Email = entity.Email,
                Name = entity.Name,
                Password = entity.Password
            };
        }

    }
}