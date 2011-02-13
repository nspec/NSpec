using System;
using System.Collections.Generic;
using NSpec.Array;
using NSpec.Extensions;

namespace SampleSpecs
{
    public class array_approach : spec
    {
        private Dictionary<string,Action> before;
        private Dictionary<string,Action> given;
        private Dictionary<string,Action> when;
        private User user;

        public void a_user()
        {
            before["each"] = () => user = new User();

            specify(() => user.Id.should_not_be_null());

            given["user is admin"] = () => 
            {
                before["each"] = ()=> user.Admin = true;

                specify(() => user.Admin.should_be_true());
            };

            //given.user_is_admin( () =>
            when["user is admin"] = () =>
            {
                before["each"] = ()=> user.Admin = true;

                specify(() => user.Admin.should_be_true());

                when["user is terminated"] = () =>
                {
                    before["each"] =() => user.Terminated = true;

                    specify(() => user.Terminated.should_be_true());
                };
            };

            specify(() => user.Admin.should_be_false());

            //not impl
            //xshould( user_should_not_have_default_password => no_op());
        }

    }
}