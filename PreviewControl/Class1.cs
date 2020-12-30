using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace PreviewControl
{
    public class Class1
    {
        public static async Task<UIElement> GetControl(StorageFile file)
        {
            var text = await FileIO.ReadTextAsync(file);

            return new MarkdownTextBlock() { Text = text };
        }
    }
}
