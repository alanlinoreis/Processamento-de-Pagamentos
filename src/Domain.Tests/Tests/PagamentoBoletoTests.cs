using Xunit;
using Domain.Entities;
using System;

namespace Domain.Tests;

    public class PagamentoBoletoTests
    {
        [Fact(DisplayName = "Processar deve seguir o mesmo ritual da classe base (LSP)")]
        public void Processar_DeveSeguirRitual_BaseLSP()
        {
            // Arrange
            Pagamento pagamento = new PagamentoBoleto(
                valor: 500m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.SemTaxa
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Contains("Boleto", recibo);
        }

        [Fact(DisplayName = "Processar deve gerar recibo aguardando compensação bancária")]
        public void Recibo_DeveConterStatusDeCompensacao()
        {
            // Arrange
            var pagamento = new PagamentoBoleto(
                valor: 250m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.SemTaxa
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Contains("aguardando compensação", recibo, StringComparison.OrdinalIgnoreCase);
        }

        [Fact(DisplayName = "Processar deve lançar exceção se antifraude reprovar")]
        public void Processar_DeveLancarExcecao_QuandoAntifraudeFalha()
        {
            // Arrange
            var pagamento = new PagamentoBoleto(
                valor: 2000m,
                antifraude: Antifraudes.Limite(1000m),
                cambio: Cambios.SemTaxa
            );

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pagamento.Processar());
        }

        [Fact(DisplayName = "Processar deve aplicar câmbio corretamente no valor do boleto")]
        public void Processar_DeveAplicarCambioCorretamente()
        {
            // Arrange
            var pagamento = new PagamentoBoleto(
                valor: 100m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.Conversao(5.35m) // simula USD → BRL
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Contains("R$ 535,00", recibo);
        }

        [Fact(DisplayName = "Recibo deve refletir o valor processado e manter contrato coerente")]
        public void Recibo_DeveManterContratoCoerenteComValor()
        {
            // Arrange
            var pagamento = new PagamentoBoleto(
                valor: 800m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.SemTaxa
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Matches(@"Boleto.*800", recibo);
        }
    }

