```sh
## Cria a exchange
rabbitmqadmin -u rabbitmq -p rabbitmq declare exchange name=teste2_ex type=direct

## Cria a fila
rabbitmqadmin -u rabbitmq -p rabbitmq declare queue name=teste_queue durable=true

## Faz o bind
rabbitmqadmin -u rabbitmq -p rabbitmq --vhost="/" declare binding source="teste2_ex" destination_type="queue" destination="teste_queue" routing_key=""

## Se não passar o usuario e senha (-u, -p) ele vai usar o padrão, que seria o guest
```
