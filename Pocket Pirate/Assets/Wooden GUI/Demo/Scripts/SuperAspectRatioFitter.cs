using UnityEngine.UI;

namespace WoodenGUI
{
    public class SuperAspectRatioFitter : AspectRatioFitter
    {
        protected override void Start()
        {
            base.Start();
            SetDirty();
        }
    }
}