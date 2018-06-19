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
    public class WorkerController : ApiController
    {
        private readonly IWorkerService _service;

        public WorkerController(IWorkerService service)
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
        public void AddElement(WorkerBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(WorkerBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(WorkerBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
