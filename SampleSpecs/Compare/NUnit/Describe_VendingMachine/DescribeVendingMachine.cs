namespace SampleSpecs.Compare.NUnit.Describe_VendingMachine
{
    public class DescribeVendingMachine
    {
        protected void given_a_new_VendingMachine()
        {
            machine = new VendingMachine();
        }
        protected VendingMachine machine;

        protected void given_doritos_are_registerd_in_A1_for_50_cents()
        {
            machine.RegisterItem("A1", "Doritos", .5m);
        }
    }
}