using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DotDiff
{
    public class JsonAuditBuilder<T> : AuditBuilderBase<T>, IAuditBuilder<T> where T : class
    {
        public override string Serialize()
        {
            using (var memoryStream = new MemoryStream())
            {
                var jsonSerializer = new DataContractJsonSerializer(AuditPairs.GetType());
                jsonSerializer.WriteObject(memoryStream, AuditPairs);
                memoryStream.Flush();
                byte[] json = memoryStream.ToArray();
                memoryStream.Close();
                AuditPairs.Clear();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }
    }
}