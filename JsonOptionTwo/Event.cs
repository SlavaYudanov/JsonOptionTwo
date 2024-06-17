namespace OptionTwoJson
{
    /// <summary>
    /// Представление событий.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Название события.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Вес события.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Время отправки события.
        /// </summary>
        public DateTime Time { get; set; } = DateTime.Now;

    }

}