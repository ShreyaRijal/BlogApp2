#pragma checksum "C:\Users\Hosanna\Documents\year3\CSC348\BlogApp2\BlogApp2\Views\Blog\AddDislikes.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "918bb46a6e63a8346a2d5dda1a01af8843b99bae"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Blog_AddDislikes), @"mvc.1.0.view", @"/Views/Blog/AddDislikes.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Blog/AddDislikes.cshtml", typeof(AspNetCore.Views_Blog_AddDislikes))]
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
#line 1 "C:\Users\Hosanna\Documents\year3\CSC348\BlogApp2\BlogApp2\Views\_ViewImports.cshtml"
using BlogApp2;

#line default
#line hidden
#line 2 "C:\Users\Hosanna\Documents\year3\CSC348\BlogApp2\BlogApp2\Views\_ViewImports.cshtml"
using BlogApp2.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"918bb46a6e63a8346a2d5dda1a01af8843b99bae", @"/Views/Blog/AddDislikes.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"493f2f755fc77b109a17f5a4b0453b292259c0d9", @"/Views/_ViewImports.cshtml")]
    public class Views_Blog_AddDislikes : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<BlogApp2.Models.BlogModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 29, true);
            WriteLiteral("<!--Dislike link button.-->\r\n");
            EndContext();
#line 4 "C:\Users\Hosanna\Documents\year3\CSC348\BlogApp2\BlogApp2\Views\Blog\AddDislikes.cshtml"
  
    ViewData["Title"] = "AddDislikes";

#line default
#line hidden
            BeginContext(166, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(169, 77, false);
#line 8 "C:\Users\Hosanna\Documents\year3\CSC348\BlogApp2\BlogApp2\Views\Blog\AddDislikes.cshtml"
Write(Html.ActionLink("Dislike", "AddDislikes", new { BlogID = Model.BlogEntryID }));

#line default
#line hidden
            EndContext();
            BeginContext(246, 14, true);
            WriteLiteral("\r\n    &nbsp;\r\n");
            EndContext();
            BeginContext(261, 32, false);
#line 10 "C:\Users\Hosanna\Documents\year3\CSC348\BlogApp2\BlogApp2\Views\Blog\AddDislikes.cshtml"
Write(Html.Encode(Model.NumOfDislikes));

#line default
#line hidden
            EndContext();
            BeginContext(293, 2, true);
            WriteLiteral("\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<BlogApp2.Models.BlogModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
