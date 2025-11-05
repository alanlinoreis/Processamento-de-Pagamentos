namespace Domain.Entities;

    public sealed class PagamentoPix : Pagamento
    {
        public PagamentoPix(
            decimal valor,
            AntifraudePolicy? antifraude = null,
            CambioPolicy? cambio = null)
            : base(valor, antifraude, cambio)
        {
        }

        protected override void Validar()
        {
            Console.WriteLine("Validando dados da conta e chave PIX...");
        }

        protected override decimal AutorizarOuCapturar(decimal valor)
        {
            Console.WriteLine("Gerando QR Code PIX para o valor informado...");
            // PIX não tem desconto ou taxa aqui
            return valor;
        }

        protected override string Confirmar(decimal valor)
        {
            return $"Pagamento via PIX confirmado. Valor recebido: {valor:C}";
        }
    }

