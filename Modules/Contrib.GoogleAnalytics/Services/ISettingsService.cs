using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contrib.GoogleAnalytics.Models;
using Orchard;

namespace Contrib.GoogleAnalytics.Services {
    public interface ISettingsService : IDependency {
        SettingsRecord Get();
        bool Set(bool enable, string script);
    }
}