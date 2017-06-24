using System;

namespace DotDiff
{
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Field)]
    public class AuditAttribute : Attribute
    {

    }
}
