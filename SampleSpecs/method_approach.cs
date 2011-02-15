using NSpec.Extensions;
using NSpec.Interpreter.Method;

namespace SampleSpecs
{
    public class method_approach: spec
    {
        private User user;

        public void a_user()
        {
            before(() => user = new User());

            specify(() => user.Id.should_not_be_default());

            when("user is admin", () =>
            {
                before(() => user.Admin = true);

                specify(() => user.Admin.should_be_true());

                when("user is terminated", () =>
                {
                    before(() => user.Terminated = true);

                    specify(() => user.Terminated.should_be_true());
                });
            });

            specify(() => user.Admin.should_be_false());

            //soon.user_should_not_have_default_password();
        }
    }

    //output

    //given a_user
    //  user Id should_not_be_null
    //  user Admin should_be_false
    //  when user is admin
    //      user Admin should_be_true
    //      when user is terminated
    //          user Terminated should_be_true
}