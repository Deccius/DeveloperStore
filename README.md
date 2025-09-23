# Developer Evaluation Project

`READ CAREFULLY`

## Instructions
**The test below will have up to 7 calendar days to be delivered from the date of receipt of this manual.**

- The code must be versioned in a public Github repository and a link must be sent for evaluation once completed
- Upload this template to your repository and start working from it
- Read the instructions carefully and make sure all requirements are being addressed
- The repository must provide instructions on how to configure, execute and test the project
- Documentation and overall organization will also be taken into consideration

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

## Overview

API REST de Vendas desenvolvida com:

- DDD (Domain Driven Design) para a separação das responsabilidades das camadas.
- Identidades Externas para trazer entidades de outros domínios
- CRUD com todas as informações, tendo optado pelo cancelamento das vendas/itens das vendas ao invés de DELETE completo.

## Tech Stack

* .NET 8


## Frameworks

* Entity Framework Core In Memory para fins dos testes, mantendo os dados em memória sem necessitar de um banco de dados.
* Swagger para melhor visibilidade e testes


## Project Structure

* DeveloperStore.Sales.Api

Projeto principal da API RESTful .NET Core contendo o controlador SalesController com os endpoints, assim como as configurações gerais de inicialização do projeto.

* DeveloperStore.Sales.application

DTOs (data object transfers) para a movimentação dos dados.

SaleService realizando as operações da aplicação através de comunicação com Domain, Repositórios e DTO.

* DeveloperStore.Sales.Domain

Entidades de domínio com suas regras de negócio.

* DeveloperStore.Sales.Infrastructure

Persistência de dados, repositórios e DbContext utilizando Entity Framework Core In Memory.

## Instruções de testes

1. Clonar este repositório e abrir no Visual Studio.

2. Caso a restauração dos pacotes NuGet não seja feita de forma automática, clicar com o botão direito no Solution Explorer e "Restore NuGet Packages"

3. Compilar a solução

4. Executar a aplicação 

- Recomenda-se realizar os testes através da interface do Swagger para melhor visibilidade.
- Não é necessário configuração de banco de dados, pois para os fins deste teste estamos utilizando o Entity Framework Core In Memory.
