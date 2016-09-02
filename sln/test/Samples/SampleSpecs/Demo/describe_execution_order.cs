using NSpec;
using System;

namespace SampleSpecs.Demo
{
    // this can be either an abstract class, or just a class
    abstract class parent_class : nspec
    {
        public const int indentSize = 3;
        public static string order = "\n\n";
        public static int indent = 0;

        void Print(string s)
        {
            Console.WriteLine(s);
        }

        protected void Increase(string s)
        {
            Write(s);
            indent += indentSize;
        }

        protected void Decrease(string s)
        {
            indent -= indentSize;
            Write(s);
        }

        protected void Write(string s)
        {
            order += s.PadLeft(s.Length + indent);
        }

        void before_all()
        {
            Increase("parent: before all\n");
        }

        void before_each()
        {
            Write("parent: before each\n");
        }

        void it_works_parent_1()
        {
            Write("parent: it works 1\n");
        }

        void it_works_parent_2()
        {
            Write("parent: it works 2\n");
        }

        void after_each()
        {
            Write("parent: after each\n\n");
        }

        void after_all()
        {
            Decrease("parent: after all\n");
            Print(order);
        }
    }

    class child_class : parent_class
    {
        void before_all()
        {
            Increase("child: before all\n");
        }

        void before_each()
        {
            Write("child: before each\n");
        }

        void it_works_child_3()
        {
            Write("child: it works 3\n");
        }

        void it_works_child_4()
        {
            Write("child: it works 4\n");
        }

        void after_each()
        {
            Write("child: after each\n");
        }

        void after_all()
        {
            Decrease("child: after all\n");
        }

        void method_level_context()
        {
            beforeAll = () => Increase("method: before all\n");

            before = () => Write("method: before each\n");

            it["it works method 5"] = () => Write("method: it works 5\n");

            it["it works method 6"] = () => Write("method: it works 6\n");

            after = () => Write("method: after each\n");

            afterAll = () => Decrease("method: after all\n");

            context["sub context"] = () =>
            {
                beforeAll = () => Increase("sub: before all\n");

                before = () => Write("sub: before each\n");

                it["it works sub 7"] = () => Write("sub: it works 7 \n");

                it["it works sub 8"] = () => Write("sub: it works 8 \n");

                after = () => Write("sub: after each\n");

                afterAll = () => Decrease("sub: after all\n");
            };
        }
    }
}
