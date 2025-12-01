using InclusaoDiversidadeEmpresas.Controllers;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.testes
{
    public class ColaboradoresControllerTests
    {
        private Colaborador CriarColaboradorFake(long id = 1)
        {
            return new Colaborador
            {
                Id = id,
                NomeColaborador = "Teste",
                GeneroColaborador = "Feminino",
                EtniaColaborador = "Branca",
                Departamento = "TI",
                Email = "teste@teste.com",
                Senha = "123456"
            };
        }

        // GET /api/Colaboradores
        [Fact]
        public async Task GetColaboradores_Returns200AndList()
        {
            // Arrange
            var mockService = new Mock<IColaboradorService>();

            mockService.Setup(s => s.GetAllColaboradores())
                .ReturnsAsync(new List<Colaborador> { CriarColaboradorFake() });

            var controller = new ColaboradoresController(mockService.Object);

            // Act
            var result = await controller.GetColaboradores();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.IsAssignableFrom<IEnumerable<Colaborador>>(okResult.Value);
        }

        // GET /api/Colaboradores/{id}
        [Fact]
        public async Task GetColaboradorById_Returns200_WhenFound()
        {
            // Arrange
            var mockService = new Mock<IColaboradorService>();
            var colaborador = CriarColaboradorFake();

            mockService.Setup(s => s.GetColaboradorById(1))
                .ReturnsAsync(colaborador);

            var controller = new ColaboradoresController(mockService.Object);

            // Act
            var result = await controller.GetColaborador(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(colaborador, okResult.Value);
        }
    }
}
