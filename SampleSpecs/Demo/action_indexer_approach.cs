using NSpec.Assertions;
using NSpec;

namespace SampleSpecs.Demo
{
    public class action_indexer_approach : spec
    {
        private User user;

        public void a_user()
        {
            before = () => user = new User();

            specify = () => user.Id.should_not_be_default();

            context["user is admin"] = () =>
            {
                before = () => user.Admin = true;

                specify = () => user.Admin.should_be_true();

                context["user is terminated"] = () =>
                {
                    before = () => user.Terminated = true;

                    specify = () => user.Terminated.should_be_true();
                };
            };

            specify = () => user.Admin.should_be_false();

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