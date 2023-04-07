#pragma checksum "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Create.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f59710e56d31527f31168b0e496b5146176f9cb3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_UserAddresses_Create), @"mvc.1.0.view", @"/Views/UserAddresses/Create.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f59710e56d31527f31168b0e496b5146176f9cb3", @"/Views/UserAddresses/Create.cshtml")]
    #nullable restore
    public class Views_UserAddresses_Create : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IdentityMicroservice.Model.UserAddress>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Create.cshtml"
  
    ViewData["Title"] = "Create";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h1>Create</h1>

<h4>UserAddress</h4>
<hr />
<div class=""row"">
    <div class=""col-md-4"">
        <form asp-action=""Create"">
            <div asp-validation-summary=""ModelOnly"" class=""text-danger""></div>
            <div class=""form-group"">
                <label asp-for=""Id"" class=""control-label""></label>
                <input asp-for=""Id"" class=""form-control"" />
                <span asp-validation-for=""Id"" class=""text-danger""></span>
            </div>
            <div class=""form-group"">
                <label asp-for=""CurrentAddress"" class=""control-label""></label>
                <input asp-for=""CurrentAddress"" class=""form-control"" />
                <span asp-validation-for=""CurrentAddress"" class=""text-danger""></span>
            </div>
            <div class=""form-group"">
                <label asp-for=""OptionalAddress"" class=""control-label""></label>
                <input asp-for=""OptionalAddress"" class=""form-control"" />
                <span asp-validation-for=""OptionalAddress");
            WriteLiteral(@""" class=""text-danger""></span>
            </div>
            <div class=""form-group"">
                <label asp-for=""PreferableAddress"" class=""control-label""></label>
                <input asp-for=""PreferableAddress"" class=""form-control"" />
                <span asp-validation-for=""PreferableAddress"" class=""text-danger""></span>
            </div>
            <div class=""form-group"">
                <label asp-for=""City"" class=""control-label""></label>
                <input asp-for=""City"" class=""form-control"" />
                <span asp-validation-for=""City"" class=""text-danger""></span>
            </div>
            <div class=""form-group"">
                <label asp-for=""State"" class=""control-label""></label>
                <input asp-for=""State"" class=""form-control"" />
                <span asp-validation-for=""State"" class=""text-danger""></span>
            </div>
            <div class=""form-group"">
                <label asp-for=""Country"" class=""control-label""></label>
                ");
            WriteLiteral(@"<input asp-for=""Country"" class=""form-control"" />
                <span asp-validation-for=""Country"" class=""text-danger""></span>
            </div>
            <div class=""form-group"">
                <label asp-for=""PostalCode"" class=""control-label""></label>
                <input asp-for=""PostalCode"" class=""form-control"" />
                <span asp-validation-for=""PostalCode"" class=""text-danger""></span>
            </div>
            <div class=""form-group"">
                <label asp-for=""ActivityTime"" class=""control-label""></label>
                <input asp-for=""ActivityTime"" class=""form-control"" />
                <span asp-validation-for=""ActivityTime"" class=""text-danger""></span>
            </div>
            <div class=""form-group"">
                <input type=""submit"" value=""Create"" class=""btn btn-primary"" />
            </div>
        </form>
    </div>
</div>

<div>
    <a asp-action=""Index"">Back to List</a>
</div>

");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 72 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Create.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
            }
            );
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IdentityMicroservice.Model.UserAddress> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591