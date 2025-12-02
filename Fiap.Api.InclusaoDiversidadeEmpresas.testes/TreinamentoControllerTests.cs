using AutoMapper;
using Fiap.Api.InclusaoDiversidadeEmpresas.Controllers;
using Fiap.Api.InclusaoDiversidadeEmpresas.Models;
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModels;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Tests.Controllers
{
    public class TreinamentoControllerTests
    {
        // Precisamos mockar tanto o Service quanto o Mapper
        private readonly Mock<ITreinamentoService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TreinamentoController _controller;

        public TreinamentoControllerTests()
        {
            _mockService = new Mock<ITreinamentoService>();
            _mockMapper = new Mock<IMapper>();

            // Injetamos os dois mocks no construtor
            _controller = new TreinamentoController(_mockService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetTreinamentos_ReturnsHttpStatusCode200()
        {
            // Arrange 
            // O controller espera um PagedResultViewModel, não apenas uma lista
            var pagedResult = new PagedResultViewModel<TreinamentoModel>
            {
                Items = new List<TreinamentoModel>(),
                TotalItems = 0
            };

            _mockService
                .Setup(s => s.GetAllTreinamentos(It.IsAny<QueryParameters>())) // Aceita qualquer parâmetro
                .ReturnsAsync(pagedResult);

            // Act
            // O método agora exige QueryParameters (pode passar null ou um objeto vazio para o teste)
            var result = await _controller.GetTreinamentos(new QueryParameters());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<PagedResultViewModel<TreinamentoModel>>(okResult.Value);
        }

        [Fact]
        public async Task GetTreinamentoById_ReturnsHttpStatusCode200()
        {
            // Arrange
            var model = new TreinamentoModel { Id = 1, Titulo = "Teste" };
            var viewModel = new TreinamentoViewModel { Id = 1, Titulo = "Teste" };

            // 1. Mock do Service retornando o Model
            _mockService
                .Setup(s => s.GetTreinamentoById(1))
                .ReturnsAsync(model);

            // 2. Mock do Mapper convertendo Model -> ViewModel
            _mockMapper
                .Setup(m => m.Map<TreinamentoViewModel>(model))
                .Returns(viewModel);

            // Act
            var result = await _controller.GetTreinamento(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(viewModel, okResult.Value);
        }

        [Fact]
        public async Task PostTreinamento_ReturnsHttpStatusCode201()
        {
            // Arrange
            var viewModelEntrada = new TreinamentoViewModel { Titulo = "Novo Treinamento" };
            var model = new TreinamentoModel { Id = 1, Titulo = "Novo Treinamento" };
            var viewModelSaida = new TreinamentoViewModel { Id = 1, Titulo = "Novo Treinamento" };

            // 1. Mock Mapper: ViewModel (Entrada) -> Model
            _mockMapper
                .Setup(m => m.Map<TreinamentoModel>(viewModelEntrada))
                .Returns(model);

            // 2. Mock Service: Recebe Model, Salva e Retorna Model preenchido
            _mockService
                .Setup(s => s.AddTreinamento(model))
                .ReturnsAsync(model);

            // 3. Mock Mapper: Model (Salvo) -> ViewModel (Retorno)
            _mockMapper
                .Setup(m => m.Map<TreinamentoViewModel>(model))
                .Returns(viewModelSaida);

            // Act
            var result = await _controller.PostTreinamento(viewModelEntrada);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

            // Assert
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(viewModelSaida, createdResult.Value);
        }
    }
}