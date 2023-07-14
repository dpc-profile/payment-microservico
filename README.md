# Payment Microservices
Projeto de aprendizado de microserviços, baseado no [Intensivão Microsserviços da FullCycle](https://www.youtube.com/playlist?list=PL5aY_NrL1rjuzBYy1Gro6IVDF1BPkPK_m), que tem como objetivo ter varias APIs comunicando entre sí,  mensageria, orquestrador e Service Mesh. 
___
## Tecnologias
Diferente do projeto original, que foi feito em Golang, todas as aplicações são feitas usando a versão 6 do .NET. As APIs são feitas em ASP.NET e o client-app é um MVC em ASP.NET. 
Será usado o Kubernetes para orquestrar os serviços, RabbitMQ para mensageria e o Istio para Service Mesh.

![](img/IntensivoMicroservicos.drawio.png)
___
## Serviços

### client-app(MVC)
Parte front-end que se comunica com as APIs

### produto-api
Retorna do "banco de dados" todos os produtos ou um produto especifico.

```sh
GET http://localhost:5034/api/v1/Produto

GET http://localhost:5034/api/v1/Produto/{uuid-do-produto}
```
___
## Pendente

### checkout-api
Recebe as informações de um produto e confirmar a compra

### order-api
Recebe a mensagem do __checkout-api__, consulta o __process-card-api(Nome temporario)__ e processa a ordem.

### process-card-api(Nome temporario)
Simula uma confirmação de pagamento de cartão de credito.

___
## Executando(Dev)
Para executar o projeto, é necessario entrar em cada um dos projetos da pasta __src__ e executar em ordem:

- 1º. produto-api
- 2º client-app (Por ultimo)
```sh
$ dotnet run
#ou
$ dotnet watch
```
___

## Variaveis de Ambiente
De momento não é preciso se preocupar com isso, as variaveis de ambiente estão sendo definidas nos __appsettings.json__, mas apenas para deixar anotado.

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