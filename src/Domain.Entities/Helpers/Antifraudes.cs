namespace Domain.Entities;

    public static class Antifraudes
    {
        // Aprova sempre o pagamento
        public static readonly AntifraudePolicy SemVerificacao = valor => true;

        // Reprova pagamentos acima de um limite definido
        public static AntifraudePolicy Limite(decimal limiteMaximo)
        {
            return valor => valor <= limiteMaximo;
        }

        // Aprova com probabilidade (simulação de risco)
        public static readonly AntifraudePolicy SimulacaoRisco = valor =>
        {
            var random = new Random();
            return random.NextDouble() > 0.2; // 80% de aprovação
        };
    }

