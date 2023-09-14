FROM rabbitmq:3.12

RUN set eux; \
	rabbitmq-plugins enable --offline rabbitmq_management; \
    # make sure the metrics collector is re-enabled (disabled in the base image for Prometheus-style metrics by default)
	rm -f /etc/rabbitmq/conf.d/20-management_agent.disable_metrics_collector.conf; \
    # grab "rabbitmqadmin" from inside the "rabbitmq_management-X.Y.Z" plugin folder
    # see https://github.com/docker-library/rabbitmq/issues/207
	cp /plugins/rabbitmq_management-*/priv/www/cli/rabbitmqadmin /usr/local/bin/rabbitmqadmin; \
	[ -s /usr/local/bin/rabbitmqadmin ]; \
	chmod +x /usr/local/bin/rabbitmqadmin; \
	apt-get update; \
	apt-get install -y --no-install-recommends python3; \
	rm -rf /var/lib/apt/lists/*; \
	rabbitmqadmin --version

EXPOSE 15671 15672

COPY --from=mcr.microsoft.com/dotnet/sdk:6.0-focal /usr/share/dotnet /usr/share/dotnet
ENV PATH=$PATH:/usr/share/dotnet

# Senhas: rabbitmq/NovaSenha
ARG RABBITMQ_KEY=jv56XA890j
ARG RABBITMQ_USER=UcRam2GuhbVt21eYdvaSWA==
ARG RABBITMQ_PASS=7ib79aXzVH4glhKCv1pkCA==
# ==============================

WORKDIR /app/libs
COPY libs ./

# ARG RABBITMQ_DEFAULT_USER=$(dotnet Crypto.dll decrypt $RABBITMQ_USER $RABBITMQ_KEY)
# ARG RABBITMQ_DEFAULT_PASS=$(dotnet Crypto.dll decrypt $RABBITMQ_PASS $RABBITMQ_KEY)

ENV RABBITMQ_ERLANG_COOKIE=SWQOKODSQALRPCLNMEQG
ENV RABBITMQ_DEFAULT_VHOST=/
# ENV RABBITMQ_DEFAULT_USER=$tempuser
# ENV RABBITMQ_DEFAULT_PASS=$temppass