namespace Domain.Entities;

    public static class Cambios
    {
        // Não aplica nenhuma conversão
        public static readonly CambioPolicy SemTaxa = valor => valor;

        // Converte o valor com uma taxa fixa (exemplo: 5%)
        public static CambioPolicy TaxaFixa(decimal taxaPercentual)
        {
            return valor => valor * (1 + taxaPercentual / 100);
        }

        // Conversão com multiplicador customizado (ex: simular câmbio USD/BRL)
        public static CambioPolicy Conversao(decimal fator)
        {
            return valor => valor * fator;
        }
    }
