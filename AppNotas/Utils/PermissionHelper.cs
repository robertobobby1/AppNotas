using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppNotas.Utils
{
    internal static class PermissionHelper
    {
        internal static async Task<bool> RequestStorrageAccess()
        {
            var currentStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
            if (currentStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                var status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                return status == Plugin.Permissions.Abstractions.PermissionStatus.Granted;
            }
            else
            {
                return true;
            }
        }

        internal static async Task<bool> RequestPhotosAccess()
        {
            var currentStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>();
            if (currentStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                var status = await CrossPermissions.Current.RequestPermissionAsync<PhotosPermission>();
                return status == Plugin.Permissions.Abstractions.PermissionStatus.Granted;
            }
            else
            {
                return true;
            }
        }

        internal static async Task<bool> RequestAudioAccess()
        {
            var currentStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<MicrophonePermission>();
            if (currentStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                var status = await CrossPermissions.Current.RequestPermissionAsync<MicrophonePermission>();
                return status == Plugin.Permissions.Abstractions.PermissionStatus.Granted;
            }
            else
            {
                return true;
            }
        }

    }
}

