
using ICIMS.Core.Interactivity.InteractionRequest;
using ICIMS.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ICIMS.Core.Interactivity
{
   public static class PopupWindows
    {
        private static readonly InteractionRequest<INotification> notification = new InteractionRequest<INotification>();
 
		private static readonly InteractionRequest<IConfirmation> confirmation = new InteractionRequest<IConfirmation>();
		static PopupWindows()
		{
			CurrentWidows = new List<MetroWindow>();
            CustomPopupRequest = new InteractionRequest<INotification>();

        }
		public static InteractionRequest<INotification> NotificationRequest
		{
            get
            {
                return notification;
            }
        }
        public static InteractionRequest<INotification> CustomPopupRequest { get; set; } 
        public static InteractionRequest<INotification> NormalNotificationRequest { get; } = new InteractionRequest<INotification>();

        public static InteractionRequest<IConfirmation> ConfirmationRequest
		{
			get
			{
				return confirmation;
			}
		}
		public static List<MetroWindow> CurrentWidows { get; set; }
		/// <summary>
		/// 消息提示
		/// </summary>
		/// <param name="txt">提示内容</param>
		public static void ShowWinMessage(string txt, bool surfaceShow = true)
		{
			TextBlock tb = new TextBlock()
			{
				Text = txt,
				//tb.Foreground = Brushes.Red;
				FontSize = 14,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			var notify = new  Notification()
			{ 
				//Topmost = surfaceShow,
				Title = "消息提示",
				Content =tb,				
			};
		    NormalNotificationRequest.Raise(notify);
		}
		 
	}
}
