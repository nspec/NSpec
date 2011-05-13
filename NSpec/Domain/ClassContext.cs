using System;

namespace NSpec.Domain
{
    public class ClassContext : Context
    {
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

        public ClassContext(Type type, Conventions conventions) : base(type.Name, 0)
        {
            Type = type;

            this.conventions = conventions;
        }

        public override void Run()
        {
            base.Run();

            //haven't figured out how to write a failing test but
            //using regular iteration causes Collection was modified
            //exception when running samples (rake samples)
            for (int i = 0; i < Examples.Count; i++)
                CreateNSpecInstance().Exercise(Examples[i]);
        }

        Conventions conventions;
    }
}