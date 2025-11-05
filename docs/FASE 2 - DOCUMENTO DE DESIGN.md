## Fase 2 – Design Orientado a Objetos: Processamento de Pagamentos

O design orientado a objetos do sistema de pagamentos é estruturado em torno de uma classe base concreta denominada **Pagamento**, responsável por orquestrar o **ritual fixo de processamento**.
Esse ritual segue sempre as mesmas etapas, nesta ordem: **Validar → Autorizar ou Capturar → Confirmar**.
A consistência do fluxo é garantida pela classe base, que define o comportamento comum e estabelece pontos de variação controlada para cada subtipo.

Os pontos de variação são representados por **métodos protegidos virtuais**, denominados ganchos. Esses ganchos permitem que subclasses especializem etapas específicas sem alterar o processo principal. Assim, a herança é usada de forma segura e previsível, garantindo que o ritual permaneça estável e coerente em todas as formas de pagamento.

As subclasses concretas — **PagamentoCartao**, **PagamentoPix** e **PagamentoBoleto** — representam as variações legítimas do mesmo processo, cada uma refinando apenas o necessário:

* PagamentoCartao realiza a autorização e captura junto à operadora;
* PagamentoPix gera e confirma o QR Code;
* PagamentoBoleto emite a linha digitável e confirma a compensação.

Todas são **classes seladas (sealed)** e não adicionam novos métodos públicos, assegurando o uso da herança por especialização e a aderência ao **Princípio da Substituição de Liskov (LSP)**.

---

### Regras do Princípio da Substituição de Liskov (LSP)

Para garantir que as subclasses possam substituir a classe base sem quebrar o comportamento esperado do sistema, o design segue três regras fundamentais:

1. **Substituibilidade:** qualquer componente que utilize um objeto do tipo Pagamento deve funcionar corretamente ao receber um PagamentoCartao, PagamentoPix ou PagamentoBoleto, sem necessidade de verificações de tipo ou conversões explícitas.
2. **Invariantes preservados:** as validações mínimas definidas na classe base devem ser mantidas ou fortalecidas nas subclasses, jamais enfraquecidas.
3. **Contratos de saída equivalentes:** o resultado final do processamento deve manter coerência e estabilidade, entregando sempre um comprovante válido e sem lançar exceções inesperadas em relação ao contrato da classe base.

---

### Eixos plugáveis por composição

Além da herança, o modelo incorpora **composição** para tratar políticas independentes e combináveis, implementadas por meio de **delegates** (funções injetáveis).
Esses delegates permitem variar comportamentos sem alterar a estrutura das classes, preservando baixo acoplamento e alta extensibilidade.

Os principais eixos de composição definidos são:

* **Antifraude:** responsável por decidir se o pagamento é aprovado ou rejeitado antes da autorização. Sua assinatura conceitual corresponde a uma função que recebe um valor e retorna uma decisão booleana.
* **Câmbio:** responsável por converter valores quando há pagamento em moeda estrangeira. Sua assinatura conceitual corresponde a uma função que recebe um valor e devolve o valor convertido.

Esses eixos são **plugáveis e intercambiáveis**, permitindo que diferentes estratégias de antifraude ou câmbio sejam aplicadas sem criação de novas subclasses, reforçando o uso adequado da composição para políticas independentes.

---

### Síntese do design

O modelo final combina **herança controlada** e **composição flexível**.
A herança garante um **ritual fixo e consistente** entre os diferentes tipos de pagamento, enquanto a composição permite **trocar políticas de negócio de forma independente**, sem romper o contrato base.
Esse design assegura **aderência ao LSP**, reduz o acoplamento e prepara o sistema para futuras extensões sem comprometer sua estabilidade.
