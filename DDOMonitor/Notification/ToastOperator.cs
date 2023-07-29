using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows.Forms;

namespace DDOMonitor
{
    /// <summary>
    /// Class <c>ToastOperator</c> will be called to show every toast notification.
    /// </summary>
    public sealed class ToastOperator
    {
        /// <summary>
        /// The <c>ServerMonitor</c>'s constructor method.
        /// This was created to set the Toast Notification button event.
        /// </summary>
        public ToastOperator()
        {
            // Every Time a user clicks on the Toast Notification, this event will trigger.
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
       
                // See what action is being requested 
                switch (args["action"])
                {
                    // User clicked on 'Show All Groups'
                    case "showAll":
                        MessageBox.Show(args["content"].ToString());
                        break;

                    default:
                        break;
                }
            };
        }

        /// <summary>
        /// Method <c>PushQuestInfoToastNotification</c> will display a Toast Notification for the available LFMs.
        /// This will show the new LFM in the main Toast content, but will display all available LFMs if the user
        /// clicks on the 'Show All Groups' button.
        /// </summary>
        /// <param name="newGroupsText">The new LFMs in text</param>
        /// <param name="allGroupsText">All available LFMs in text</param>
        public void PushQuestInfoToastNotification(string newGroupsText, string allGroupsText)
        {
            new ToastContentBuilder()
                .AddText("New Available DDO Group!")
                .AddText(newGroupsText)
                .AddButton(new ToastButton()
                    .SetContent("Show All Groups")
                    .AddArgument("action", "showAll")
                    .AddArgument("content", allGroupsText))
                .AddButton(new ToastButtonDismiss())
                .SetToastScenario(ToastScenario.Alarm)
                .Show();
        }

        /// <summary>
        /// Method <c>PushServerOnlineToastNotification</c> will display a server online Toast Notification.
        /// </summary>
        /// <param name="server">The chosen server</param>
        public void PushServerOnlineToastNotification(string server)
        {
            new ToastContentBuilder()
                .AddText("DDO Servers are back online!")
                .AddText("The chosen server " + server + " is back online!")
                .AddButton(new ToastButtonDismiss())
                .SetToastScenario(ToastScenario.Alarm)
                .Show();
        }

        /// <summary>
        /// Method <c>PushServerOfflineToastNotification</c> will display a server offline Toast Notification.
        /// </summary>
        public void PushServerOfflineToastNotification()
        {
            new ToastContentBuilder()
                .AddText("DDO Servers appears to be offline!")
                .AddText("You will be notified when they become online again.")
                .Show();
        }
    }
}
