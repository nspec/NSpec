namespace SampleSpecs.WebSite
{
    class Tea
    {
        private readonly int temperature;

        public Tea(int temperature)
        {
            this.temperature = temperature;
        }

        public string Taste()
        {
            return temperature >= 215 ? "hot" : "cold";
        }
        public double Temperature { get; set; }
    }
}