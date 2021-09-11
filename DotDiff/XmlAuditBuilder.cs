using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DotDiff
{
    public class XmlAuditBuilder<T> : AuditBuilderBase<T>, IAuditBuilder<T> where T : class
    {
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