# Integração de Pedidos Bling - TagPlus

Essa é uma integração simples, que encontra pedidos de venda no Bling e cadastra como pedidos no TagPlus.
Foi testada no .net Core, foi testada na versão 3.1 no sistema operacional Windows.

## Para executar

```
git clone https://github.com/svbgabriel/bling-integration-tagplus
cd bling-integration-tagplus
dotnet restore
dotnet run
```

## Para gerar um executável

```
dotnet publish --runtime win-x64 --self-contained true -p:PublishSingleFile=true --framework netcoreapp3.1 --configuration Release
```

O executável vai estar disponível em bling-integration-tagplus\bin\Release\netcoreapp3.1\win-x64\publish