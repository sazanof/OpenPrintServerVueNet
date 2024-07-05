namespace OpenPrintServerVueNet.Server.Payload
{
    public class GetResultsPayload
    {
        public int? Id { get; set; } = null;

        public int Page { get; set; }

        public int Limit { get; set; } = 50;

    }
}
