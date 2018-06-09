﻿using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbstractPizzeriaRestApi.Controllers
{
    public class BasicController : ApiController
    {
        private readonly IBasicService _service;

        public BasicController(IBasicService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void CreateOrder(RequestBindingModel model)
        {
            _service.CreateOrder(model);
        }

        [HttpPost]
        public void TakeOrderInWork(RequestBindingModel model)
        {
            _service.TakeOrderInWork(model);
        }

        [HttpPost]
        public void FinishOrder(RequestBindingModel model)
        {
            _service.FinishOrder(model.Id);
        }

        [HttpPost]
        public void PayOrder(RequestBindingModel model)
        {
            _service.PayOrder(model.Id);
        }

        [HttpPost]
        public void PutComponentOnStock(ResourceIngridientBindingModel model)
        {
            _service.PutComponentOnStock(model);
        }
    }
}
