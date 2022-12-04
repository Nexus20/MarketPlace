using System.Linq.Expressions;
using AutoMapper;
using MarketPlace.Application.Exceptions;
using MarketPlace.Application.Helpers.Expressions;
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

    public async Task<List<OrderResult>> GetWithDetailsAsync(GetOrdersRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var source = await _orderRepository.GetWithDetailsAsync(predicate);
        var result = _mapper.Map<List<Order>, List<OrderResult>>(source);
        return result;
    }
    
    public async Task<List<ShopOrderResult>> GetShopOrdersAsync(GetOrdersRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var source = await _orderRepository.GetWithDetailsAsync(predicate);
        var result = _mapper.Map<List<Order>, List<ShopOrderResult>>(source);
        return result;
    }
    
    public async Task<List<BuyerOrderResult>> GetBuyerOrdersAsync(GetOrdersRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var source = await _orderRepository.GetWithDetailsAsync(predicate);
        var result = _mapper.Map<List<Order>, List<BuyerOrderResult>>(source);
        return result;
    }

    public async Task<OrderResult> GetByIdAsync(string id)
    {
        var source = await _orderRepository.GetByIdWithDetailsAsync(id);

        if (source == null)
            throw new NotFoundException($"Order {id} not found");
        
        var result = _mapper.Map<Order, OrderResult>(source);
        return result;
    }

    public async Task<OrderStatus> ChangeStatusAsync(string id, ChangeOrderStatusRequest request)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(id);
        
        if(orderToUpdate == null)
            throw new NotFoundException($"Order {id} not found");

        if (orderToUpdate.Status is OrderStatus.Canceled or OrderStatus.Received)
            throw new ValidationException($"Can't update order with {orderToUpdate.Status.ToString()} status");

        if (orderToUpdate.Status == OrderStatus.Accepted && request.NewStatus == OrderStatus.New)
            throw new ValidationException("Can't set status of accepted order to new");

        orderToUpdate.Status = request.NewStatus;
        await _orderRepository.UpdateAsync(orderToUpdate);
        return orderToUpdate.Status;
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
    
    private Expression<Func<Order, bool>>? CreateFilterPredicate(GetOrdersRequest request)
    {
        Expression<Func<Order, bool>>? predicate = null;

        if (!string.IsNullOrWhiteSpace(request.BuyerId))
        {
            Expression<Func<Order, bool>> searchStringExpression = x => x.BuyerId == request.BuyerId;
            predicate = ExpressionsHelper.And(predicate, searchStringExpression);
        }

        if (!string.IsNullOrWhiteSpace(request.ShopId))
        {
            Expression<Func<Order, bool>> shopIdExpression = x => x.ShopId == request.ShopId;
            predicate = ExpressionsHelper.And(predicate, shopIdExpression);
        }
        //
        // if (request.Status.HasValue && Enum.IsDefined(request.Status.Value))
        // {
        //     Expression<Func<HelpRequest, bool>> statusPredicate = x => x.Status == request.Status.Value;
        //     predicate = ExpressionsHelper.And(predicate, statusPredicate);
        // }
        //
        // if (request.StartDate.HasValue && request.EndDate.HasValue && request.StartDate < request.EndDate)
        // {
        //     Expression<Func<HelpRequest, bool>> dateExpression = x => x.CreatedDate > request.StartDate.Value
        //                                                               && x.CreatedDate < request.EndDate.Value;
        //     predicate = ExpressionsHelper.And(predicate, dateExpression);
        // }

        return predicate;
    }
}