using InclusaoDiversidadeEmpresas.Controllers;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.Services;
using InclusaoDiversidadeEmpresas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace InclusaoDiversidadeEmpresas.Tests.Controllers
{
    public class RelatoriosControllerTests
    {
        [Fact]
        public async Task GetRelatorioDiversidade_DeveRetornarOk_QuandoRelatorioExistir()
        {
            // Arrange
            var mockService = new Mock<IRelatorioService>();

            var relatorioFake = new RelatorioDeDiversidadeModel
            {
                DataGerada = DateTime.UtcNow,
                TotalColaborador = 10,
                ContagemDeMulheres = 4,
                ContagemDePessoasNegras = 3,
                ContagemDePessoasLgbt = 2,
                ContagemDePessoasComDesabilidade = 1
            };

            mockService
                .Setup(s => s.GerarRelatorioAsync())
                .ReturnsAsync(relatorioFake);

            var controller = new RelatoriosController(mockService.Object);

            // Act
            var resultado = await controller.GetRelatorioDiversidade();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);

            var retorno = Assert.IsType<DashboardDiversidadeViewModel>(okResult.Value);

            // Validação das propriedades
            Assert.Equal(relatorioFake.DataGerada, retorno.DataGerada);
            Assert.Equal(10, retorno.TotalColaborador);
            Assert.Equal(4, retorno.QtdMulheres);
            Assert.Equal(3, retorno.QtdNegros);
            Assert.Equal(2, retorno.QtdLgbt);
            Assert.Equal(1, retorno.QtdPcd);

            // Validação das porcentagens
            Assert.Equal(40.00, retorno.PorcentagemMulheres);
            Assert.Equal(30.00, retorno.PorcentagemNegros);
            Assert.Equal(20.00, retorno.PorcentagemLgbt);
            Assert.Equal(10.00, retorno.PorcentagemPcd);

            mockService.Verify(s => s.GerarRelatorioAsync(), Times.Once);
        }

        [Fact]
        public async Task GetRelatorioDiversidade_DeveRetornarNotFound_QuandoRelatorioForNulo()
        {
            // Arrange
            var mockService = new Mock<IRelatorioService>();

            mockService
                .Setup(s => s.GerarRelatorioAsync())
                .ReturnsAsync((RelatorioDeDiversidadeModel)null!);

            var controller = new RelatoriosController(mockService.Object);

            // Act
            var resultado = await controller.GetRelatorioDiversidade();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Não foi possível gerar o relatório.", notFoundResult.Value);

            mockService.Verify(s => s.GerarRelatorioAsync(), Times.Once);
        }
    }
}