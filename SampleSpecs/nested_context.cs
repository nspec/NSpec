using System;
using System.Linq.Expressions;
using NSpec;

namespace SampleSpecs
{
    public class nested_context : spec
    {
        private User user;

        public void a_user()
        {
            before(() => user = new User());

            specify(() => user.Id.should_not_be_null());

            when( "user is admin",() =>
            {
                before(() => user.Admin = true);

                specify(() => user.Admin.should_be_true());

                when( "user is terminated",() =>
                {
                    before(() => user.Terminated = true);

                    specify(() => user.Terminated.should_be_true());
                });
            });

            specify(() => user.Admin.should_be_false());

            //not impl
            xshould( user_should_not_have_default_password => no_op());
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
    
    public class User
    {
        public int Id { get; set; }

        public bool Terminated { get; set; }

        public bool Admin { get; set; }
    }
}