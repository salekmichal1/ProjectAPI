using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPIOrder.Data;
using ProjectAPIOrder.Model;
using ProjectAPIOrder.Model.DTO;
using ProjectAPIOrder.Repository;
using System.Net;

namespace ProjectAPIOrder.Endpoints
{
    [Route("api/order")]
    [ApiController]
    public  class OrderEndpoints : ControllerBase
    {
        private ResponseDTO _response;
        private IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private IProductRepo _productRepo;

        public OrderEndpoints(ApplicationDbContext dbContext, IMapper mapper, IProductRepo productRepo)
        {
            _dbContext = dbContext;
            this._response = new ResponseDTO();
            _mapper = mapper;
            _productRepo = productRepo;
        }

        [HttpGet("GetOrder/{userId}")]
        public async Task<ResponseDTO> GetOrder(string userId)
        {
            try
            {
                OrderDTO order = new() { OrderHeader = _mapper.Map<OrderHeaderDTO>(_dbContext.OrderHeaders.First(u => u.UserId == userId)) };

                order.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDTO>>(_dbContext.OrderDetails.Where(u => u.OrderHeaderId == order.OrderHeader.OrderHeaderId));

                IEnumerable<ProductDTO> productDTOs = await _productRepo.GetProducts();

                foreach(var item in order.OrderDetails)
                {
                    item.Product = productDTOs.FirstOrDefault(u => u.Id == item.ProductId);
                    order.OrderHeader.OrderTotal += (item.Count * item.Product.Price);
                }

                _response.Result = order;
            }
            catch(Exception e)
            {
                _response.Accept = false;
                _response.Message = e.Message.ToString();
            }
            return _response;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("UpsertOrder")]
        public async Task<ResponseDTO> UpsertOrder(OrderDTO orderDTO)
        {
            try
            {
                var orderHeaderFromDb = await _dbContext.OrderHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == orderDTO.OrderHeader.UserId);
                if(orderHeaderFromDb == null)
                {
                    OrderHeader orderHeader = _mapper.Map<OrderHeader>(orderDTO.OrderHeader);
                    _dbContext.OrderHeaders.Add(orderHeader);
                    await _dbContext.SaveChangesAsync();
                    orderDTO.OrderDetails.First().OrderHeaderId = orderHeader.OrderHeaderId;
                    _dbContext.OrderDetails.Add(_mapper.Map<OrderDetails>(orderDTO.OrderDetails.First()));
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    var orderDetailsFromDb = await _dbContext.OrderDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == orderDTO.OrderDetails.First().ProductId && u.OrderHeaderId == orderHeaderFromDb.OrderHeaderId);
                    if(orderDetailsFromDb == null)
                    {
                        orderDTO.OrderDetails.First().OrderHeaderId = orderHeaderFromDb.OrderHeaderId;
                        _dbContext.OrderDetails.Add(_mapper.Map<OrderDetails>(orderDTO.OrderDetails.First()));
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        orderDTO.OrderDetails.First().Count += orderDetailsFromDb.Count;
                        orderDTO.OrderDetails.First().OrderHeaderId = orderDetailsFromDb.OrderHeaderId;
                        orderDTO.OrderDetails.First().OrderDetailsId = orderDetailsFromDb.OrderDetailsId;
                        _dbContext.OrderDetails.Update(_mapper.Map<OrderDetails>(orderDTO.OrderDetails.First()));
                        await _dbContext.SaveChangesAsync();

                    }
                }
                _response.Result = orderDTO;
            }
            catch(Exception e)
            {
                _response.Message = e.Message.ToString();
                _response.Accept = false;
            }
            return _response;
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("DeleteOrder")]
        public async Task<ResponseDTO> DeleteOrder([FromBody]int orderDetailsId)
        {
            try
            {
                OrderDetails orderDetails = _dbContext.OrderDetails.First(u => u.OrderDetailsId == orderDetailsId);

                int countOfItem = _dbContext.OrderDetails.Where(u => u.OrderHeaderId == orderDetails.OrderHeaderId).Count();
                _dbContext.OrderDetails.Remove(orderDetails);

                if (countOfItem == 1)
                {
                    var orderHeaderToRemove = await _dbContext.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == orderDetails.OrderHeaderId);

                    _dbContext.OrderHeaders.Remove(orderHeaderToRemove);
                }
                await _dbContext.SaveChangesAsync();
                
                _response.Result = true;
            }
            catch (Exception e)
            {
                _response.Message = e.Message.ToString();
                _response.Accept = false;
            }
            return _response;
        }
    }
}
