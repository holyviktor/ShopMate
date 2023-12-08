using System.ComponentModel.DataAnnotations;
using ShopMate.Core.Entities;

namespace ShopMate.WebApi.Models;

public class UserOrdersModel
{
    public UserOrdersModel(int id, UserAddressModel address, DateTime date, Status status, double totalPrice, CouponModel coupon, List<string> products)
    {
        Id = id;
        this.address = address;
        Date = date;
        Status = status;
        TotalPrice = totalPrice;
        Coupon = coupon;
        this.products = products;
    }

    public int Id { get; set; }
    public UserAddressModel address { get; set; }
    public DateTime Date { get; set; }
    public Status Status { get; set; }
    public double TotalPrice { get; set; }
    public CouponModel Coupon { get; set; }
    public List<string> products { get; set; }
    
}