namespace OpenPrintServerVueNet.Server.Payload
{
    public class JobSyncedPayload
    {
        public bool success {  get; set; }

        public int timestamp { get; set; }

        public Int64[] ids { get; set; }
    }
}
