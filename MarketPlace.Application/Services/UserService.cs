using AutoMapper;
using MarketPlace.Application.Exceptions;
using MarketPlace.Application.Interfaces.Persistent;
using MarketPlace.Application.Interfaces.Services;
using MarketPlace.Application.Models.Requests.Auth;
using MarketPlace.Application.Models.Results.Buyers;
using MarketPlace.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MarketPlace.Application.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IBuyerRepository _buyerRepository;

    public UserService(IMapper mapper, ILogger<UserService> logger, IBuyerRepository buyerRepository)
    {
        _mapper = mapper;
        _logger = logger;
        _buyerRepository = buyerRepository;
    }

    public async Task<BuyerResult> RegisterBuyerAsync(RegisterBuyerRequest request)
    {
        var buyerExists = await _buyerRepository.ExistsAsync(x => x.User.Email == request.Email);

        if (buyerExists)
            throw new ValidationException("Buyer with such parameters already exists");
        
        var buyerEntity = _mapper.Map<RegisterBuyerRequest, Buyer>(request);
        await _buyerRepository.AddAsync(buyerEntity, request.Password);
        _logger.LogInformation("New Shop {@Entity} was created successfully", buyerEntity);
        var result = _mapper.Map<Buyer, BuyerResult>(buyerEntity); 
        return result;
    }
}