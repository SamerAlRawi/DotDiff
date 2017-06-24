using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DotDiff
{
    public class JsonAuditBuilder<T> : AuditBuilderBase<T>, IAuditBuilder<T> where T : class
    {
        public override IAuditBuilder<T> Audit(T oldValue, T newValue)
        {
            base.Audit(oldValue, newValue);
            return this;
        }

        public override IAuditBuilder<T> Include(Expression<Func<T, object>> exp)
        {
            base.Include(exp);
            return this;
        }

        public override string Serialize()
        {
            var memoryStream = new MemoryStream();
            var jsonSerializer = new DataContractJsonSerializer(AuditPairs.GetType());
            jsonSerializer.WriteObject(memoryStream, AuditPairs);
            memoryStream.Flush();
            byte[] json = memoryStream.ToArray();
            memoryStream.Close();

            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
    }
}