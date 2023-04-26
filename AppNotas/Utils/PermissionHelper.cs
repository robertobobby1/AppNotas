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

        //internal static async Task<bool> RequestStorrageReadAccess()
        //{
        //    var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
        //    if (status != PermissionStatus.Granted)
        //    {
        //        var res = await MainThread.InvokeOnMainThreadAsync<PermissionStatus>(async () =>
        //        {
        //            return await Permissions.RequestAsync<Permissions.StorageRead>();
        //        });

        //        return res == PermissionStatus.Granted;
        //    }
        //    return true;
        //}

        //internal static async Task<bool> RequestPhotosAccess()
        //{
        //    var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
        //    if (status != PermissionStatus.Granted)
        //    {
        //        var res = await Permissions.RequestAsync<Permissions.Photos>();
        //        return res == PermissionStatus.Granted;
        //    }
        //    return true; 
        //}

        //internal static async Task<bool> RequestStorrageWriteAccess()
        //{
        //    var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
        //    if (status != PermissionStatus.Granted)
        //    {
        //        var res = await Permissions.RequestAsync<Permissions.StorageWrite>();
        //        return res == PermissionStatus.Granted;
        //    }
        //    return true;
        //}

    }
}

