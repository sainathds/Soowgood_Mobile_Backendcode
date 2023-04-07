using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using videocalling_blazor.Shared;

namespace videocalling_blazor.Client.Interop
{
    public static class VideoJS
    {
        public static ValueTask<Device[]> GetVideoDevicesAsync(
              this IJSRuntime? jsRuntime) =>
              jsRuntime?.InvokeAsync<Device[]>(
                  "videoInterop.getVideoDevices") ?? new ValueTask<Device[]>();

        public static ValueTask StartVideoAsync(
            this IJSRuntime? jSRuntime,
            string deviceId,
            string selector) =>
            jSRuntime?.InvokeVoidAsync(
                "videoInterop.startVideo",
                deviceId, selector) ?? new ValueTask();

        public static ValueTask<bool> CreateOrJoinRoomAsync(
            this IJSRuntime? jsRuntime,
            string roomName,
            string token,
            string serviceforname,
            string username) =>
            jsRuntime?.InvokeAsync<bool>(
                "videoInterop.createOrJoinRoom",
                roomName, token, serviceforname, username) ?? new ValueTask<bool>(false);

        public static ValueTask LeaveRoomAsync(
            this IJSRuntime? jsRuntime) =>
            jsRuntime?.InvokeVoidAsync(
                "videoInterop.leaveRoom") ?? new ValueTask();

        public static ValueTask<bool> JoinRoomSuccessAsync(
           this IJSRuntime? jsRuntime
          ) =>
           jsRuntime?.InvokeAsync<bool>(
               "videoInterop.joinroomsuccess") ?? new ValueTask<bool>(false);

        public static ValueTask<bool> UnMuteandMuteVideoRoomAsync(
           this IJSRuntime? jsRuntime,
           string roomName,
           bool cammerastatus
            ) =>
           jsRuntime?.InvokeAsync<bool>(
               "videoInterop.unmuteandmutevideo",
               roomName, cammerastatus) ?? new ValueTask<bool>(false);

        public static ValueTask<bool> UnMuteandMuteAudioRoomAsync(
           this IJSRuntime? jsRuntime,
           string roomName,
           bool cammerastatus
            ) =>
           jsRuntime?.InvokeAsync<bool>(
               "videoInterop.unmuteandmuteaudio",
               roomName, cammerastatus) ?? new ValueTask<bool>(false);


        //public static ValueTask<bool> AlertyfyConfirmAsync(this IJSRuntime? jsRuntime,string message) =>
        //   jsRuntime?.InvokeVoidAsync("videoInterop.alertyfyconfirm", message,CallBackInteropWrapper.Create<string>(async s => {
        //       this.callBackResult = s;
        //       this.StateHasChanged();
        //       await Task.Completed;
        //   }));




    }
}
