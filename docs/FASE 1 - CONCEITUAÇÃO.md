## Fase 1 – Conceituação: Processamento de Pagamentos (Cartão, PIX e Boleto)

O processamento de pagamentos segue um **ritual fixo e comum** para todos os meios disponíveis na plataforma, composto pelas etapas: **Validar → Autorizar/Capturar → Confirmar**.
Apesar de compartilharem o mesmo fluxo, cada tipo de pagamento possui **variações específicas** em uma ou mais etapas do processo.

* **Cartão:** envolve a validação de dados do titular, a autorização junto à operadora e a captura final do valor autorizado.
* **PIX:** requer a geração e validação de um QR Code, além da confirmação instantânea após o pagamento.
* **Boleto:** realiza a validação de dados do sacado, emite a linha digitável e aguarda a confirmação de compensação bancária.

Essas diferenças justificam o uso de **herança por especialização**, onde uma classe base `Pagamento` define o **ritual fixo** do processo (`Processar()`), e as subclasses (`PagamentoCartao`, `PagamentoPix`, `PagamentoBoleto`) apenas **refinam** os pontos de variação (métodos protegidos e sobrescritos), sem alterar o fluxo principal.

Entretanto, há políticas complementares que independem do tipo de pagamento, como **antifraude** (verificação adicional antes da autorização) e **câmbio** (conversão de valores em moeda estrangeira).
Esses comportamentos não pertencem a uma “espécie” de pagamento, mas sim a **eixos de variação independentes**, sendo mais adequadamente tratados por **composição**, através de **delegates** injetáveis que podem ser trocados livremente.

Em resumo:

* **Herança:** define a especialização dos tipos de pagamento, preservando o ritual fixo.
* **Composição:** oferece flexibilidade para aplicar políticas independentes, como antifraude e câmbio, sem criar novas subclasses.

Essa combinação garante um design orientado a objetos **coeso, extensível e aderente ao Princípio da Substituição de Liskov (LSP)**, mantendo o cliente agnóstico ao tipo específico de pagamento enquanto permite variação controlada de comportamento.
