#pragma checksum "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\Pages\LeaveMeeting.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ab8118c75f39672744aa5a169b6a171f17b71e0d"
// <auto-generated/>
#pragma warning disable 1591
namespace videocalling_blazor.Client.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\_Imports.razor"
using videocalling_blazor.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\_Imports.razor"
using videocalling_blazor.Client.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\Pages\LeaveMeeting.razor"
using System.Web;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/leavemeeting")]
    public partial class LeaveMeeting : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "container-fluid");
            __builder.AddMarkupContent(2, "\r\n    ");
            __builder.AddMarkupContent(3, @"<div class=""row h-100"">
        <div class=""col-12"">
            <div class=""row banner"">
                <div class=""col-12"">
                    <div class=""row"">
                        <div class=""col-md-6 col-lg-6 col-sm-12"">
                            <img src=""/images/SoowGood-Logo.png"" class=""logo_sooWGood"">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    ");
            __builder.OpenElement(4, "div");
            __builder.AddAttribute(5, "class", "row h-500");
            __builder.AddMarkupContent(6, "\r\n        ");
            __builder.OpenElement(7, "div");
            __builder.AddAttribute(8, "class", "col-12");
            __builder.AddMarkupContent(9, "\r\n            ");
            __builder.OpenElement(10, "div");
            __builder.AddAttribute(11, "class", "row");
            __builder.AddMarkupContent(12, "\r\n                ");
            __builder.OpenElement(13, "div");
            __builder.AddAttribute(14, "class", "col-md-8 col-sm-12 col-xs-12 offset-md-2 leaveCall");
            __builder.AddMarkupContent(15, "\r\n                    ");
            __builder.AddMarkupContent(16, "<h3> Do you want to Rejoin call</h3>\r\n                    ");
            __builder.OpenElement(17, "button");
            __builder.AddAttribute(18, "class", "btn btn-lg twilio-btn-blue");
            __builder.AddAttribute(19, "style", "margin-right:20px");
            __builder.AddAttribute(20, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 24 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\Pages\LeaveMeeting.razor"
                                                                                                     async _ => await OnRejoinMeeting()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(21, "\r\n                        <i class=\"fas fa-check\" aria-label=\"Join room\"></i> Yes\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(22, "\r\n\r\n                    ");
            __builder.OpenElement(23, "button");
            __builder.AddAttribute(24, "class", "btn btn-lg twilio-btn-red");
            __builder.AddAttribute(25, "style", "margin-right:20px");
            __builder.AddAttribute(26, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 28 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\Pages\LeaveMeeting.razor"
                                                                                                    async _ => await OnLeaveMeeting()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(27, "\r\n                        <i class=\"fas fa-times\" aria-label=\"Join room\"></i> No\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(28, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(29, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(30, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(31, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(32, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 36 "E:\OfficeData\angular_workspace\soowgood\videocalling_blazor\Client\Pages\LeaveMeeting.razor"
       
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    string? _bookingid;
    string? _usertype;

    protected override async Task OnInitializedAsync()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        //if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("_room", out var _rName))
        //{
        //    _testroomName = _rName;
        //}

        var data = HttpUtility.ParseQueryString(new Uri(NavigationManager.Uri).Query);
        if (data["_data"] == null)
        {
            NavigationManager.NavigateTo("errorpage?code=404");
        }
        else
        {

            _bookingid = data["_data"];
            _usertype = data["_usertype"];

        }

    }

    async ValueTask OnRejoinMeeting()
    {
        NavigationManager.NavigateTo("/?_data=" + _bookingid + "&_usertype=" + _usertype, forceLoad: true);
    }

    async ValueTask OnLeaveMeeting()
    {
        if (_usertype != "Provider")
            NavigationManager.NavigateTo("https://soowgood.net/rateyourprovider/" + _bookingid, forceLoad: true);
        else
            NavigationManager.NavigateTo("https://soowgood.net/online-appointment/" + _bookingid, forceLoad: true);

    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
