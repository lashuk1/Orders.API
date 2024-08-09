using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using Orders.API.Controllers;
using Orders.API.Hubs;
using Orders.Application.Commands;
using Orders.Application.Queries;
using Orders.Domain.Entities;
using Shouldly;

namespace Orders.API.Test;

public class OrderController_Test
{
    #region Setup

    private readonly IMediator _mediatorMock;
    private readonly IHubContext<OrderHub> _hubContextMock;
    private readonly OrdersController _controller;

    public OrderController_Test()
    {
        _mediatorMock = Substitute.For<IMediator>();
        _hubContextMock = Substitute.For<IHubContext<OrderHub>>();
        _controller = new OrdersController(_mediatorMock, _hubContextMock);
    }

    #endregion
    
    #region Tests

    [Fact]
    public async Task Get_ReturnsOkResult_WithListOfOrders()
    {
        // Arrange
        var orders = new List<Order> { new() { Id = 1 }, new() { Id = 2 } };
        _mediatorMock.Send(Arg.Any<GetAllOrdersQuery>()).Returns(orders);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        var returnValue = okResult.Value.ShouldBeOfType<List<Order>>();
        returnValue.Count.ShouldBe(orders.Count);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtActionResult_WithOrderId()
    {
        // Arrange
        var command = new CreateOrderCommand { Amount = 10, IsBuyOrder = true, Price = 15, UserId = Guid.NewGuid().ToString() };
        _mediatorMock.Send(Arg.Any<CreateOrderCommand>()).Returns(1);

        // Act
        var result = await _controller.Create(command);

        // Assert
        var createdAtActionResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
        var returnValue = createdAtActionResult.Value.ShouldBeOfType<int>();
        returnValue.ShouldBe(1);
    }

    [Fact]
    public async Task UpdateOrder_ReturnsOkResult_WhenOrderIsUpdated()
    {
        // Arrange
        var command = new UpdateOrderCommand { Id = 1 };
        _mediatorMock.Send(Arg.Any<UpdateOrderCommand>()).Returns(1);

        // Act
        var result = await _controller.UpdateOrder(1, command);

        // Assert
        var okResult = result.ShouldBeOfType<OkObjectResult>();
        var returnValue = okResult.Value.ShouldBeOfType<int>();
        returnValue.ShouldBe(1);
    }

    [Fact]
    public async Task DeleteOrder_ReturnsOkResult_WhenOrderIsDeleted()
    {
        // Arrange
        var command = new DeleteOrderCommand { Id = 1 };
        _mediatorMock.Send(Arg.Any<DeleteOrderCommand>()).Returns(1);

        // Act
        var result = await _controller.DeleteOrder(1);

        // Assert
        var okResult = result.ShouldBeOfType<OkObjectResult>();
        var returnValue = okResult.Value.ShouldBeOfType<int>();
        returnValue.ShouldBe(1);
    }

    #endregion
}
