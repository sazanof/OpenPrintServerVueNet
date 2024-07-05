using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib;
using System.Net;
using OpenPrintServerVueNet.Server.Models;

namespace OpenPrintServerVueNet.Server.Classes
{
    public class Snmp
    {
        string community = "public";

        int timeout = 1000;

        int retry = 0;

        int maxRepetitions = 10;

        string user = string.Empty;

        string contextName = string.Empty;

        string authentication = string.Empty;

        string authPhrase = string.Empty;

        string privacy = string.Empty;

        string privPhrase = string.Empty;

        WalkMode mode = WalkMode.WithinSubtree;

        VersionCode version = VersionCode.V1;

        Levels level = Levels.Reportable;

        public IPAddress ip;

        public Snmp(IPAddress _ip)
        {
            ip = _ip;
        }

        public Snmp(IPAddress _ip, VersionCode _version)
        {
            ip = _ip;
            version = _version;
        }

        public Snmp(IPAddress _ip, string _community)
        {
            ip = _ip;
            community = _community;
        }

        public Snmp(IPAddress _ip, string _community, VersionCode _version, WalkMode _mode)
        {
            ip = _ip;
            community = _community;
            version = _version;
            mode = _mode;
        }

        public IList<Variable> Walk(string section)
        {
            if (version == VersionCode.V3)
            {
                throw new NotImplementedException("v3 not work, sorry");
            }

            ObjectIdentifier test = new ObjectIdentifier(section);
            IList<Variable> result = new List<Variable>();

            try
            {
                IPEndPoint receiver = new IPEndPoint(ip, 161);

                if (version == VersionCode.V1)
                {
                    Messenger.Walk(
                         version,
                         receiver,
                         new OctetString(community),
                         test,
                         result,
                         timeout,
                         mode
                         );
                }
                else if (version == VersionCode.V2)
                {
                    Messenger.BulkWalk(
                        version,
                        receiver,
                        new OctetString(community),
                        new OctetString(string.IsNullOrWhiteSpace(contextName) ? string.Empty : contextName),
                        test,
                        result,
                        timeout,
                        maxRepetitions,
                        mode,
                        null,
                        null
                        );
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public IList<Variable>? Get(string section)
        {
            if (version == VersionCode.V3)
            {
                throw new NotImplementedException("v3 not work, sorry");
            }

            try
            {

                IList<Variable> result = new List<Variable>();
                result.Add(new Variable(new ObjectIdentifier(section)));
                IPEndPoint receiver = new IPEndPoint(ip, 161);

                return Messenger.Get(
                     version,
                     receiver,
                     new OctetString(community),
                     result,
                     timeout
                     );
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
