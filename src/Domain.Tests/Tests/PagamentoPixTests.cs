using Xunit;
using Domain.Entities;
using System;

namespace Domain.Tests;

    public class PagamentoPixTests
    {
        [Fact(DisplayName = "Processar deve manter o ritual fixo da classe base (LSP)")]
        public void Processar_DeveManterRitual_BaseLSP()
        {
            // Arrange
            Pagamento pagamento = new PagamentoPix(
                valor: 500m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.SemTaxa
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Contains("PIX confirmado", recibo);
        }

        [Fact(DisplayName = "Processar deve gerar recibo com identificação de PIX")]
        public void Processar_DeveGerarReciboPIX()
        {
            // Arrange
            var pagamento = new PagamentoPix(
                valor: 300m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.SemTaxa
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Contains("PIX", recibo, StringComparison.OrdinalIgnoreCase);
        }

        [Fact(DisplayName = "Processar deve aplicar câmbio sobre o valor PIX")]
        public void Processar_DeveAplicarCambio()
        {
            // Arrange
            var pagamento = new PagamentoPix(
                valor: 1000m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.TaxaFixa(5m)
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            // +5% de câmbio = 1050
            Assert.Contains("R$ 1.050,00", recibo);
        }

        [Fact(DisplayName = "Processar deve lançar exceção quando antifraude reprova")]
        public void Processar_DeveLancarExcecao_QuandoAntifraudeFalha()
        {
            // Arrange
            var pagamento = new PagamentoPix(
                valor: 2000m,
                antifraude: Antifraudes.Limite(1000m),
                cambio: Cambios.SemTaxa
            );

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pagamento.Processar());
        }

        [Fact(DisplayName = "Recibo deve manter contrato coerente com valor final")]
        public void Recibo_DeveManterContratoCoerente()
        {
            // Arrange
            var pagamento = new PagamentoPix(
                valor: 400m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.SemTaxa
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Matches(@"PIX confirmado.*400", recibo);
        }
    }

