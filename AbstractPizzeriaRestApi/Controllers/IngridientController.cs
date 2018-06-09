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
    public class IngridientController : ApiController
    {
        private readonly IIngridientService _service;

        public IngridientController(IIngridientService service)
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(IngridientBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(IngridientBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(IngridientBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
