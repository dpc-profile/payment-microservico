## Visualizando aplicação
### Configurando Istio
```sh
# Doc
https://istio.io/latest/docs/setup/getting-started/#install

# Faz a instalação do istio
$ istioctl install --set profile=demo -y

# Injeta o istio no namespace, assim toda aplicação implementada vai ter o proxy do istio
$ kubectl label namespace default istio-injection=enabled
```

### Configurando Kiali
```sh
# Instala os aplicativos de monitoria
$ kubectl apply -f k8s/telemetry 

# Abre o dashboard do kiali
$ istioctl dashboard kiali 
```