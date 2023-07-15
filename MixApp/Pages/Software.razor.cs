using Microsoft.AspNetCore.Components;

namespace MixApp.Pages
{
    public partial class SoftwareBase : ComponentBase
    {
        public int currentCount = 0;

        public void IncrementCount()
        {
            currentCount++;
        }
    }
}