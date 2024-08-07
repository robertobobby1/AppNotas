using Plugin.Media;
using Telerik.XamarinForms.RichTextEditor;
using AppNotas.Utils;
using Xamarin.Forms;

namespace AppNotas.Behaviours
{
    public class PickImageBehavior : Behavior<RadRichTextEditor>
    {
        protected override void OnAttachedTo(RadRichTextEditor richTextEditor)
        {
            base.OnAttachedTo(richTextEditor);

            richTextEditor.PickImage += OnPickImage;
        }

        protected override void OnDetachingFrom(RadRichTextEditor richTextEditor)
        {
            base.OnDetachingFrom(richTextEditor);

            richTextEditor.PickImage -= OnPickImage;
        }

        private static async void OnPickImage(object sender, PickImageEventArgs eventArgs)
        {
            var mediaPlugin = CrossMedia.Current;

            if (mediaPlugin.IsPickPhotoSupported)
            {
                if (!await PermissionHelper.RequestPhotosAccess())
                {
                    return;
                }

                if (!await PermissionHelper.RequestStorrageAccess())
                {
                    return;
                }

                var mediaFile = await mediaPlugin.PickPhotoAsync();

                if (mediaFile != null)
                {
                    var imageSource = RichTextImageSource.FromFile(mediaFile.Path);
                    eventArgs.Accept(imageSource);
                    return;
                }

            }

            eventArgs.Cancel();
        }
    }
}
