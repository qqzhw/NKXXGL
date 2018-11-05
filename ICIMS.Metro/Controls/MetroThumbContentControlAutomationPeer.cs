using System.Windows;
using System.Windows.Automation.Peers;

namespace ICIMS.Metro.Controls
{
    /// <summary>
    /// The MetroThumbContentControlAutomationPeer class exposes the <see cref="T:ICIMS.Metro.Controls.MetroThumbContentControl" /> type to UI Automation.
    /// </summary>
    public class MetroThumbContentControlAutomationPeer : FrameworkElementAutomationPeer
    {
        public MetroThumbContentControlAutomationPeer(FrameworkElement owner)
            : base(owner)
        {
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }

        protected override string GetClassNameCore()
        {
            return "MetroThumbContentControl";
        }
    }
}