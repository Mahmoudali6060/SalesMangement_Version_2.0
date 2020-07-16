using System;
using System.Collections.Generic;
using System.Text;
using Database.Entities;
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

        public OrderOperationsDSL(IOrderHeaderOperationsRepo orderHeaderOperationsRepo, IOrderDetailsOperationsRepo orderDetailsOperationsRepo, IPurechasesOperationsRepo purechasesOperationsRepo, ISalesinvoicesOperationsRepo salesinvoicesOperationsRepo, IOrder_PurechaseOperationsRepo order_PurechaseOperationsRepo, ISafeOperationsRepo safeOperationsRepo)
        {
            _orderHeaderOperationsRepo = orderHeaderOperationsRepo;
            _orderDetailsOperationsRepo = orderDetailsOperationsRepo;
            _purechasesOperationsRepo = purechasesOperationsRepo;
            _salesinvoicesOperationsRepo = salesinvoicesOperationsRepo;
            _order_PurechaseOperationsRepo = order_PurechaseOperationsRepo;
            _safeOperationsRepo = safeOperationsRepo;
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
            //[1] Save Order (Header and Details)
            _orderHeaderOperationsRepo.Add(entity.OrderHeader);
            entity.OrderDetails = SetOrderHeaderId(entity.OrderHeader.Id, entity.OrderDetails);
            _orderDetailsOperationsRepo.AddRange(entity.OrderDetails);

            //[2] Save Purecchase pill
            PurechasesHeader purechasesHeader = PreparePurechasesEntity(entity);
            _purechasesOperationsRepo.Add(purechasesHeader);

            //[3] Save farmer safe in Safe Entity as hidden rows
            Safe farmerSafe = PrepareFarmerSafeEntity(purechasesHeader, entity.OrderHeader.Id);
            _safeOperationsRepo.Add(farmerSafe);

            //[4]Save Order_Purechase
            Order_Purechase order_Purechase = PrepareOrder_Purechase(entity.OrderHeader.Id, purechasesHeader.Id);
            _order_PurechaseOperationsRepo.Add(order_Purechase);

            //[5] Save Salesinvoice pill
            AddSalesinvoicesEntity(entity);

            return true;
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
                Notes = $"كشف رقم :{entity.Id}",
                IsHidden = true,
                HeaderId = entity.Id,
                OrderId = orderId
            };
        }
        public bool Update(OrderDTO entity)
        {
            //[1] Update Order
            _orderHeaderOperationsRepo.Update(entity.OrderHeader);
            _orderHeaderOperationsRepo.DeleteRelatedOrderDetials(entity.OrderHeader.Id);
            SetOrderHeaderId(entity.OrderHeader.Id, entity.OrderDetails);
            _orderDetailsOperationsRepo.AddRange(entity.OrderDetails);

            //[2] Update Purechase
            PurechasesHeader purechasesHeader = PreparePurechasesEntity(entity);
            purechasesHeader.Id = _order_PurechaseOperationsRepo.GetByOrderHeaderId(entity.OrderHeader.Id).PurechasesHeaderId;
            _purechasesOperationsRepo.Update(purechasesHeader);

            //[3] Update Safe 
            _safeOperationsRepo.DeleteByOrderId(entity.OrderHeader.Id);

            //_safeOperationsRepo.DeleteByHeaderId(purechasesHeader.Id, AccountTypesEnum.Clients);
            var safe = PrepareFarmerSafeEntity(purechasesHeader, entity.OrderHeader.Id);
            _safeOperationsRepo.Add(safe);

            //[4]Update Order_Purechase
            Order_Purechase order_Purechase = PrepareOrder_Purechase(entity.OrderHeader.Id, purechasesHeader.Id);
            _order_PurechaseOperationsRepo.Update(order_Purechase);

            //[5] Update Salesinvoice
            _salesinvoicesOperationsRepo.DeleteSalesinvoiceDetails(entity.OrderHeader);
            _salesinvoicesOperationsRepo.DeleteSalesinvoiceHeader(entity.OrderHeader);
            AddSalesinvoicesEntity(entity);

            return true;
        }
        public bool Delete(long id)
        {
            OrderHeader orderHeader = GetById(id);
            _purechasesOperationsRepo.DeleteRelatedPurechase(id);
            _orderHeaderOperationsRepo.Delete(id);
            _salesinvoicesOperationsRepo.DeleteSalesinvoiceDetails(orderHeader);
            _salesinvoicesOperationsRepo.DeleteSalesinvoiceHeader(orderHeader);
            _safeOperationsRepo.DeleteByOrderId(id);
            return true;
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
            List<PurechasesDetials> purechasesDetials = new List<PurechasesDetials>();
            List<OrderDetails> orderDetails = new List<OrderDetails>();
            orderDetails = (List<OrderDetails>)orderHeader.OrderDetails;
            decimal total = 0;
            int totalQuantity = 0;

            for (int i = 0; i < orderDetails.Count; i++)
            {
                if (purechasesDetials.Find(x => x.Price == orderDetails[i].Price) == null)
                {
                    purechasesDetials.Add(new PurechasesDetials()
                    {
                        Quantity = orderDetails[i].Quantity,
                        Weight = orderDetails[i].Weight,
                        Price = orderDetails[i].Price,
                    });
                }

                else
                {
                    purechasesDetials.Find(x => x.Price == orderDetails[i].Price).Quantity += orderDetails[i].Quantity;
                    purechasesDetials.Find(x => x.Price == orderDetails[i].Price).Weight += orderDetails[i].Weight;
                }
                decimal decent = (orderDetails[i].Quantity);
                decimal gift = decimal.Parse((AppSettings.GiftRate * orderDetails[i].Quantity).ToString());
                total += orderDetails[i].Weight * orderDetails[i].Price - (gift + decent);//
                totalQuantity += orderDetails[i].Quantity;
            }
            ///Prepare purechase Header
            return new PurechasesHeader()
            {
                PurechasesDate = orderHeader.OrderHeader.OrderDate,
                FarmerId = orderHeader.OrderHeader.FarmerId,
                PurechasesDetialsList = purechasesDetials,
                Created = orderHeader.OrderHeader.Created,
                Total = total - total * AppSettings.CommissionRate,//صافي الفاتورة
                Commission = total * AppSettings.CommissionRate,
                CommissionRate = AppSettings.CommissionRate * 100,
                Gift = decimal.Parse((AppSettings.GiftRate * totalQuantity).ToString()),
                Descent = totalQuantity
            };
        }
        private void AddSalesinvoicesEntity(OrderDTO orderHeader)
        {
            ///Prepare Salesinvoices Details
            long sellerId = 0;
            decimal total = 0;
            foreach (OrderDetails item in orderHeader.OrderDetails)
            {
                total = 0;
                sellerId = item.SellerId;
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
                    SellerId = sellerId,
                    SalesinvoicesDetialsList = salesinvoicesDetials,
                };
                _salesinvoicesOperationsRepo.Add(salesinvoicesHeader, orderHeader.OrderHeader.Id);

            }
        }
        private Order_Purechase PrepareOrder_Purechase(long orderHeaderId, long purechasesHeaderId)
        {
            return new Order_Purechase()
            {
                OrderHeaderId = orderHeaderId,
                PurechasesHeaderId = purechasesHeaderId
            };
        }


        #endregion
    }
}
