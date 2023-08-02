```sh
## Cria a exchange
rabbitmqadmin -u rabbitmq -p rabbitmq declare exchange name=checkout_ex type=direct
rabbitmqadmin -u rabbitmq -p rabbitmq declare exchange name=order_ex type=direct
rabbitmqadmin -u rabbitmq -p rabbitmq declare exchange name=process-card_ex type=direct

## Cria a fila
rabbitmqadmin -u rabbitmq -p rabbitmq declare queue name=checkout_queue durable=true
rabbitmqadmin -u rabbitmq -p rabbitmq declare queue name=order_queue durable=true
rabbitmqadmin -u rabbitmq -p rabbitmq declare queue name=process-card_queue durable=true

## Faz o bind
rabbitmqadmin -u rabbitmq -p rabbitmq --vhost="/" declare binding source="checkout_ex" destination_type="queue" destination="checkout_queue" routing_key=""
rabbitmqadmin -u rabbitmq -p rabbitmq --vhost="/" declare binding source="order_ex" destination_type="queue" destination="order_queue" routing_key=""
rabbitmqadmin -u rabbitmq -p rabbitmq --vhost="/" declare binding source="process-card_ex" destination_type="queue" destination="process-card_queue" routing_key=""

## Se não passar o usuario e senha (-u, -p) ele vai usar o padrão, que seria o guest
```
