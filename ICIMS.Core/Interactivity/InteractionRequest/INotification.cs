

using System;
using System.Windows;

namespace ICIMS.Core.Interactivity.InteractionRequest
{
    /// <summary>
    /// Represents an interaction request used for notifications.
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Gets or sets the title to use for the notification.
        /// </summary>
        string Title { get; set; }
       
        /// <summary>
        /// Gets or sets the content of the notification.
        /// </summary>
        object Content { get; set; }
        bool? DialogResult { get; set; }
        Action<bool?> Finish { get; set; }
        bool IsModal { get; set; }
        bool Topmost { get; set; }
         
        ResizeMode ResizeMode { get; set; }

        WindowState WindowState { get; set; }
        bool Maximized { get; set; }

        Action TriggerClose { get; set; }
    }
}
