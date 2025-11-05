namespace Domain.Entities;

    public sealed class PagamentoCartao : Pagamento
    {
        public PagamentoCartao(
            decimal valor,
            AntifraudePolicy? antifraude = null,
            CambioPolicy? cambio = null)
            : base(valor, antifraude, cambio)
        {
        }

        protected override void Validar()
        {
            Console.WriteLine("Validando dados do cartão (número, validade, CVV)...");
        }

        protected override decimal AutorizarOuCapturar(decimal valor)
        {
            Console.WriteLine($"Autorizando pagamento de {valor:C} com a operadora...");
            // Simula taxa de operadora de 2,5%
            return valor * 0.975m;
        }

        protected override string Confirmar(decimal valor)
        {
            return $"Pagamento com cartão confirmado no valor de {valor:C}.";
        }
    }

