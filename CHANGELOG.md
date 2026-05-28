# Changelog

## Estrutura inicial

- Mantida a organização original do template em camadas.
- Preservados os recursos existentes de autenticação, usuários, saúde da aplicação, logging e infraestrutura.
- Mantida a configuração de Docker Compose com PostgreSQL, MongoDB e Redis.

## Banco de dados e mapeamento

- Adicionado o agregado de vendas ao `DefaultContext`.
- Criadas as entidades `Sale` e `SaleItem`.
- Criado o value object `ExternalIdentity` para representar referências externas de cliente, filial e produto.
- Criados os mapeamentos `SaleConfiguration` e `SaleItemConfiguration` com Fluent API.
- Adicionada migration para criação das tabelas `Sales` e `SaleItems`.
- Adicionado repositório `SaleRepository` e contrato `ISaleRepository`.

## Regras de negócio

- Implementada a regra de desconto por quantidade no domínio.
- Bloqueada a venda de mais de 20 itens idênticos.
- Implementado cancelamento de venda.
- Implementado cancelamento individual de item.
- Recalculado o total da venda sempre que itens são alterados ou cancelados.

## Aplicação

- Adicionados commands e handlers com MediatR para:
  - criação de venda;
  - busca por ID;
  - listagem;
  - atualização;
  - exclusão;
  - cancelamento de venda;
  - cancelamento de item.
- Adicionados validators com FluentValidation para entrada dos casos de uso.
- Adicionados DTOs e mapeamento de retorno para manter o domínio isolado da API.

## Mensageria e logs

- Criados eventos `SaleCreatedEvent`, `SaleModifiedEvent`, `SaleCancelledEvent` e `ItemCancelledEvent`.
- Criado o contrato `ISaleEventPublisher`.
- Implementado `RebusSaleEventPublisher`.
- Configurado Rebus com transporte in-memory.
- Criados handlers Rebus para registrar os eventos no application log.

## API

- Criado `SalesController`.
- Expostos endpoints REST para CRUD de vendas.
- Expostos endpoints para cancelamento de venda e item.
- Adicionada listagem com paginação, filtros e ordenação.
- Tratadas exceções de domínio, validação, conflito e recurso não encontrado.

## Testes

- Adicionados testes unitários para os cenários de desconto.
- Adicionado teste para bloqueio de quantidade acima de 20 itens.
- Adicionados testes para cancelamento de item e venda.
- Adicionado teste de handler usando Bogus para massa de dados.
- Adicionado mock com NSubstitute para repositório e publicação de evento.

## Documentação

- Atualizado o `README.md` com instruções de execução, migrations, testes e endpoints.
- Criada documentação de API em `.doc/sales-api.md`.
- Criado `TESTS.md` com roteiro de validação manual e automatizada.
- Criado `CHECKLIST.md` com rastreabilidade dos requisitos.

## Higiene do repositório

- Adicionado `.DS_Store` ao `.gitignore`.
