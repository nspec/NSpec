using NSpec;
using OpenQA.Selenium.Firefox;

namespace SeleniumBug
{
    class describe_google : nspec
    {
        private FirefoxDriver firefox;

        void before_all()
        {
            firefox = new FirefoxDriver();
        }

        void given_i_want_to_search_at_google()
        {
            before = () =>
                 firefox.Navigate().GoToUrl("http://www.google.com.br");

            it["should have Google Search button at the home page"] = () =>
            {
                var elements = firefox.FindElementsByCssSelector("input");
                elements.should_contain(element =>
                    element.GetAttribute("value").Equals("Pesquisa Google"));
            };

            context["when submitting with \"kittens\" in the search box"] = () =>
            {
                it["should have \"q=kittens\" on the query string"] = () =>
                {
                    var element = firefox.FindElementById("lst-ib");
                    element.SendKeys("kittens");
                    element.Submit();
                    firefox.Url.should_contain("q=kittens");
                };
            };
        }
    }
}
