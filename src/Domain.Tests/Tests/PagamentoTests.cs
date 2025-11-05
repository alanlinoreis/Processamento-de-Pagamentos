using Xunit;
using Domain.Entities;
using System;

namespace Domain.Tests;

    public class PagamentoTests
    {
        [Fact(DisplayName = "Processar deve seguir o ritual fixo e retornar recibo padrão")]
        public void Processar_DeveExecutarRitualPadrao()
        {
            // Arrange
            var pagamento = new Pagamento(
                valor: 100m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.SemTaxa
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Contains("Pagamento confirmado", recibo);
        }

        [Fact(DisplayName = "Processar deve lançar exceção quando antifraude reprova")]
        public void Processar_DeveLancarExcecao_QuandoAntifraudeFalha()
        {
            // Arrange
            var pagamento = new Pagamento(
                valor: 2000m,
                antifraude: Antifraudes.Limite(1000m),
                cambio: Cambios.SemTaxa
            );

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pagamento.Processar());
        }

        [Fact(DisplayName = "Processar deve aplicar política de câmbio corretamente")]
        public void Processar_DeveAplicarCambioCorretamente()
        {
            // Arrange
            var pagamento = new Pagamento(
                valor: 100m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.TaxaFixa(10m) // +10%
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Contains("R$ 110,00", recibo); // formato com non-breaking space (pt-BR)
        }

        [Fact(DisplayName = "Construtor deve lançar exceção para valor inválido")]
        public void Construtor_DeveLancarExcecao_ParaValorInvalido()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Pagamento(
                    valor: 0m,
                    antifraude: Antifraudes.SemVerificacao,
                    cambio: Cambios.SemTaxa
                )
            );
        }

        [Fact(DisplayName = "Processar deve produzir saída coerente com valor processado")]
        public void Processar_DeveRetornarReciboCoerenteComValor()
        {
            // Arrange
            var pagamento = new Pagamento(
                valor: 250m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.SemTaxa
            );

            // Act
            var recibo = pagamento.Processar();

            // Assert
            Assert.Matches(@"Pagamento confirmado: R\$.*250", recibo);
        }
    }

