namespace WebApplication1.Models.DTOs;

public class AddProduct_Warehouse
{
        public int IdWareHouse { get; set; }
        public int IdProduct { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedAt { get; set; }
}