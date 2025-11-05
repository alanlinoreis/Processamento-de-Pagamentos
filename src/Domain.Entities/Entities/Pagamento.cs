namespace Domain.Entities;

    public delegate bool AntifraudePolicy(decimal valor);
    public delegate decimal CambioPolicy(decimal valor);

    public class Pagamento
    {
        public decimal Valor { get; }

        private readonly AntifraudePolicy _antifraude;
        private readonly CambioPolicy _cambio;

        public Pagamento(
            decimal valor,
            AntifraudePolicy? antifraude = null,
            CambioPolicy? cambio = null)
        {
            if (valor <= 0) throw new ArgumentOutOfRangeException(nameof(valor));

            Valor = valor;
            _antifraude = antifraude ?? (v => true);
            _cambio = cambio ?? (v => v);
        }

        public string Processar()
        {
            Validar();

            if (!_antifraude(Valor))
                throw new InvalidOperationException("Pagamento reprovado pelo antifraude.");

            var autorizado = AutorizarOuCapturar(Valor);
            var convertido = _cambio(autorizado);

            return Confirmar(convertido);
        }

        // Ganchos (pontos de variação)
        protected virtual void Validar() { }
        protected virtual decimal AutorizarOuCapturar(decimal valor) => valor;
        protected virtual string Confirmar(decimal valorFinal) => $"Pagamento confirmado: {valorFinal:C}";
    }

