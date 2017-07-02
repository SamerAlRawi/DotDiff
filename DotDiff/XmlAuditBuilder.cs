using System;
using System.IO;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Serialization;

namespace DotDiff
{
    public class XmlAuditBuilder<T> : AuditBuilderBase<T>, IAuditBuilder<T> where T : class
    {
        public override IAuditBuilder<T> Audit(T oldValue, T newValue)
        {
            base.Audit(oldValue, newValue);
            return this;
        }

        public override IAuditBuilder<T> Include(AuditPair auditPair)
        {
            base.Include(auditPair);
            return this;
        }

        public override IAuditBuilder<T> Include(Expression<Func<T, object>> exp)
        {
            base.Include(exp);
            return this;
        }

        public override string Serialize()
        {
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(AuditPairs.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, AuditPairs, emptyNamepsaces);
                AuditPairs.Clear();
                return stream.ToString();
            }
        }
    }
}