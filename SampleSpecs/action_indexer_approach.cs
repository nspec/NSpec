using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace SampleSpecs
{
    public class action_indexer_approach: spec
    {
        private User user;

        public void a_user()
        {
            before.each = () => user = new User();

            specify(() => user.Id.should_not_be_default());

            when["user is admin"] = () =>
            {
                before.each = () => user.Admin = true;

                specify(() => user.Admin.should_be_true());

                when["user is terminated"] = () =>
                {
                    before.each = () => user.Terminated = true;

                    specify(() => user.Terminated.should_be_true());
                };
            };

            specify(() => user.Admin.should_be_false());

            //soon.user_should_not_have_default_password();
        }
    }
    
    //output from above
    //given a_user
    //    user Id should_not_be_default
    //    user Admin should_be_false
    //    when user is admin
    //        user Admin should_be_true
    //        when user is terminated
    //            user Terminated should_be_true
}