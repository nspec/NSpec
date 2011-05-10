using System;
using System.Linq;
using System.Reflection;
using NSpec.Domain.Extensions;
using System.Text.RegularExpressions;

namespace NSpec.Domain
{
    public class ClassContext : Context
    {
        Conventions conventions;

        public ClassContext(Type type, Conventions conventions)
            : base(type.Name, 0)
        {
            Type = type;

            this.conventions = conventions;
        }

        public void Build()
        {
            BuildMethodLevelBefore();

            BuildMethodLevelAct();
        }

        private void BuildMethodLevelBefore()
        {
            var before = conventions.GetMethodLevelBefore(Type);

            if (before != null) BeforeInstance = i => before.Invoke(i, null);
        }

        private void BuildMethodLevelAct()
        {
            var act = conventions.GetMethodLevelAct(Type);

            if (act != null) ActInstance = i => act.Invoke(i, null);               
        }
    }
}