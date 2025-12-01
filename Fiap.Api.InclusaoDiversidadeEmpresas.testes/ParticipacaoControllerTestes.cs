using Fiap.Api.InclusaoDiversidadeEmpresas.Controllers;
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using InclusaoDiversidadeEmpresas.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.testes
{
    public class ParticipacaoEmTreinamentoControllerTests
    {
        [Fact]
        public async Task GetParticipacoes_ReturnsHttpStatusCode200()
        {
            // Arrange
            var mockService = new Mock<IParticipacaoEmTreinamentoService>();

            mockService
                .Setup(s => s.ListarParticipacaoEmTreinamentoService())
                .ReturnsAsync(new List<ParticipacaoEmTreinamentoModel>());

            var controller = new ParticipacaoEmTreinamentoController(mockService.Object);

            // Act
            var result = await controller.GetParticipacoes();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.IsAssignableFrom<IEnumerable<ParticipacaoEmTreinamentoModel>>(okResult.Value);
        }

        [Fact]
        public async Task GetParticipacaoById_ReturnsHttpStatusCode200()
        {
            // Arrange
            var mockService = new Mock<IParticipacaoEmTreinamentoService>();
            var participacao = new ParticipacaoEmTreinamentoModel { Id = 1 };

            mockService
                .Setup(s => s.ObterParticipacaoEmTreinamentoServicePorId(1))
                .ReturnsAsync(participacao);

            var controller = new ParticipacaoEmTreinamentoController(mockService.Object);

            // Act
            var result = await controller.GetParticipacao(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(participacao, okResult.Value);
        }

        [Fact]
        public async Task PostParticipacao_ReturnsHttpStatusCode201()
        {
            // Arrange
            var mockService = new Mock<IParticipacaoEmTreinamentoService>();
            var novaParticipacao = new ParticipacaoEmTreinamentoModel { Id = 1 };

            mockService
                .Setup(s => s.CriarParticipacaoEmTreinamentoService(novaParticipacao))
                .ReturnsAsync(novaParticipacao);

            var controller = new ParticipacaoEmTreinamentoController(mockService.Object);

            // Act
            var result = await controller.PostParticipacao(novaParticipacao);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

            // Assert
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(novaParticipacao, createdResult.Value);
        }
    }
}
