using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MS.AFORO255.History.DTO;
using MS.AFORO255.History.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS.AFORO255.History.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly IDistributedCache _distributedCache; //PARA PODER TRABAJAR EN CACHE, ESTE ES PROPIAMENTE DE NET CORE Y USAR REDIS DB

        public HistoryController(IHistoryService historyService, IDistributedCache distributedCache)
        {
            _historyService = historyService;
            _distributedCache = distributedCache;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> Get(int accountId)
        {
            string _key = $"key-account-{accountId}";

            var _cache = _distributedCache.GetString(_key);
            List<HistoryResponse> model = null;

            if (_cache == null) //SI NO EXISTE CACHE CON ESA KEY ENTONCES COSULTAMOS EN LA BASE DE DATOS EN DISCO
            {
                var result = await _historyService.GetAll();
                model = result.Where(x => x.AccountId == accountId).ToList();

                //ACA LE ESTAMOS DEFINIENDO EL TIEMPO DE VIDA DEL CACHE
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

                _distributedCache.SetString(_key, Newtonsoft.Json.JsonConvert.SerializeObject(model), options);
                
            }else //PERO SI HAY INFO EN CACHE CONSULTAMOS LA BD EN MEMORIA
            {
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HistoryResponse>>(_cache);
            }

            return Ok(model);

            //ESTO ESTABAMOS DEVOLVIENDO SIN UTILIZAR REDIS DB
            //var response = await _historyService.GetAll();
            //return Ok(response.Where(x => x.AccountId == accountId).ToList());

        }


        //ESTE METODO ERA SOLO PARA PROBAR CREAR DATOS EN LA BASE DE MONGO

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] HistoryTransaction request)
        //{
        //    await _historyService.Add(request);
        //    return Ok();
        //}
    }
}