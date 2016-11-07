using FluentAssertions;
using NSpec;
using SampleSpecs.Model;
using System;

namespace SampleSpecs.Demo
{
    class action_indexer_approach : nspec
    {
        User user;

        void a_user()
        {
            before = () => user = new User();

            specify = () => user.Id.Should().NotBe(0, String.Empty);

            context["user is admin"] = () =>
            {
                before = () => user.Admin = true;

                specify = () => user.Admin.Should().BeTrue(String.Empty);

                context["user is terminated"] = () =>
                {
                    before = () => user.Terminated = true;

                    specify = () => user.Terminated.Should().BeTrue(String.Empty);
                };
            };

            specify = () => user.Admin.Should().BeFalse(String.Empty);

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
