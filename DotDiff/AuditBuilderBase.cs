using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DotDiff
{
    public class AuditBuilderBase<T> : IAuditBuilder<T> where T : class
    {
        private T _oldValue;
        private T _newValue;
        protected readonly List<AuditPair> AuditPairs = new List<AuditPair>();

        public virtual IAuditBuilder<T> Audit(T oldValue, T newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            return this;
        }

        public virtual IAuditBuilder<T> Include(Expression<Func<T, object>> exp)
        {
            var memberExpression = exp.Body as MemberExpression;
            var unaryExpression = exp.Body as UnaryExpression;

            var member = memberExpression ?? unaryExpression.Operand as MemberExpression;

            if (member == null)
                throw new ArgumentException("Expression is not a member lambda" + exp.Body, "exp");

            string propertyName = member.Member.Name;
            object oldValue = exp.Compile()(_oldValue);
            object newValue = exp.Compile()(_newValue);
            AuditPairs.Add(new AuditPair
            {
                NewValue = newValue?.ToString(),
                OldValue = oldValue?.ToString(),
                PropertyName = propertyName
            });
            return this;
        }

        public virtual string Serialize()
        {
            throw new NotImplementedException($"{typeof(AuditBuilderBase<>).Name} is not for use, Please use {typeof(XmlAuditBuilder<>).Name} OR {typeof(JsonAuditBuilder<>).Name}");
        }
    }
}