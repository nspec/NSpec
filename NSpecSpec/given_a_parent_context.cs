using System;
using System.Linq;
using NSpec.Domain;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpecSpec
{
    public class given_a_parent_context : spec
    {
        private Context classContext;

        public void a_child_with_a_failing_example()
        {
            before.each = () =>
                          {
                              classContext = new Context("class");

                              var methodContext = new Context("method");

                              var givenContext = new Context("given");

                              classContext.AddContext(methodContext);

                              methodContext.AddContext(givenContext);

                              givenContext.AddExample(new Example("failed example") {Exception = new Exception()});
                          };

            specify("should be failure 0 != 1",() =>
                    {
                        var fails = classContext.Failures();
                        fails.Count().should_be(1);
                    });
        }
    }
}