# Payment Microservices
Projeto de aprendizado de microserviços, baseado no [Intensivão Microsserviços da FullCycle](https://www.youtube.com/playlist?list=PL5aY_NrL1rjuzBYy1Gro6IVDF1BPkPK_m), que tem como objetivo ter varias APIs comunicando entre sí,  mensageria, orquestrador e Service Mesh. 
___
## Tecnologias
Diferente do projeto original, que foi feito em Golang, todas as aplicações são feitas usando a versão 6 do .NET. As APIs são feitas em ASP.NET e o client-app é um MVC em ASP.NET. 
Será usado o Kubernetes para orquestrar os serviços, RabbitMQ para mensageria e o Istio para Service Mesh.

![Diagrama do Projeto](img/IntensivoMicroservicos.drawio.png)

___
## Serviços

### client-app(MVC)
Parte front-end que se comunica com as APIs.

### produto-api
Retorna do "banco de dados" todos os produtos ou um produto especifico.

```sh
GET http://localhost:5034/api/v1/Produto

GET http://localhost:5034/api/v1/Produto/{uuid-do-produto}
```

___
## Pendente

### checkout-api
Recebe as informações de um produto e confirmar a compra.

### order-api
Recebe a mensagem do **checkout-api**, consulta o **process-card-api(Nome temporario)** e processa a ordem.

### process-card-api(Nome temporario)
Simula uma confirmação de pagamento de cartão de credito.

___
## Responsabilidades
### client-app
- Fornece acesso ao usuário.
- Quando o usuário finalizar o pedido, os dados desse pedido devem ser postado como mensagem no RabbitMQ.
    - Os Dados são:
        - ProdutoId
        - Nome
        - Email
        - Telefone
- Ao finalizar o pedido, exibir uma mensagem informando que o pedido está sendo processado.

### ckeckout-api
- Ao pegar uma mensagem da fila de checkout, verificar se os dados do produto ainda são validos.
- Confirmando os dados, postar o pedido na fila de order.

### order-api
- Ao pegar uma mensagem da fila de order, (postar mensagem no RabbitMQ para ser processado pelo **process-card-api(Nome temporario)**)????

### process-card-api(Nome temporario)
- Ao pegar uma mensagem, altera seu status de "Pendente" para "Aprovado".

___
## Executando(Dev)
Para executar o projeto, é necessario entrar em cada um dos projetos da pasta **src** e executar em ordem:

- 1º. produto-api
- 2º. client-app (Por ultimo)
```sh
$ dotnet run
#ou
$ dotnet watch
```

___
## Variaveis de Ambiente
De momento não é preciso se preocupar com isso, as variaveis de ambiente estão sendo definidas nos **appsettings.json**, mas apenas para deixar anotado.

### client-app

- PRODUTO_API - Endereço do serviço produto-api
```sh
# Exemplo
PRODUTO_API=http://localhost:5034
```
- API_VERSION - Versão da API, no momento está a v1
```sh
# Exemplo
API_VERSION=v1
```