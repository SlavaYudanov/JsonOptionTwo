namespace OptionTwoJson
{
    /// <summary>
    /// ������������� �������.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// �������� �������.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ��� �������.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// ����� �������� �������.
        /// </summary>
        public DateTime Time { get; set; } = DateTime.Now;

    }

}