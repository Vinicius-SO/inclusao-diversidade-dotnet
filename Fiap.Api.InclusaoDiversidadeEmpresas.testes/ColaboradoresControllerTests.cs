using Fiap.Api.InclusaoDiversidadeEmpresas.Models;
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModels;
using InclusaoDiversidadeEmpresas.Controllers;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.ViewModels;
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

       [Fact]
        public async Task GetColaboradores_Returns200AndPagedResult()
        {
            // Arrange
            var mockService = new Mock<IColaboradorService>();

            var pagedResult = new PagedResultViewModel<ColaboradorListaViewModel>
            {
                Items = new List<ColaboradorListaViewModel>
                {
                    new ColaboradorListaViewModel { Id = 1, Nome = "Teste" }
                },
                Page = 1,
                PageSize = 10,
                TotalItems = 1
            };

            mockService
                .Setup(s => s.GetAllColaboradores(It.IsAny<QueryParameters>()))
                .ReturnsAsync(pagedResult);

            var controller = new ColaboradoresController(mockService.Object);

            // Act
            var result = await controller.GetColaboradores(new QueryParameters());
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<PagedResultViewModel<ColaboradorListaViewModel>>(okResult.Value);
        }

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


      [Fact]
        public async Task PostColaborador_Returns201AndCreatedObject()
        {
            // Arrange
            var mockService = new Mock<IColaboradorService>();
            var colaborador = CriarColaboradorFake();

            mockService.Setup(s => s.AddColaborador(colaborador))
                .ReturnsAsync(colaborador);

            var controller = new ColaboradoresController(mockService.Object);

            // Act
            var result = await controller.PostColaborador(colaborador);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

            // Assert
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(colaborador, createdResult.Value);
        }

       [Fact]
        public async Task PutColaborador_ReturnsNoContent_WhenUpdated()
        {
            // Arrange
            var mockService = new Mock<IColaboradorService>();
            var colaborador = CriarColaboradorFake();

            mockService.Setup(s => s.UpdateColaborador(colaborador))
                .ReturnsAsync(colaborador);

            var controller = new ColaboradoresController(mockService.Object);

            // Act
            var result = await controller.PutColaborador(1, colaborador);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

      [Fact]
        public async Task DeleteColaborador_ReturnsNoContent_WhenDeleted()
        {
            var mockService = new Mock<IColaboradorService>();

            mockService.Setup(s => s.DeleteColaborador(1)).ReturnsAsync(true);

            var controller = new ColaboradoresController(mockService.Object);

            var result = await controller.DeleteColaborador(1);

            Assert.IsType<NoContentResult>(result);
        }

    }
}
