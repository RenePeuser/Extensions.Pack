using System;
using System.Reflection;

namespace Extensions.Pack
{
    /// <summary>Represents the extension methods for the <see cref="EventInfo" />.</summary>
    public static class EventInfoExtensions
    {
        /// <summary>The create delegate.</summary>
        /// <typeparam name="TSender">The generic type of the sender.</typeparam>
        /// <typeparam name="TEventArgs">The generic event args.</typeparam>
        /// <param name="eventInfo">The event info.</param>
        /// <param name="actionHandler">The action handler.</param>
        /// <returns>The <see cref="Delegate" />.</returns>
        public static Delegate CreateDelegate<TSender, TEventArgs>(this EventInfo eventInfo, Action<TSender, TEventArgs> actionHandler)
        {
            Throw.IfNull(() => eventInfo);
            Throw.IfNull(() => actionHandler);

            var createdDelegate = Delegate.CreateDelegate(
                eventInfo.EventHandlerType,
                actionHandler.Target,
                actionHandler.Method);
            return createdDelegate;
        }
    }
}
