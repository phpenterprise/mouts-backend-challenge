# Checklist da Entrega

Este checklist consolida os principais requisitos do desafio e onde eles foram atendidos no projeto.

| Requisito | Status | Evidencia | Observacao |
| --- | --- | --- | --- |
| API de vendas com CRUD completo | Cumprido | `SalesController` | Criação, leitura, listagem, atualização e exclusão. |
| Numero da venda | Cumprido | `Sale.SaleNumber` | Campo obrigatório e único no banco. |
| Data da venda | Cumprido | `Sale.SaleDate` | Campo obrigatório. |
| Cliente | Cumprido | `ExternalIdentity Customer` | ID externo e descrição desnormalizada. |
| Filial | Cumprido | `ExternalIdentity Branch` | ID externo e descrição desnormalizada. |
| Produtos da venda | Cumprido | `SaleItem` | Cada item contém produto, quantidade, preço, desconto e total. |
| Produto como identidade externa | Cumprido | `ExternalIdentity Product` | ID externo e descrição desnormalizada. |
| Valor total da venda | Cumprido | `Sale.TotalAmount` | Calculado a partir dos itens ativos. |
| Total por item | Cumprido | `SaleItem.TotalAmount` | Calculado no domínio. |
| Desconto por item | Cumprido | `SaleItem.Discount` | Calculado automaticamente por quantidade. |
| Status cancelado/não cancelado | Cumprido | `IsCancelled` em venda e item | Cancelamento de venda e de item. |
| 0% abaixo de 4 itens | Cumprido | `SaleItem.Recalculate` | Coberto por teste unitário. |
| 10% entre 4 e 9 itens | Cumprido | `SaleItem.Recalculate` | Coberto por teste unitário. |
| 20% entre 10 e 20 itens | Cumprido | `SaleItem.Recalculate` | Coberto por teste unitário. |
| Bloqueio acima de 20 itens | Cumprido | `SaleItem.Recalculate` | Lança exceção de domínio. |
| DDD | Cumprido | `Domain`, `Application`, `ORM`, `WebApi` | Regras no domínio e casos de uso na aplicação. |
| External Identities | Cumprido | `ExternalIdentity` | Evita acoplamento com domínios externos. |
| EF Core | Cumprido | `DefaultContext` | PostgreSQL via Npgsql. |
| Fluent API | Cumprido | `SaleConfiguration`, `SaleItemConfiguration` | Domínio sem Data Annotations. |
| Migrations | Cumprido | `20260528230000_AddSales` | Cria tabelas de vendas e itens. |
| MediatR | Cumprido | Commands/Handlers em `Application/Sales` | Fluxo de dados por casos de uso. |
| FluentValidation | Cumprido | Validators de request e command | Validação antes do processamento. |
| Rebus | Cumprido | `AddRebus` com `UseInMemoryTransport` | Transporte in-memory configurado. |
| Application log | Cumprido | Handlers de eventos Rebus | Logs estruturados para eventos. |
| SaleCreatedEvent | Cumprido | `SaleCreatedEventHandler` | Publicado ao criar venda. |
| SaleModifiedEvent | Cumprido | `SaleModifiedEventHandler` | Publicado ao atualizar venda. |
| SaleCancelledEvent | Cumprido | `SaleCancelledEventHandler` | Publicado ao cancelar venda. |
| ItemCancelledEvent | Cumprido | `ItemCancelledEventHandler` | Publicado ao cancelar item. |
| Paginação | Cumprido | `ListSalesCommand` | `page` e `size`. |
| Filtros | Cumprido | `SaleRepository.ApplyFilters` | `saleNumber`, `customerId`, `branchId`, `isCancelled`. |
| Ordenação | Cumprido | `SaleRepository.ApplySorting` | `saleDate`, `saleNumber`, `totalAmount` asc/desc. |
| Consultas sem tracking | Cumprido | `AsNoTracking` na listagem | Evita tracking em leitura. |
| Swagger com endpoints expostos | Cumprido | `.doc/evidence/swagger-ui.png` | Evidencia visual dos endpoints de Auth, Sales e Users. |
| Resposta padrao na raiz da API | Cumprido | `GET /` | Retorna JSON com status, Swagger e health check. |
| xUnit | Cumprido | `SaleTests`, `CreateSaleHandlerTests` | Testes unitários. |
| Bogus/Faker | Cumprido | `CreateSaleHandlerTests` | Geração de massa de dados. |
| NSubstitute | Cumprido | `CreateSaleHandlerTests` | Mock de repositório e publisher. |
| Testes manuais documentados | Cumprido | `TESTS.md` | Fluxo de validação via API. |
| README com execução | Cumprido | `README.md` | Run, migrations, testes e endpoints. |
| Gitignore de arquivos locais | Cumprido | `.gitignore` | `.DS_Store`

## Pontos para validação local

Antes da entrega final, executar em ambiente com .NET 8 instalado:

```bash
cd template/backend
dotnet restore
dotnet build Ambev.DeveloperEvaluation.sln
dotnet test Ambev.DeveloperEvaluation.sln
```

Também é recomendável validar a migration em um banco limpo:

```bash
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
```
