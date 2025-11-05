using Xunit;
using Domain.Entities;
using System;

namespace Domain.Tests;

    public class PagamentoCartaoTests
    {
        [Fact(DisplayName = "Processar deve seguir o mesmo ritual da classe base (LSP)")]
        public void Processar_DeveManterRitualFixo_BaseLSP()
        {
            // Arrange
            Pagamento pagamento = new PagamentoCartao(
                valor: 300m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.SemTaxa
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Contains("Pagamento com cartão confirmado", recibo);
        }

        [Fact(DisplayName = "Processar deve aplicar taxa de operadora corretamente")]
        public void Processar_DeveAplicarTaxaOperadora()
        {
            // Arrange
            var pagamento = new PagamentoCartao(
                valor: 1000m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.SemTaxa
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            // 2,5% de taxa → valor final = 975
            Assert.Contains("R$ 975,00", recibo);
        }

        [Fact(DisplayName = "Processar deve lançar exceção se antifraude reprovar")]
        public void Processar_DeveLancarExcecao_QuandoAntifraudeFalha()
        {
            // Arrange
            var pagamento = new PagamentoCartao(
                valor: 2000m,
                antifraude: Antifraudes.Limite(1000m),
                cambio: Cambios.SemTaxa
            );

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pagamento.Processar());
        }

        [Fact(DisplayName = "Processar deve aplicar câmbio sobre valor líquido da operadora")]
        public void Processar_DeveAplicarCambioSobreValorLiquido()
        {
            // Arrange
            var pagamento = new PagamentoCartao(
                valor: 1000m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.TaxaFixa(10m) // +10%
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            // valor líquido 975 + 10% = 1072,5
            Assert.Contains("R$ 1.072,50", recibo);
        }

        [Fact(DisplayName = "Recibo deve conter descrição específica de cartão")]
        public void Recibo_DeveConterDescricaoDeCartao()
        {
            // Arrange
            var pagamento = new PagamentoCartao(
                valor: 250m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.SemTaxa
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Contains("cartão confirmado", recibo, StringComparison.OrdinalIgnoreCase);
        }
    }

