using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Order.DTOs;
using Order.IRepositories;
using Order.Models;
using Purechase;
using Safes;
using Salesinvoice;
using Shared.Classes;
using Shared.Enums;

namespace Order.DataServiceLayer
{
    public class OrderOperationsDSL : IOrderOperationsDSL
    {
        private IOrderHeaderOperationsRepo _orderHeaderOperationsRepo;
        private IOrderDetailsOperationsRepo _orderDetailsOperationsRepo;
        private IPurechasesOperationsRepo _purechasesOperationsRepo;
        private ISalesinvoicesOperationsRepo _salesinvoicesOperationsRepo;
        private IOrder_PurechaseOperationsRepo _order_PurechaseOperationsRepo;
        private ISafeOperationsRepo _safeOperationsRepo;
        private IServiceProvider _serviceProvider;
        private IConfiguration _configuration;
        public OrderOperationsDSL(IOrderHeaderOperationsRepo orderHeaderOperationsRepo, IOrderDetailsOperationsRepo orderDetailsOperationsRepo, IPurechasesOperationsRepo purechasesOperationsRepo, ISalesinvoicesOperationsRepo salesinvoicesOperationsRepo, IOrder_PurechaseOperationsRepo order_PurechaseOperationsRepo, ISafeOperationsRepo safeOperationsRepo, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _orderHeaderOperationsRepo = orderHeaderOperationsRepo;
            _orderDetailsOperationsRepo = orderDetailsOperationsRepo;
            _purechasesOperationsRepo = purechasesOperationsRepo;
            _salesinvoicesOperationsRepo = salesinvoicesOperationsRepo;
            _order_PurechaseOperationsRepo = order_PurechaseOperationsRepo;
            _safeOperationsRepo = safeOperationsRepo;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public IEnumerable<OrderHeader> GetAll()
        {
            return _orderHeaderOperationsRepo.GetAll();
        }
        public OrderListDTO GetAll(int currentPage, string keyword, bool isToday)
        {
            return _orderHeaderOperationsRepo.GetAll(currentPage, keyword, isToday);
        }
        public OrderHeader GetById(long id)
        {
            return _orderHeaderOperationsRepo.GetById(id);
        }


        public bool Add(OrderDTO entity)
        {
            var options = GetOptions();
            using (var context = new EntitiesDbContext(options))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //[1] Save Order (Header and Details)
                        _orderHeaderOperationsRepo.Add(entity.OrderHeader, context);//Save in OrderHeader Table (Master Table)
                        entity.OrderDetails = SetOrderHeaderId(entity.OrderHeader.Id, entity.OrderDetails);//set OrderHeaderId to OrderDetails table
                        _orderDetailsOperationsRepo.AddRange(entity.OrderDetails, context);//Save in OrderDetails table (details for master table)

                        //[2] Save Purecchase pill
                        PurechasesHeader purechasesHeader = PreparePurechasesEntity(entity);//Prepare PurchaseEntity(Header and details for Cleint)
                        _purechasesOperationsRepo.Add(purechasesHeader, context);//Add Purchase(Header and Details)

                        //[3] Save farmer safe in Safe Entity as hidden rows
                        Safe farmerSafe = PrepareFarmerSafeEntity(purechasesHeader, entity.OrderHeader.Id);//Prepare Safe for Client
                        _safeOperationsRepo.Add(farmerSafe, context);//Add Client Safe row

                        //[4]Save Order_Purechase
                        Order_Purechase order_Purechase = PrepareOrder_Purechase(entity.OrderHeader.Id, purechasesHeader.Id);
                        _order_PurechaseOperationsRepo.Add(order_Purechase, context);

                        //[5] Save Salesinvoice pill
                        var salesinvoicesHeaderList = PrepareSalesinvoicesEntity(entity);//Prepare Salesinvoice(Header and Details)
                        foreach (var salesinvoicesHeader in salesinvoicesHeaderList)
                        {
                            SalesinvoicesHeader updatedSalesinvoicesHeader = _salesinvoicesOperationsRepo.Add(salesinvoicesHeader, entity.OrderHeader.Id, context);//Save in Salesinvoice(Header and Details) and update Total
                            _safeOperationsRepo.DeleteByHeaderId(updatedSalesinvoicesHeader.Id, AccountTypesEnum.Sellers, context);//Delete old record in safe related to this Seller
                            var sellerSafe = PrepareSellerSafeEntity(updatedSalesinvoicesHeader, updatedSalesinvoicesHeader.Total, entity.OrderHeader.Id);
                            _safeOperationsRepo.Add(sellerSafe, context);
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        public bool Update(OrderDTO entity)
        {
            var options = GetOptions();
            using (var context = new EntitiesDbContext(options))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //[1] Update Order (Master and Details)
                        _orderHeaderOperationsRepo.Update(entity.OrderHeader, context);//Update Order table(Master)
                        _orderHeaderOperationsRepo.DeleteRelatedOrderDetials(entity.OrderHeader.Id, context);//Delete order details related to this order header
                        SetOrderHeaderId(entity.OrderHeader.Id, entity.OrderDetails);
                        _orderDetailsOperationsRepo.AddRange(entity.OrderDetails, context);//Adding of new Order Details

                        //[2] Update Purechase (Master and Details)
                        PurechasesHeader purechasesHeader = PreparePurechasesEntity(entity);
                        var orderHeader = _order_PurechaseOperationsRepo.GetByOrderHeaderId(entity.OrderHeader.Id);
                        if (orderHeader != null && orderHeader.PurechasesHeaderId != 0)
                        {
                            purechasesHeader.Id = orderHeader.PurechasesHeaderId;
                            _purechasesOperationsRepo.Update(purechasesHeader, context);
                        }
                        else
                        {
                            _purechasesOperationsRepo.Add(purechasesHeader, context);
                        }

                        //[3] Update Client Safe 
                        _safeOperationsRepo.DeleteByOrderId(entity.OrderHeader.Id, context);
                        var safe = PrepareFarmerSafeEntity(purechasesHeader, entity.OrderHeader.Id);
                        _safeOperationsRepo.Add(safe, context);

                        //[4]Update Order_Purechase
                        Order_Purechase order_Purechase = PrepareOrder_Purechase(entity.OrderHeader.Id, purechasesHeader.Id);
                        _order_PurechaseOperationsRepo.Update(order_Purechase);

                        //[5] Update Salesinvoice
                        List<SalesinvoicesDetials> deletedSalesinvoicesDetials = _salesinvoicesOperationsRepo.DeleteSalesinvoiceDetails(entity.OrderHeader, context);//Delete SalesinvoiceDetails
                        UpdateSalesInvoicTotal(deletedSalesinvoicesDetials,context);
                        _salesinvoicesOperationsRepo.DeleteSalesinvoiceHeader(entity.OrderHeader.Created, context);//Delete Old Salesinvoice related to this Order
                        var salesinvoicesHeaderList = PrepareSalesinvoicesEntity(entity);//Prepare Salesinvoice(Header and Details)
                        foreach (var salesinvoicesHeader in salesinvoicesHeaderList)
                        {
                            SalesinvoicesHeader updatedSalesinvoicesHeader = _salesinvoicesOperationsRepo.Add(salesinvoicesHeader, entity.OrderHeader.Id, context);//Save in Salesinvoice(Header and Details) and update Total
                            _safeOperationsRepo.DeleteByHeaderId(salesinvoicesHeader.Id, AccountTypesEnum.Sellers, context);//Delete old record in safe related to this Seller
                            var sellerSafe = PrepareSellerSafeEntity(updatedSalesinvoicesHeader, updatedSalesinvoicesHeader.Total, entity.OrderHeader.Id);
                            _safeOperationsRepo.Add(sellerSafe, context);
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private void UpdateSalesInvoicTotal(List<SalesinvoicesDetials> deletedSalesinvoicesDetials, EntitiesDbContext context)
        {
            foreach (var item in deletedSalesinvoicesDetials)
            {
                var salesinvoiceHeader = _salesinvoicesOperationsRepo.GetById(item.SalesinvoicesHeaderId, context);
                salesinvoiceHeader.Total = salesinvoiceHeader.Total - ((item.Weight * item.Price) + (AppSettings.MashalRate + AppSettings.ByaaRate) * item.Quantity);
                _salesinvoicesOperationsRepo.Update(salesinvoiceHeader, context);
            }
        }

        public bool Delete(long id)
        {
            var options = GetOptions();
            using (var context = new EntitiesDbContext(options))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        OrderHeader orderHeader = _orderHeaderOperationsRepo.GetById(id, context);
                        _purechasesOperationsRepo.DeleteRelatedPurechase(id, context);//Delete related purechase
                        _safeOperationsRepo.DeleteByOrderId(id, context);//Delete from Safe
                        _orderHeaderOperationsRepo.DeleteRelatedOrderDetials(id, context);//Delelte related order details
                        _salesinvoicesOperationsRepo.DeleteSalesinvoiceDetails(orderHeader, context);
                        _salesinvoicesOperationsRepo.DeleteSalesinvoiceHeader(orderHeader.Created, context);//Delete related Salesinvoice                      _orderHeaderOperationsRepo.Delete(id, context);
                        _orderHeaderOperationsRepo.Delete(id, context);//Delete OrderHeader
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private DbContextOptions<EntitiesDbContext> GetOptions()
        {
            string dbConn = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            var options = new DbContextOptionsBuilder<EntitiesDbContext>()
                .UseSqlServer(new SqlConnection(dbConn))
                .Options;
            return options;
        }

        #region Helper Methods
        private IEnumerable<OrderDetails> SetOrderHeaderId(long orderHeaderId, IEnumerable<OrderDetails> orderDetails)
        {
            foreach (var item in orderDetails)
            {
                item.OrderHeaderId = orderHeaderId;
            }
            return orderDetails;
        }
        private PurechasesHeader PreparePurechasesEntity(OrderDTO orderHeader)
        {
            ///Prepare purechase Details
            List<PurechasesDetials> purechasesDetials = new List<PurechasesDetials>();//To add PurchaseDetail row in this list
            decimal total = 0;//To calculate purchaseHeader Total
            decimal subTotal = 0;//To calculate purchaseHeader sub total 
            int totalQuantity = 0;//Calculate PurchaseHeader Quantity

            foreach (OrderDetails item in orderHeader.OrderDetails)
            {
                if (purechasesDetials.Find(x => x.Price == item.Price) == null)
                {
                    purechasesDetials.Add(new PurechasesDetials()
                    {
                        Quantity = item.Quantity,
                        Weight = item.Weight,
                        Price = item.Price,
                    });
                }

                else
                {
                    purechasesDetials.Find(x => x.Price == item.Price).Quantity += item.Quantity;
                    purechasesDetials.Find(x => x.Price == item.Price).Weight += item.Weight;
                }
                subTotal = item.Price * item.Weight;
                subTotal = Math.Ceiling(subTotal);
                decimal decent = Math.Ceiling((AppSettings.Decentate * item.Quantity));
                decimal gift = Math.Ceiling(decimal.Parse((AppSettings.GiftRate * item.Quantity).ToString()));
                total += subTotal - (gift + decent);//

                totalQuantity += item.Quantity;
            }
            ///Prepare purechase Header
            return new PurechasesHeader()
            {
                PurechasesDate = orderHeader.OrderHeader.OrderDate,
                FarmerId = orderHeader.OrderHeader.FarmerId,
                PurechasesDetialsList = purechasesDetials,
                Created = orderHeader.OrderHeader.Created,
                Total = total - Math.Ceiling(total * AppSettings.CommissionRate),//صافي الفاتورة
                Commission = Math.Ceiling(total * AppSettings.CommissionRate),
                CommissionRate = AppSettings.CommissionRate * 100,
                Gift = decimal.Parse(Math.Ceiling((AppSettings.GiftRate * totalQuantity)).ToString()),
                Descent = totalQuantity,
                Expense = 0M
            };
        }
        private List<SalesinvoicesHeader> PrepareSalesinvoicesEntity(OrderDTO orderHeader)
        {
            List<SalesinvoicesHeader> salesinvoicesHeaderList = new List<SalesinvoicesHeader>();
            ///Prepare Salesinvoices Details
            decimal total = 0;
            foreach (OrderDetails item in orderHeader.OrderDetails)
            {
                total = 0;
                ICollection<SalesinvoicesDetials> salesinvoicesDetials = new List<SalesinvoicesDetials>();
                salesinvoicesDetials.Add(new SalesinvoicesDetials()
                {
                    Quantity = item.Quantity,
                    Weight = item.Weight,
                    Price = item.SellingPrice,
                    Byaa = 3 * item.Quantity,
                    Mashal = 3 * item.Quantity,
                    OrderDate = orderHeader.OrderHeader.Created
                });
                total += (item.Weight * item.SellingPrice) + (AppSettings.MashalRate + AppSettings.ByaaRate) * item.Quantity;
                ///Prepare Salesinvoices Header
                SalesinvoicesHeader salesinvoicesHeader = new SalesinvoicesHeader()
                {
                    SalesinvoicesDate = orderHeader.OrderHeader.OrderDate,
                    SellerId = item.SellerId,
                    SalesinvoicesDetialsList = salesinvoicesDetials,
                    ByaaTotal = AppSettings.ByaaRate * item.Quantity,
                    MashalTotal = AppSettings.MashalRate * item.Quantity,
                };
                salesinvoicesHeaderList.Add(salesinvoicesHeader);
            }
            return salesinvoicesHeaderList;

        }
        private Order_Purechase PrepareOrder_Purechase(long orderHeaderId, long purechasesHeaderId)
        {
            return new Order_Purechase()
            {
                OrderHeaderId = orderHeaderId,
                PurechasesHeaderId = purechasesHeaderId
            };
        }
        private Safe PrepareFarmerSafeEntity(PurechasesHeader entity, long orderId)
        {
            return new Safe()
            {
                Date = entity.PurechasesDate,
                AccountId = entity.FarmerId,
                AccountTypeId = (int)AccountTypesEnum.Clients,
                Incoming = entity.Total,
                Notes = $"الفاتورة رقم :{entity.Id}",
                IsHidden = true,
                HeaderId = entity.Id,
                OrderId = orderId
            };

        }
        private Safe PrepareSellerSafeEntity(SalesinvoicesHeader entity, decimal total, long orderId)
        {
            return new Safe()
            {
                Date = entity.SalesinvoicesDate,
                AccountId = entity.SellerId,
                AccountTypeId = (int)AccountTypesEnum.Sellers,
                Outcoming = total,
                Notes = $" رقم الكشف :{entity.Id}",
                IsHidden = true,
                HeaderId = entity.Id,
                OrderId = orderId
            };
        }
        #endregion
    }
}
