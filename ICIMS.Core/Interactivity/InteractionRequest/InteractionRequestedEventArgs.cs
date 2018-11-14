

using System;

namespace ICIMS.Core.Interactivity.InteractionRequest
{
    /// <summary>
    /// Event args for the <see cref="IInteractionRequest.Raised"/> event.
    /// </summary>
    public class InteractionRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new instance of <see cref="InteractionRequestedEventArgs"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        public InteractionRequestedEventArgs(INotification context, Action callback)
        {
            this.Context = context;
            this.Callback = callback;
        }
        public InteractionRequestedEventArgs(INotification context, Action callback,Action close)
        {
            this.Context = context;
            this.Callback = callback;
            this.Closed = close;
        }
        /// <summary>
        /// Gets the context for a requested interaction.
        /// </summary>
        public INotification Context { get; private set; }

        /// <summary>
        /// Gets the callback to execute when an interaction is completed.
        /// </summary>
        public Action Callback { get; private set; }
        public Action Closed { get; internal set; }
    }
}
