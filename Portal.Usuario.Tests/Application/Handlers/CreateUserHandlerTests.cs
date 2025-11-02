using Moq;
using Portal.Usuario.Application.Handlers;
using Portal.Usuario.Application.InputModels;
using Portal.Usuario.Core.Entities;
using Portal.Usuario.Core.Interfaces;
using System.Linq.Expressions;

namespace Portal.Usuario.Tests.Handlers
{
    public class CreateUserHandlerTests
    {
        private readonly Mock<IApplicationDbRepository<User>> _repositoryMock;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            _repositoryMock = new Mock<IApplicationDbRepository<User>>();
            _handler = new CreateUserHandler(null!, _repositoryMock.Object); // O IMediator não é usado
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoEmailJaExiste()
        {
            // Arrange
            var input = new PostUserInput("Fulano Beltrano", "teste@teste.com", "1234", new DateTime(1995,09,28));
            var existingUser = new User {Id = 1, Name = "Ciclano Beltrano", Password = "1234", Email = "teste@teste.com" };

            _repositoryMock
                .Setup(r => r.GetOne(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _handler.Handle(input, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email já cadastrado", result.Message);
            _repositoryMock.Verify(r => r.Create(It.IsAny<User>()), Times.Never);
            _repositoryMock.Verify(r => r.Save(), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveCriarUsuario_QuandoEmailNaoExiste()
        {
            // Arrange
            var input = new PostUserInput("Fulano Beltrano", "novo@teste.com", "1234", new DateTime(1995,09,28));

            _repositoryMock
                .Setup(r => r.GetOne(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            _repositoryMock
                .Setup(r => r.Create(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(r => r.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(input, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Cadastrado realizado com sucesso!", result.Message);
            _repositoryMock.Verify(r => r.Create(It.IsAny<User>()), Times.Once);
            _repositoryMock.Verify(r => r.Save(), Times.Once);
        }

    }
}