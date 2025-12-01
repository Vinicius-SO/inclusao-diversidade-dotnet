using Fiap.Api.InclusaoDiversidadeEmpresas.Controllers;
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using InclusaoDiversidadeEmpresas.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.testes
{
    public class TreinamentoControllerTests
    {
        [Fact]
        public async Task GetTreinamentos_ReturnsHttpStatusCode200()
        {
            // Arrange 
            var mockService = new Mock<ITreinamentoService>();

            mockService
                .Setup(s => s.ListarTreinamentos())
                .ReturnsAsync(new List<TreinamentoModel>());

            var controller = new TreinamentoController(mockService.Object);

            // Act — chama a action
            var result = await controller.GetTreinamentos();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Assert
            Assert.Equal(200, okResult.StatusCode);

            Assert.IsAssignableFrom<IEnumerable<TreinamentoModel>>(okResult.Value);
        }

        [Fact]
        public async Task GetTreinamentoById_ReturnsHttpStatusCode200()
        {
            // Arrange
            var mockService = new Mock<ITreinamentoService>();
            var treinamento = new TreinamentoModel { Id = 1, Titulo = "Teste" };

            mockService
                .Setup(s => s.ObterTreinamentoPorId(1))
                .ReturnsAsync(treinamento);

            var controller = new TreinamentoController(mockService.Object);

            // Act
            var result = await controller.GetTreinamento(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(treinamento, okResult.Value);
        }

        [Fact]
        public async Task PostTreinamento_ReturnsHttpStatusCode201()
        {
            // Arrange
            var mockService = new Mock<ITreinamentoService>();
            var novoTreinamento = new TreinamentoModel { Id = 1, Titulo = "Novo Treinamento" };

            mockService
                .Setup(s => s.CriarTreinamento(novoTreinamento))
                .ReturnsAsync(novoTreinamento);

            var controller = new TreinamentoController(mockService.Object);

            // Act
            var result = await controller.PostTreinamento(novoTreinamento);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

            // Assert
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(novoTreinamento, createdResult.Value);
        }

    }
}