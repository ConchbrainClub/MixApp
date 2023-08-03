using Microsoft.AspNetCore.Components;

namespace MixApp.Pages
{
    public partial class SettingBase : ComponentBase
    {
        public int currentCount = 0;

        public void IncrementCount()
        {
            currentCount++;
        }
    }
}