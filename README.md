# MarkdownFilePreviewExtension

This is an example of an app that extends Files to provide markdown file previews in the preview pane.

Support for previews is provided by the service "com.markdownpreview.controlservice".
Files locates the name of this service by reading the service property registered in the app manifest, [here](https://github.com/winston-de/MarkdownFilePreviewExtension/blob/fc3a14793d358012a4f70cf63182012b471297e4/MarkdownFileExtension/Package.appxmanifest#L53).
Files also looks at the [FileExtensions.json](https://github.com/winston-de/MarkdownFilePreviewExtension/blob/master/MarkdownFileExtension/Public/FileExtensions.json) file to get a list of file extensions that the preview service is registered for.
If the selected file has an extension listed within this file, then Files will call the extension's service.

### How the service works

The service takes the file buffer as a parameter. It then parses that data as a string, and adds it to a string that contains valid xaml.
For example, if the input text was "Hello, world!", the xaml would be
```cs
<controls:MarkdownTextBlock xml:space="preserve" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls">
  <controls:MarkdownTextBlock.Text>
    Hello, world!
  </controls:MarkdownTextBlock.Text>
</controls:MarkdownTextBlock>
```
The xaml string is them sent back to Files, and loaded using the [XamlReader](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.markup.xamlreader.load).
