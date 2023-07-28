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

### checkout-api
Consome uma mensagem com as informações do usuário e o uuid do produto, checa o produto com **produto-api**, "valida" as informações do usuário e postar na fila para o **order-api** consumir.

```sh
# Post feito pelo serviço MessageConsumer, para dar inicio ao processo.
POST http://localhost:5221/api/v1/Checkout
Content-Type: application/json

{
    "ProdutoUuid":"7d3af8b1-3755-4772-8f8d-72e25b1abefa",
    "UsuarioNome":"daniel",
    "UsuarioEmail":"daniel@gmail.com",
    "UsuarioTelefone":"11 2222-4444"
}
```

```sh
# Post final, após as informações serem validadas, é criada a mensagem para o RabbitMQ.
POST http://localhost:5221/api/v1/MessageProducer
Content-Type: application/json

{
    "ProdutoUuid":"7d3af8b1-3755-4772-8f8d-72e25b1abefa",
    "UsuarioNome":"daniel",
    "UsuarioEmail":"daniel@gmail.com",
    "UsuarioTelefone":"11 2222-4444",
    "CreatedAt":"2023-07-26T17:15:03.1714833-03:00"
}
```

___
## Pendente

### order-api
Consome mensagens postada pelo **checkout-api** e pelo **process-card-api**, cria um uuid para a transação, grava a transação em um banco de dados e posta uma mensagem para ser consumida pelo **process-card-api**. 
Se a transação já está com o status "Aprovado" o processo é encerrado com sucesso. Se a transação está com status "Rejeitado", algumas tentativas a mais serão feitas, e em caso de falha, o pedido será cancelado.

### process-card-api
Simula uma confirmação de pagamento de cartão de credito, vai alterar o status para "Aprovado" ou "Rejeitado".

___
## Responsabilidades
### client-app
- Fornece uma interface para o usuário visualizar os produtos.
- Fornece uma interface para o usuário realizar o pedido.
- Postar na exchange **checkout_ex** o pedido finalizado.
    - Exemplo dos dados:
        - ProdutoUuid
        - UsuarioNome
        - UsuarioEmail
        - UsuarioTelefone

- (**Não Implementado**) Fornecer meios do usuário consultar o status do pedido em uma pagina, ou com o UUID da transação, ou passando as mesmas informações do pedido(nome, email e telefone). Ao usar as informações do pedido, deve ser mostrado todos os pedidos feitos.

### produto-api
- Consulta em um "banco de dados", retornando todos os produtos, ou o especificado pelo uuid.

### ckeckout-api
- Consumir mensagems da fila **checkout_queue**.
- Validar o produto consultado o **produto-api** para garantir se o produto ainda é valido.
- Validação os dados do usuário(validação fake).
- Posta a mensagem na exchange **order_ex**.
    - Exemplo dos dados:
        - ProdutoUuid
        - UsuarioNome
        - UsuarioEmail
        - UsuarioTelefone
        - CreatedAt

### order-api
- Consumir mensagens da fila **order_queue**.
- Adicionar o UUID para o pedido.
- Salvar o pedido no Redis.
- Postar mensagens em **process_card_ex** para ser consumida pelo **process-card-api**.
- Finaliza o processo do pedido.

### process-card-api
- Consumir mensagens da fila **processed_order_queue**.
- Altera o status da ordem de "Pendente" para "Aprovado" ou "Rejeitado".
- Atualiza o UpdatedAt com o datetime atual.
- Posta a mensagem na exchange **order_ex**.

___
## Executando(Dev)
Para executar o projeto, é necessario entrar em cada um dos projetos da pasta **src** e executar em ordem:

- 1º
    produto-api
    checkout-api
- 2º 
    client-app (Por ultimo)
```sh
$ dotnet run
#ou
$ dotnet watch
```

___
## Variaveis de Ambiente
De momento não é preciso se preocupar com isso, as variaveis de ambiente estão sendo definidas nos **appsettings.json**, mas apenas para informar:

### client-app / checkout-api
- PRODUTO:URL - Endereço do serviço produto-api
```sh
PRODUTO:URL=http://localhost:5034
#ou
PRODUTO__URL=http://localhost:5034
```

- PRODUTO:VERSION - Versão da API, no momento a versão é v1
```sh
PRODUTO:VERSION=v1
#ou
PRODUTO__VERSION=v1
```

### checkout-api
- RABBITMQ:QUEUE_CONSUME - Fila que é consumida.
```sh
RABBITMQ:QUEUE_CONSUME=checkout_queue
#ou
RABBITMQ__QUEUE_CONSUME=checkout_queue
```

- RABBITMQ:EX_PRODUCE - Exchange aonde é postada mensagem.
```sh
RABBITMQ:EX_PRODUCE=order_ex
#ou
RABBITMQ__EX_PRODUCE=order_ex
```
