using Confluent.Kafka;
using Kafka.Demo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kafka.Demo.Controllers
{
    [Route("api/Customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ProducerConfig _configuration;
        private readonly IConfiguration _config;
        public CustomersController(ProducerConfig configuration, IConfiguration config)
        {
            _configuration = configuration;
            _config = config;
        }
        [HttpGet]
        [Route("SendCustomerDetails")]
        public async Task<ActionResult> Get([FromQuery]Customer customer)
        {
            string serialized = JsonConvert.SerializeObject(customer);
            var topic = _config.GetSection("TopicName").Value;
            using (var producer = new ProducerBuilder<Null, string>(_configuration).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = serialized });
                producer.Flush(TimeSpan.FromSeconds(30));
                return Ok(true);
            }
        }
    }
}
