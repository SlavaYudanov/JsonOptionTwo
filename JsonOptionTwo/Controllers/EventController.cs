using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;

namespace OptionTwoJson.Controllers
{
    /// <summary>
    /// Контроллер для обработки событий.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger<EventController> _logger;

        /// <summary>
        /// Список для хранения 
        /// </summary>
        private static List<Event> _events = new List<Event>();

        /// <summary>
        /// Внедряет зависимости ILogger.
        /// </summary>
        /// <param name="logger">Экземпляр ILogger для текущего контроллера.</param>
        public EventController(ILogger<EventController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Добавляет событие в список и возвращает обновленный список событий.
        /// </summary>
        /// <param name="events">Список события.</param>
        /// <returns>Результат операции.</returns>
        [HttpPost]
        public IActionResult PostEvent([FromBody] Event events)
        {
            if (events == null || string.IsNullOrEmpty(events.Name))
            {
                return BadRequest("Ожидаются корректные данные.");
            }

            _events.Add(events);
            return Ok();
        }

        /// <summary>
        /// Получает список событий в временном интервале и сумму value каждого события.
        /// </summary>
        /// <param name="startTime">Начальное время.</param>
        /// <param name="endTime">Конечное время</param>
        /// <returns>Список объектов с двумя датами интервалом в минуту и сумму value.</returns>
        [HttpGet]
        public IActionResult GetEvent(DateTime startTime, DateTime endTime)
        {
            if (startTime > DateTime.Now || endTime > DateTime.Now || startTime > endTime)
            {
                return BadRequest("Ожидаются корректные данные даты.");
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

            return Ok(eventsList); // Тут на самом деле интресно выходит , просто вернуть словарь не выйдет из-за Tuple. Решил просто преобразовать в лист для сериализации в json для запроса в POSTMAN.
        }
    }
}