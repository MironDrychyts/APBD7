namespace WebApplication1.Repositories;
public interface IWareHouseRepository
{
    public bool CheckIfProductExists(int idProduct);

    public bool CheckIfWareHouseExists(int IdWarehouse);

    public bool CheckIfOrderExists(int IdOrder);

    public bool IfIdOrderExistsInProduct_Warehouse(int IdOrder);

    public void UpdateFulfilledAt(int IdOrder);

    public double GetProductPrice(int IdProduct);

    public int AddProductWareHouseRecord(int IdOrder, int IdProduct, int IdWareHouse, int Amount, double totalPrice);

    public int GetOrderId(int IdProduct, int Amount, DateTime CreatedAt);
}