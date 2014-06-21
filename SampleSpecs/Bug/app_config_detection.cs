using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using System.Configuration;
using NSpec.Assertions.nUnit;

namespace SampleSpecs.Bug
{
    class app_config_detection : nspec
    {
        void it_finds_app_config()
        {
            ConfigurationManager.AppSettings["SomeConfigEntry"].should_be("Worky");
        }
    }
}
