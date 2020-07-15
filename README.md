# Integração de Pedidos Bling - TagPlus

Essa é uma integração simples, que encontra pedidos de venda no Bling e cadastra como pedidos no TagPlus.
Foi testada no .net Core, foi testada na versão 3.1 no sistema operacional Windows.

## Para desenvolver

```
git clone https://github.com/svbgabriel/bling-integration-tagplus
cd bling-integration-tagplus
dotnet restore
```

## Para gerar um executável

```
dotnet publish --runtime win-x64 --self-contained true -p:PublishSingleFile=true --framework netcoreapp3.1 --configuration Release
```

Um executável para o Windows 64-bits vai estar disponível em bling-integration-tagplus\bin\Release\netcoreapp3.1\win-x64\publish

## Para executar

Para executar é necessário preencher o arquivo de configurações, esse arquivo deve ter o nome ".env". O arquivo ".env.example" pode ser usado como base.
As configurações são do tipo "chave e valor" e estão descritas no arquivo ".env.example".

Com a configuração realizada, ele pode ser executado com o comando abaixo:

```
dotnet run
```
