using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DotDiff
{
    public abstract class AuditBuilderBase<T> : IAuditBuilder<T> where T : class
    {
        private T _oldValue;
        private T _newValue;
        protected readonly List<AuditPair> AuditPairs = new List<AuditPair>();

        public virtual IAuditBuilder<T> Audit(T oldValue, T newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            TrackAnotatedProperties();
            return this;
        }

        private void TrackAnotatedProperties()
        {
            var props = typeof(T).GetProperties();
            var auditProps = props.Where(pi => pi.GetCustomAttributes(typeof(AuditAttribute), false).Length > 0);
            foreach (var propertyInfo in auditProps)
            {
                AddAuditPair(propertyInfo);
            }
        }

        private void AddAuditPair(PropertyInfo propertyInfo)
        {
            var key = propertyInfo.Name;
            var oldValue = propertyInfo.GetValue(_oldValue, null);
            var newValue = propertyInfo.GetValue(_newValue, null);
            AddAuditPairToList(key, oldValue, newValue);
        }

        public virtual IAuditBuilder<T> Include(AuditPair auditPair)
        {
            AuditPairs.Add(auditPair);
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
            AddAuditPairToList(propertyName, oldValue, newValue);
            return this;
        }

        private void AddAuditPairToList(string key, object oldValue, object newValue)
        {
            AuditPairs.Add(new AuditPair
            {
                NewValue = newValue?.ToString(),
                OldValue = oldValue?.ToString(),
                Key = key
            });
        }

        public virtual string Serialize()
        {
            throw new NotImplementedException($"{typeof(AuditBuilderBase<>).Name} is a base class, Please use {typeof(XmlAuditBuilder<>).Name} OR {typeof(JsonAuditBuilder<>).Name}");
        }
    }
}