namespace OpenPrintServerVueNet.Classes.DTO
{
    public class PrinterPortDTO
    {
        public int Id { get; set; }
        public int? PrinterId { get; set; }
        public string? Name { get; set; }
        public string? HostAddress { get; set; }
        public string? MacAddress { get; set; }
        public string? Description { get; set; }
    }
}
