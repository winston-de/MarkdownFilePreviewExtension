using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace FilePreviewService
{
    public sealed class Preview : IBackgroundTask
    {
        private BackgroundTaskDeferral backgroundTaskDeferral;
        private AppServiceConnection appServiceconnection;
        private String[] inventoryItems = new string[] { "Robot vacuum", "Chair" };
        private double[] inventoryPrices = new double[] { 129.99, 88.99 };

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral so that the service isn't terminated.
            this.backgroundTaskDeferral = taskInstance.GetDeferral();

            // Associate a cancellation handler with the background task.
            taskInstance.Canceled += OnTaskCanceled;

            // Retrieve the app service connection and set up a listener for incoming app service requests.
            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            appServiceconnection = details.AppServiceConnection;
            appServiceconnection.RequestReceived += OnRequestReceived;
        }

        private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            // This function is called when the app service receives a request.
            // Get a deferral because we use an awaitable API below (SendResponseAsync()) to respond to the message
            // and we don't want this call to get cancelled while we are waiting.
            AppServiceDeferral messageDeferral = args.GetDeferral();
            var message = args.Request.Message;
            var returnMessage = new ValueSet();

            var bytearray = message["byteArray"] as byte[];
            var buffer = bytearray.AsBuffer();
            var text = "";

            using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
            {
                text = dataReader.ReadString(buffer.Length);
            }

            //returnMessage.Add("preview", new MarkdownTextBlock { Text = text });
            //var control = new MarkdownTextBlock { Text = text };

            //returnMessage.Add("preview", text);
            string xaml = $"<controls:MarkdownTextBlock xml:space=\"preserve\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:controls=\"using:Microsoft.Toolkit.Uwp.UI.Controls\"><controls:MarkdownTextBlock.Text>{ System.Security.SecurityElement.Escape(text) }</controls:MarkdownTextBlock.Text></controls:MarkdownTextBlock>";
            //xaml = xaml.Replace("\\r\\n", "&#x0a;");
            returnMessage.Add("preview", xaml);
            await args.Request.SendResponseAsync(returnMessage);
            messageDeferral.Complete();
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (this.backgroundTaskDeferral != null)
            {
                // Complete the service deferral.
                this.backgroundTaskDeferral.Complete();
            }
        }
    }
}
