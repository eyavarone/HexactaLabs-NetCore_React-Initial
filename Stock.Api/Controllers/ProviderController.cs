using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Stock.Api.DTOs;
using Stock.AppService.Services;
using Stock.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace Stock.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/provider")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly ProviderService service;
        private readonly IMapper mapper;

        public ProviderController(ProviderService _service, IMapper _mapper)
        {
            service = _service;
            mapper = _mapper;
        }

        /// <summary>
        /// Permite recuperar todas las instancias
        /// </summary>
        /// <returns>Una colección de instancias</returns>
        [HttpGet]
        public ActionResult<IEnumerable<ProviderDTO>> Get()
        {
            try
            {
                var results = service.GetAll().ToList();
                return Ok(mapper.Map<IEnumerable<ProviderDTO>>(results));
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Permite recuperar una instancia mediante un identificador
        /// </summary>
        /// <param name="id">Identificador de la instancia a recuperar</param>
        /// <returns>Una instancia</returns>
        [HttpGet("{id}")]
        public ActionResult<ProviderDTO> Get(string id)
        {
            try
            {
                var lookUpProvider = service.Get(id);
                return this.mapper.Map<ProviderDTO>(lookUpProvider);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Permite crear una nueva instancia
        /// </summary>
        /// <param name="value">Una instancia</param>
        [HttpPost]
        public ActionResult<ProviderDTO> Post([FromBody] ProviderDTO value)
        {
            TryValidateModel(value);

            try
            {
                var newProvider = this.service.Create(this.mapper.Map<Provider>(value));
                return this.mapper.Map<ProviderDTO>(newProvider);
            }
            catch (System.ArgumentException)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Permite actualizar una instancia existente
        /// </summary>
        /// <param name="id">Id de la instancia a actualizar </param>
        /// <param name="value">Una instancia</param>
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] ProviderDTO value)
        {
            try
            {
                var provider = this.service.Get(id);
                TryValidateModel(value);
                this.mapper.Map<ProviderDTO, Provider>(value, provider);
                this.service.Update(provider);
                return Ok();
            }
            catch (System.ArgumentException)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Permite borrar una instancia
        /// </summary>
        /// <param name="id">Identificador de la instancia a borrar</param>
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                var provider = this.service.Get(id);

                this.service.Delete(provider);

                return Ok();
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }
    }
}