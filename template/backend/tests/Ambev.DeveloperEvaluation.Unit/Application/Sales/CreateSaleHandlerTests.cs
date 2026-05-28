using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleEventPublisher _eventPublisher;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _eventPublisher = Substitute.For<ISaleEventPublisher>();
        _handler = new CreateSaleHandler(_saleRepository, _eventPublisher);
    }

    [Fact(DisplayName = "Create sale should persist sale and publish created event")]
    public async Task Handle_ShouldPersistSaleAndPublishCreatedEvent()
    {
        var command = GenerateCommand();

        _saleRepository
            .ExistsBySaleNumberAsync(command.SaleNumber, null, Arg.Any<CancellationToken>())
            .Returns(false);

        _saleRepository
            .CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Sale>());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.SaleNumber.Should().Be(command.SaleNumber);
        result.TotalAmount.Should().BeGreaterThan(0);

        await _saleRepository
            .Received(1)
            .CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        await _eventPublisher
            .Received(1)
            .PublishSaleCreatedAsync(result.Id, result.SaleNumber, Arg.Any<CancellationToken>());
    }

    private static CreateSaleCommand GenerateCommand()
    {
        var itemFaker = new Faker<SaleInputItem>()
            .CustomInstantiator(faker => new SaleInputItem(
                faker.Random.Guid(),
                faker.Commerce.ProductName(),
                faker.Random.Int(1, 20),
                faker.Random.Decimal(1, 500)));

        return new Faker<CreateSaleCommand>()
            .CustomInstantiator(faker => new CreateSaleCommand(
                faker.Commerce.Ean13(),
                faker.Date.RecentOffset().UtcDateTime,
                faker.Random.Guid(),
                faker.Name.FullName(),
                faker.Random.Guid(),
                faker.Company.CompanyName(),
                itemFaker.Generate(faker.Random.Int(1, 4))))
            .Generate();
    }
}
