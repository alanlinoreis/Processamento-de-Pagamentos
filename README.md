# 🧾 Pagamentos (Cartão, PIX e Boleto)

Projeto desenvolvido por **Alan Lino dos Reis** para a disciplina de **Programação Orientada a Objetos (UTFPR Medianeira)**.  
Para executar, utilize os comandos:  
```bash
dotnet restore
dotnet run
dotnet test
```

A solução usa herança no padrão Template Method para estruturar o fluxo de pedidos e composição via delegates (Func<decimal, decimal>) para injetar políticas de frete e promoção dinamicamente.