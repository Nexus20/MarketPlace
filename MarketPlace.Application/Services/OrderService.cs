using AutoMapper;
using MarketPlace.Application.Exceptions;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Application.Models.Requests.Orders;
using MarketPlace.Application.Models.Results.Orders;
using MarketPlace.Domain.Entities;
using MarketPlace.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace MarketPlace.Application.Services;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IMapper _mapper;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(ILogger<OrderService> logger, IMapper mapper, IBuyerRepository buyerRepository, IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _buyerRepository = buyerRepository;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderResult> CreateAsync(string buyerId, CreateOrderRequest request)
    {
        var buyerExists = await _buyerRepository.ExistsAsync(x => x.Id == buyerId);

        if (!buyerExists)
            throw new ValidationException($"Buyer with id {buyerId} doesn't exist");

        var productsIds = request.OrderItems.Select(x => x.ProductId).ToList();
        var productsToBuy = await _productRepository.GetAsync(x => productsIds.Contains(x.Id));

        if (productsToBuy.Count != productsIds.Count)
            throw new ValidationException("Some products ids are invalid. Products don't exist");

        var orderToAdd = _mapper.Map<CreateOrderRequest, Order>(request);
        orderToAdd.Status = OrderStatus.New;
        orderToAdd.PaymentMethod = PaymentMethod.OnDelivery;
        orderToAdd.BuyerId = buyerId;
        orderToAdd.Items = request.OrderItems.Select(x => new OrderItem()
        {
            Count = x.Count,
            Order = orderToAdd,
            ProductId = x.ProductId,
        }).ToList();
        
        orderToAdd.Cost = productsToBuy.Sum(x =>
        {
            var count = request.OrderItems.Single(y => x.Id == y.ProductId).Count;
            return x.FinalPrice * count;
        });
        
        productsToBuy.ForEach(x =>
        {
            x.Count -= request.OrderItems.Single(y => x.Id == y.ProductId).Count;
        });
        
        await _orderRepository.AddAsync(orderToAdd, productsToBuy);

        _logger.LogInformation("Order #{Id} Successfully placed", orderToAdd.Id);
        
        var result = _mapper.Map<Order, OrderResult>(orderToAdd);
        
        return result;
    }
}