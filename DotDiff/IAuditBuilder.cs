using System;
using System.Linq.Expressions;

namespace DotDiff
{
    public interface IAuditBuilder<T> where T : class
    {
        IAuditBuilder<T> Include(Expression<Func<T, object>> exp);
        IAuditBuilder<T> Audit(T oldValue, T newValue);
        string Serialize();
    }
}