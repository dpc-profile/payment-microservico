```sh
# Tool necessaria
dotnet tool install --global PlantUmlClassDiagramGenerator
```

```sh
# Precisa ir na pasta src e a partir dela rodar o comando.
puml-gen checkout-api outUML -dir -createAssociation -allInOne -excludePaths bin,obj,Properties -excludeUmlBeginEndTags -ignore Private,Protected
```
