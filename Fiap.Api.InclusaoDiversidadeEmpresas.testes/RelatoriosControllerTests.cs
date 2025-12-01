using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.testes
{
    public class RelatorioServiceTests
    {
        [Fact]
        public async Task GerarRelatorioAsync_DeveGerarRelatorioCorretamente()
        {
            // Arrange
            var mockColaboradorService = new Mock<IColaboradorService>();

            var colaboradoresFake = new List<Colaborador>
            {
                new Colaborador {
                    NomeColaborador = "A", Email = "a@a.com", Senha = "123",
                    Departamento = "TI",
                    GeneroColaborador = "Feminino",
                    EtniaColaborador = "Preta",
                    TemDisabilidade = true
                },

                new Colaborador {
                    NomeColaborador = "B", Email = "b@b.com", Senha = "123",
                    Departamento = "TI",
                    GeneroColaborador = "Masculino",
                    EtniaColaborador = "Branca",
                    TemDisabilidade = false
                },

                new Colaborador {
                    NomeColaborador = "C", Email = "c@c.com", Senha = "123",
                    Departamento = "TI",
                    GeneroColaborador = "Não Binário",
                    EtniaColaborador = "Parda",
                    TemDisabilidade = false
                },

                new Colaborador {
                    NomeColaborador = "D", Email = "d@d.com", Senha = "123",
                    Departamento = "TI",
                    GeneroColaborador = "Feminino",
                    EtniaColaborador = "Branca",
                    TemDisabilidade = true
                }
            };

            mockColaboradorService
                .Setup(s => s.GetAllColaboradores())
                .ReturnsAsync(colaboradoresFake);

            var service = new RelatorioService(mockColaboradorService.Object);

            // Act
            var relatorio = await service.GerarRelatorioAsync();

            // Assert
            Assert.NotNull(relatorio);
            Assert.Equal(4, relatorio.TotalColaborador);
            Assert.Equal(2, relatorio.ContagemDeMulheres);               // 2 Feminino
            Assert.Equal(2, relatorio.ContagemDePessoasNegras);          // 1 Preta + 1 Parda
            Assert.Equal(1, relatorio.ContagemDePessoasLgbt);            // 1 Não Binário
            Assert.Equal(2, relatorio.ContagemDePessoasComDesabilidade); // 2 true
        }

        [Fact]
        public async Task GerarRelatorioAsync_DeveRetornarValoresZerados_QuandoSemColaboradores()
        {
            // Arrange
            var mockColaboradorService = new Mock<IColaboradorService>();

            mockColaboradorService
                .Setup(s => s.GetAllColaboradores())
                .ReturnsAsync(new List<Colaborador>());

            var service = new RelatorioService(mockColaboradorService.Object);

            // Act
            var relatorio = await service.GerarRelatorioAsync();

            // Assert
            Assert.NotNull(relatorio);
            Assert.Equal(0, relatorio.TotalColaborador);
            Assert.Equal(0, relatorio.ContagemDeMulheres);
            Assert.Equal(0, relatorio.ContagemDePessoasNegras);
            Assert.Equal(0, relatorio.ContagemDePessoasLgbt);
            Assert.Equal(0, relatorio.ContagemDePessoasComDesabilidade);
        }
    }
}
