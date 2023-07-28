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
Consome uma mensagem com as informações do usuário e do uuid do produto, consulta as informações de produto no **produto-api** e valida o que foi recebido, "valida" as informações do usuário e postar na fila as informações do produto e do usuário, para o **order-api** consumir.

```sh
# Post feito pelo serviço MessageConsumer, para dar inicio ao processo.
POST http://localhost:5221/api/v1/Checkout
Content-Type: application/json

{
    "ProdutoUuid":"7d3af8b1-3755-4772-8f8d-72e25b1abefa",
    "UsuarioNome":"daniel",
    "UsuarioEmail":"daniel@gmail.com",
    "UsuarioTelefone":"11 2222-4444",
    "CreatedAt":"2023-07-26T17:15:03.1714833-03:00"
}
```

```sh
# Post final, aonde é produzido a mensagem para o RabbitMQ.
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
Consumir a mensagem postada pelo **checkout-api**, consulta o **process-card-api(Nome temporario)** e processa a ordem.

### process-card-api(Nome temporario)
Simula uma confirmação de pagamento de cartão de credito.

___
## Responsabilidades
### client-app
- Fornece acesso ao usuário.
- Quando o usuário finalizar o pedido, os dados desse pedido devem ser postado na fila **checkout_ex**.
    - Os dados são:
        - ProdutoUuid
        - UsuarioNome
        - UsuarioEmail
        - UsuarioTelefone
- Ao finalizar o pedido, exibir uma mensagem informando que o pedido está sendo processado.
- (**Talves para implementar**) Nessa pagina, o usuário deve ter acesso ao ID da transação e um acompanhamento visual dos processos finalizandos.
- (**Talves para implementar**) O usuário poderá consultar o status do pedido em uma aba, passando as mesmas informações do pedido(nome, email e telefone).

### produto-api
- Consulta em um "banco de dados", retornando todos os produtos, ou o especificado pelo uuid.

### ckeckout-api
- Ao consumir uma mensagem da fila **checkout_queue**, consulta o **produto-api** usando o ProdutoUuid.
- Coleta os dados do pedido.
- Após consultar, ele postara a mensagem na exchange **order_ex**, com as informações do produto, junto com as do usuário.
    - Os dados são:
        - ProdutoUuid
        - UsuarioNome
        - UsuarioEmail
        - UsuarioTelefone

### order-api
- Ao consumir uma mensagem da fila **order_ex**, postar mensagem em **process_card_ex** para ser consumida pelo **process-card-api**
- Ao consumir uma mensagem da fila **processed_order_ex**, esse pedido será gravado no banco de dados, com isso finalizando o processo.

### process-card-api
- Ao consumir uma mensagem, altera seu status de "Pendente" para "Aprovado", e posta a mensagem novamente para o **order-api** finalizar o processo ao todo.
- Ele postara a mensagem na fila **processed_order_ex**.
- Ao alterar o status, o campo UpdatedAt também deve ser preenchido com o datetime

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
