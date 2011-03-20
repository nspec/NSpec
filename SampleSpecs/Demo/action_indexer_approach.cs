using NSpec.Assertions;
using NSpec.Extensions;
using NSpec;

namespace SampleSpecs.Demo
{
    public class action_indexer_approach : spec
    {
        private User user;

        public void a_user()
        {
            before = () => user = new User();

            it += () => user.Id.should_not_be_default();

            context["user is admin"] = () =>
            {
                before = () => user.Admin = true;

                it += () => user.Admin.should_be_true();

                context["user is terminated"] = () =>
                {
                    before = () => user.Terminated = true;

                    it += () => user.Terminated.should_be_true();
                };
            };

            it += () => user.Admin.should_be_false();

            it["should work"] = () =>
            {

            };

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