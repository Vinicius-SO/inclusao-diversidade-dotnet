using AutoMapper;
using Fiap.Api.InclusaoDiversidadeEmpresas.Controllers;
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModel;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Testes
{
    public class ParticipacaoControllerTests
    {
        private readonly Mock<IParticipacaoEmTreinamentoService> _serviceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ParticipacaoController _controller;

        public ParticipacaoControllerTests()
        {
            _serviceMock = new Mock<IParticipacaoEmTreinamentoService>();
            _mapperMock = new Mock<IMapper>();

            _controller = new ParticipacaoController(
                _serviceMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Listar_ReturnsOkWithData()
        {
            // Arrange
            var modelos = new List<ParticipacaoEmTreinamentoModel>
            {
                new ParticipacaoEmTreinamentoModel { Id = 1 }
            };

            var viewModels = new List<ParticipacaoViewModel>
            {
                new ParticipacaoViewModel { ColaboradorId = 1, TreinamentoId = 1, Completo = false }
            };

            _serviceMock
                .Setup(s => s.ListarParticipacaoPaginado(1, 10))
                .ReturnsAsync(modelos);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<ParticipacaoViewModel>>(modelos))
                .Returns(viewModels);

            // Act
            var result = await _controller.Listar(1, 10);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(viewModels, okResult.Value);
        }

        [Fact]
        public async Task Criar_ReturnsOkWithCreatedObject()
        {
            // Arrange
            var vm = new ParticipacaoViewModel { ColaboradorId = 1, TreinamentoId = 1, Completo = false };
            var model = new ParticipacaoEmTreinamentoModel { Id = 1 };

            _mapperMock.Setup(m => m.Map<ParticipacaoEmTreinamentoModel>(vm))
                .Returns(model);

            _serviceMock
                .Setup(s => s.CriarParticipacaoEmTreinamentoService(model))
                .ReturnsAsync(model);

            _mapperMock.Setup(m => m.Map<ParticipacaoViewModel>(model))
                .Returns(vm);

            // Act
            var result = await _controller.Criar(vm);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(vm, okResult.Value);
        }

        [Fact]
        public async Task Deletar_ReturnsNoContent_WhenRemoved()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.DeletarParticipacaoEmTreinamentoService(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Deletar(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Deletar_ReturnsNotFound_WhenNotRemoved()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.DeletarParticipacaoEmTreinamentoService(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Deletar(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
