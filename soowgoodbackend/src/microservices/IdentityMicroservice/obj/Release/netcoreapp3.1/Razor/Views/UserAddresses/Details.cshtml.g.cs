#pragma checksum "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ba57c8dbddf773c7086b02691cea492575939a6d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_UserAddresses_Details), @"mvc.1.0.view", @"/Views/UserAddresses/Details.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ba57c8dbddf773c7086b02691cea492575939a6d", @"/Views/UserAddresses/Details.cshtml")]
    #nullable restore
    public class Views_UserAddresses_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IdentityMicroservice.Model.UserAddress>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
  
    ViewData["Title"] = "Details";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Details</h1>\r\n\r\n<div>\r\n    <h4>UserAddress</h4>\r\n    <hr />\r\n    <dl class=\"row\">\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 14 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.CurrentAddress));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 17 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayFor(model => model.CurrentAddress));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 20 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.OptionalAddress));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 23 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayFor(model => model.OptionalAddress));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 26 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.PreferableAddress));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 29 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayFor(model => model.PreferableAddress));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 32 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.City));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 35 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayFor(model => model.City));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 38 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.State));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 41 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayFor(model => model.State));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 44 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Country));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 47 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayFor(model => model.Country));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 50 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.PostalCode));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 53 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayFor(model => model.PostalCode));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 56 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ActivityTime));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 59 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
       Write(Html.DisplayFor(model => model.ActivityTime));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n    </dl>\r\n</div>\r\n<div>\r\n    <a asp-action=\"Edit\"");
            BeginWriteAttribute("asp-route-id", " asp-route-id=\"", 1959, "\"", 1983, 1);
#nullable restore
#line 64 "E:\OfficeData\angular_workspace\soowgood\soowgoodbackend\src\microservices\IdentityMicroservice\Views\UserAddresses\Details.cshtml"
WriteAttributeValue("", 1974, Model.Id, 1974, 9, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Edit</a> |\r\n    <a asp-action=\"Index\">Back to List</a>\r\n</div>\r\n");
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
