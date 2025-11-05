using System;
using Domain.Entities;

namespace Domain.App;

    class Program
{
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Demonstração de Processamento de Pagamentos ===\n");

            // Pagamento via Cartão
            var pagamentoCartao = new PagamentoCartao(
                valor: 500m,
                antifraude: Antifraudes.Limite(1000m),
                cambio: Cambios.SemTaxa
            );
            Console.WriteLine(pagamentoCartao.Processar());
            Console.WriteLine();

            // Pagamento via PIX
            var pagamentoPix = new PagamentoPix(
                valor: 1500m,
                antifraude: Antifraudes.SemVerificacao,
                cambio: Cambios.TaxaFixa(3m) // simula taxa de câmbio internacional
            );
            Console.WriteLine(pagamentoPix.Processar());
            Console.WriteLine();

            // Pagamento via Boleto
            var pagamentoBoleto = new PagamentoBoleto(
                valor: 800m,
                antifraude: Antifraudes.SimulacaoRisco,
                cambio: Cambios.Conversao(5.35m) // simula conversão USD → BRL
            );
            Console.WriteLine(pagamentoBoleto.Processar());
            Console.WriteLine();

            // Teste de Substituição (LSP)
            ExecutarProcessamento(pagamentoCartao);
            ExecutarProcessamento(pagamentoPix);
            ExecutarProcessamento(pagamentoBoleto);

            Console.WriteLine("\n=== Fim da Demonstração ===");
        }

        private static void ExecutarProcessamento(Pagamento pagamento)
        {
            Console.WriteLine("\n[LSP Teste] Processando pagamento genérico...");
            Console.WriteLine(pagamento.Processar());
        }
    }
