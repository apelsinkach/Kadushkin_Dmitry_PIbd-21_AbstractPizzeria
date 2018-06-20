using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbstractPizzeriaRestApi.Controllers
{
    public class StatementController : ApiController
    {
        private readonly IStatementService _service;

        public StatementController(IStatementService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetStocksLoad()
        {
            var list = _service.GetStocksLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult GetClientOrders(StatementBindingModel model)
        {
            var list = _service.GetClientOrders(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void SaveProductPrice(StatementBindingModel model)
        {
            _service.SaveProductPrice(model);
        }

        [HttpPost]
        public void SaveStocksLoad(StatementBindingModel model)
        {
            _service.SaveStocksLoad(model);
        }

        [HttpPost]
        public void SaveClientOrders(StatementBindingModel model)
        {
            _service.SaveClientOrders(model);
        }
    }
}
