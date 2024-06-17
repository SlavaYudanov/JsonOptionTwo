using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;

namespace OptionTwoJson.Controllers
{
    /// <summary>
    /// ���������� ��� ��������� �������.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        /// <summary>
        /// ������.
        /// </summary>
        private readonly ILogger<EventController> _logger;

        /// <summary>
        /// ������ ��� �������� 
        /// </summary>
        private static List<Event> _events = new List<Event>();

        /// <summary>
        /// �������� ����������� ILogger.
        /// </summary>
        /// <param name="logger">��������� ILogger ��� �������� �����������.</param>
        public EventController(ILogger<EventController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// ��������� ������� � ������ � ���������� ����������� ������ �������.
        /// </summary>
        /// <param name="events">������ �������.</param>
        /// <returns>��������� ��������.</returns>
        [HttpPost]
        public IActionResult PostEvent([FromBody] Event events)
        {
            if (events == null || string.IsNullOrEmpty(events.Name))
            {
                return BadRequest("��������� ���������� ������.");
            }

            _events.Add(events);
            return Ok();
        }

        /// <summary>
        /// �������� ������ ������� � ��������� ��������� � ����� value ������� �������.
        /// </summary>
        /// <param name="startTime">��������� �����.</param>
        /// <param name="endTime">�������� �����</param>
        /// <returns>������ �������� � ����� ������ ���������� � ������ � ����� value.</returns>
        [HttpGet]
        public IActionResult GetEvent(DateTime startTime, DateTime endTime)
        {
            if (startTime > DateTime.Now || endTime > DateTime.Now || startTime > endTime)
            {
                return BadRequest("��������� ���������� ������ ����.");
            }

            var eventsDictionary = new Dictionary<Tuple<DateTime, DateTime>, int>();

            for (var minute = startTime; minute <= endTime; minute = minute.AddMinutes(1))
            {
                var eventsInMinute = _events.Where(e => e.Time >= minute && e.Time < minute.AddMinutes(1)).ToList();
                var sumOfValues = eventsInMinute.Sum(e => e.Value);
                eventsDictionary.Add(Tuple.Create(minute, minute.AddMinutes(1)), sumOfValues);
            }

            var eventsList = eventsDictionary.Select(kv => new
            {
                Time = kv.Key.Item1.ToString() + ";" + kv.Key.Item2.ToString(),
                SumOfValues = kv.Value
            }).ToList();

            return Ok(eventsList); // ��� �� ����� ���� �������� ������� , ������ ������� ������� �� ������ ��-�� Tuple. ����� ������ ������������� � ���� ��� ������������ � json ��� ������� � POSTMAN.
        }
    }
}