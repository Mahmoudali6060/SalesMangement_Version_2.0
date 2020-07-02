﻿using System;
using System.Collections.Generic;
using System.Text;
using Database.Entities;
using Order.IRepositories;
using Order.Models;
using Purechase;
using Salesinvoice;

namespace Order.DataServiceLayer
{
    public class OrderOperationsDSL : IOrderOperationsDSL
    {
        private IOrderHeaderOperationsRepo _orderHeaderOperationsRepo;
        private IOrderDetailsOperationsRepo _orderDetailsOperationsRepo;
        private IPurechasesOperationsRepo _purechasesOperationsRepo;
        private ISalesinvoicesOperationsRepo _salesinvoicesOperationsRepo;
        private IOrder_PurechaseOperationsRepo _order_PurechaseOperationsRepo;

        public OrderOperationsDSL(IOrderHeaderOperationsRepo orderHeaderOperationsRepo, IOrderDetailsOperationsRepo orderDetailsOperationsRepo, IPurechasesOperationsRepo purechasesOperationsRepo, ISalesinvoicesOperationsRepo salesinvoicesOperationsRepo, IOrder_PurechaseOperationsRepo order_PurechaseOperationsRepo)
        {
            _orderHeaderOperationsRepo = orderHeaderOperationsRepo;
            _orderDetailsOperationsRepo = orderDetailsOperationsRepo;
            _purechasesOperationsRepo = purechasesOperationsRepo;
            _salesinvoicesOperationsRepo = salesinvoicesOperationsRepo;
            _order_PurechaseOperationsRepo = order_PurechaseOperationsRepo;
        }

        public IEnumerable<OrderHeader> GetAll()
        {
            return _orderHeaderOperationsRepo.GetAll();
        }
        public IEnumerable<OrderHeader> GetAllDaily()
        {
            return _orderHeaderOperationsRepo.GetAllDaily();

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

            //[3]Save Order_Purechase
            Order_Purechase order_Purechase = PrepareOrder_Purechase(entity.OrderHeader.Id, purechasesHeader.Id);
            _order_PurechaseOperationsRepo.Add(order_Purechase);

            //[4] Save Salesinvoice pill
            AddSalesinvoicesEntity(entity);

            return true;
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

            //[3]Update Order_Purechase
            Order_Purechase order_Purechase = PrepareOrder_Purechase(entity.OrderHeader.Id, purechasesHeader.Id);
            _order_PurechaseOperationsRepo.Update(order_Purechase);

            //[4] Update Salesinvoice
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
            //_orderHeaderOperationsRepo.DeleteRelatedOrderDetials(id);
            _salesinvoicesOperationsRepo.DeleteSalesinvoiceDetails(orderHeader);
            //_order_PurechaseOperationsRepo.Delete(id);
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
                decimal gift = decimal.Parse((0.5 * orderDetails[i].Quantity).ToString());

                total += orderDetails[i].Weight * orderDetails[i].Price - (gift + decent);//Geft and Decent
            }
            ///Prepare purechase Header
            return new PurechasesHeader()
            {
                PurechasesDate = orderHeader.OrderHeader.OrderDate,
                FarmerId = orderHeader.OrderHeader.FarmerId,
                PurechasesDetialsList = purechasesDetials,
                Created = orderHeader.OrderHeader.Created,
                Total = total,
                Commission = total * 0.09M
            };
        }

        private void AddSalesinvoicesEntity(OrderDTO orderHeader)
        {
            ///Prepare Salesinvoices Details
            long sellerId = 0;
            foreach (OrderDetails item in orderHeader.OrderDetails)
            {
                sellerId = item.SellerId;
                ICollection<SalesinvoicesDetials> salesinvoicesDetials = new List<SalesinvoicesDetials>();
                salesinvoicesDetials.Add(new SalesinvoicesDetials()
                {
                    Quantity = item.Quantity,
                    Weight = item.Weight,
                    Price = item.SellingPrice,
                    OrderDate = orderHeader.OrderHeader.Created
                });

                ///Prepare Salesinvoices Header
                SalesinvoicesHeader salesinvoicesHeader = new SalesinvoicesHeader()
                {
                    SalesinvoicesDate = orderHeader.OrderHeader.OrderDate,
                    SellerId = sellerId,
                    SalesinvoicesDetialsList = salesinvoicesDetials
                };
                _salesinvoicesOperationsRepo.Add(salesinvoicesHeader);
            }
        }

        private void UpdateSalesinvoicesEntity(OrderDTO orderHeader)
        {
            ///Prepare Salesinvoices Details
            long sellerId = 0;
            foreach (OrderDetails item in orderHeader.OrderDetails)
            {
                sellerId = item.SellerId;
                ICollection<SalesinvoicesDetials> salesinvoicesDetials = new List<SalesinvoicesDetials>();
                salesinvoicesDetials.Add(new SalesinvoicesDetials()
                {
                    Quantity = item.Quantity,
                    Weight = item.Weight,
                    Price = item.Price,
                });

                ///Prepare Salesinvoices Header
                SalesinvoicesHeader salesinvoicesHeader = new SalesinvoicesHeader()
                {
                    SalesinvoicesDate = orderHeader.OrderHeader.OrderDate,
                    SellerId = sellerId,
                    SalesinvoicesDetialsList = salesinvoicesDetials
                };
                _salesinvoicesOperationsRepo.Update(salesinvoicesHeader);
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