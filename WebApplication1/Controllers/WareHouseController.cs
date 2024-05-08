using WebApplication1.Models.DTOs;
using WebApplication1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exercise5.Controllers;

[ApiController]
[Route("/api/warehouse")]
public class WareHouseController : ControllerBase
{
    private readonly IWareHouseRepository _iWareHouseRepository;

    public WareHouseController(IWareHouseRepository iWareHouseRepository)
    {
        _iWareHouseRepository = iWareHouseRepository;
    }

    [HttpPost]
    public IActionResult AddProduct_WareHouse([FromBody] AddProduct_Warehouse addProductWarehouse)
    {
        if (addProductWarehouse.Amount <= 0)
        {
            return BadRequest("Amount <= 0");
        }

        if (!_iWareHouseRepository.CheckIfProductExists(addProductWarehouse.IdProduct) &&
            !_iWareHouseRepository.CheckIfWareHouseExists(addProductWarehouse.IdWareHouse))
        {
            return NotFound("Product or Warehouse does not exists");
        }

        int orderId = _iWareHouseRepository.GetOrderId(addProductWarehouse.IdProduct,
            addProductWarehouse.Amount,
            addProductWarehouse.CreatedAt);

        
        if (!_iWareHouseRepository.CheckIfOrderExists(orderId))
        {
            return NotFound("This order does not exists");
        }

        
       
        if (_iWareHouseRepository.IfIdOrderExistsInProduct_Warehouse(orderId))
        {
            return BadRequest("The order has already been fulfilled");
        }
        
        UpdateFulfilledAt(orderId);
        
        double productPrice = _iWareHouseRepository.GetProductPrice(addProductWarehouse.IdProduct);
        double totalPrice = productPrice * addProductWarehouse.Amount;

        _iWareHouseRepository.AddProductWareHouseRecord(orderId, addProductWarehouse.IdProduct, addProductWarehouse.IdWareHouse, addProductWarehouse.Amount, totalPrice);
        
        
        
        return Ok();
    }

    [HttpPut]
    [NonAction]
    private void UpdateFulfilledAt(int orderId)
    {
        _iWareHouseRepository.UpdateFulfilledAt(orderId);
    }
    
}