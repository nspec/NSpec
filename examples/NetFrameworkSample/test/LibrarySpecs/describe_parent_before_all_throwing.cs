using NSpec;
using Shouldly;
using System;

namespace LibrarySpecs
{
    class describe_parent_before_all_throwing : nspec
    {
        void before_all()
        {
            Console.WriteLine("Parent before_all");

            throw new Exception("Parent before_all");
        }

        /*
        void before_each()
        {
            Console.WriteLine("Parent before_each");

            throw new Exception("Parent before_each");
        }
        */
    }

    class describe_child_before_all : describe_parent_before_all_throwing
    {
        void describe_test()
        {
            /*
            before = () => { throw new Exception("Child before"); };
            */

            it["Should Fail"] = () =>
            {
                Console.WriteLine("Never make it here!");

                true.ShouldBeTrue();
            };
        }
    }
}
