# Prova BonifiQ Backend

[![DotNet CI](https://github.com/marcelohnq/prova-bonifiq/actions/workflows/ci.yml/badge.svg)](https://github.com/marcelohnq/prova-bonifiq/actions/workflows/ci.yml)

## Para começar

Antes de rodar o projeto, você precisa rodar as migrations. Para isso, primeiro instale o [EF Tools](https://learn.microsoft.com/en-us/ef/core/get-started/overview/install#get-the-entity-framework-core-tools):
```
dotnet tool install --global dotnet-ef
```
Agora, pode rodar as migrations de fato:
```
dotnet ef database update 
``` 

Pronto, o projeto já criou as tabelas e alguns registros no seu localDB. 

Rode o projeto e, se tudo deu certo, você deverá ver uma página do Swagger com as APIs que utilizaremos no teste.

## Testes

O projeto possui testes unitários e testes de integração. Todos os testes são utilizados para medir a cobertura de código.

Os testes foram construídos com:

- [XUnit](https://xunit.net/)
- Microsoft.AspNetCore.Mvc.Testing
- [Moq](https://github.com/devlooped/moq)
- [Testcontainers.MsSql](https://dotnet.testcontainers.org/modules/mssql/) / docker
- coverlet.collector

Para rodar os testes de integração é necessário ter o [Docker](https://www.docker.com/) configurado no seu PC. O pacote [Testcontainers](https://testcontainers.com/) irá construir/destruir containers automaticamente para cada classe de teste, tornando cada teste isolados e independentes, com seu próprio banco de dados SQL Server.

## Tarefas

As tarefas solicitadas são melhor detalhadas nas seguintes issues, e suas PR's associados:

- [#1 Parte1Controller](https://github.com/marcelohnq/prova-bonifiq/issues/1)
- [#2 Parte2Controller](https://github.com/marcelohnq/prova-bonifiq/issues/2)
- [#3 Parte3Controller](https://github.com/marcelohnq/prova-bonifiq/issues/3)
- [#4 Parte4Controller](https://github.com/marcelohnq/prova-bonifiq/issues/4)

Além disso, foram realizadas outras melhorias:

- [Refatorar a Solução .NET](https://github.com/marcelohnq/prova-bonifiq/issues/5)
- [Adicionar Actions](https://github.com/marcelohnq/prova-bonifiq/issues/11)

