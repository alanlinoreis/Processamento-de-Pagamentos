namespace Domain.Entities;

    public sealed class PagamentoBoleto : Pagamento
    {
        public PagamentoBoleto(
            decimal valor,
            AntifraudePolicy? antifraude = null,
            CambioPolicy? cambio = null)
            : base(valor, antifraude, cambio)
        {
        }

        protected override void Validar()
        {
            Console.WriteLine("Validando dados do sacado e CPF/CNPJ...");
        }

        protected override decimal AutorizarOuCapturar(decimal valor)
        {
            Console.WriteLine("Emitindo boleto bancário e gerando linha digitável...");
            // Nenhuma taxa, apenas retorno do valor original
            return valor;
        }

        protected override string Confirmar(decimal valor)
        {
            return $"Boleto no valor de {valor:C} emitido e aguardando compensação bancária.";
        }
    }

