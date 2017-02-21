using System.Threading.Tasks;

namespace SampleSpecsApi.SampleSystem
{
    public class AsyncSystemUnderTest
    {
        public async Task<bool> IsAlwaysTrueAsync()
        {
            return await Task.Run(() => true);
        }
    }
}
