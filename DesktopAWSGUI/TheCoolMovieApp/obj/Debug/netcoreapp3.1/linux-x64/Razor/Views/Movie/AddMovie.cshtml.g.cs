#pragma checksum "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1813e4ed0f0bfc35f8abfbdd924e0e9ac489322d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Movie_AddMovie), @"mvc.1.0.view", @"/Views/Movie/AddMovie.cshtml")]
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
#nullable restore
#line 1 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\_ViewImports.cshtml"
using TheCoolMovieApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\_ViewImports.cshtml"
using TheCoolMovieApp.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1813e4ed0f0bfc35f8abfbdd924e0e9ac489322d", @"/Views/Movie/AddMovie.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"844781e2b15fb8fc1397a49e6f5651b2571514a3", @"/Views/_ViewImports.cshtml")]
    public class Views_Movie_AddMovie : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TheCoolMovieApp.Models.MovieModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
   if (UserModel.LoggedIn == true)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h2>Add Movie:</h2>\r\n");
#nullable restore
#line 6 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
         using (Html.BeginForm("AddNewMovie", "Movie", FormMethod.Post, new { enctype = "multipart/form-data" }))
        {
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.Label("Title:"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.TextBoxFor(model => model.Title));

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.ValidationMessageFor(model => model.Title));

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.Label("Year:"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.TextBoxFor(model => model.Year));

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.Label("Country of Origin:"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.TextBoxFor(model => model.Origin));

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.Label("Length:"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.TextBoxFor(model => model.Length));

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.Label("File Location:"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.TextBoxFor(model => model.File, new { type = "file" }));

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
       Write(Html.ValidationMessageFor(model => model.File));

#line default
#line hidden
#nullable disable
            WriteLiteral("            <button type=\"submit\">Add Movie</button>\r\n");
#nullable restore
#line 22 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 22 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
         
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h3> Please Sign in to upload a movie.</h3>\r\n");
#nullable restore
#line 27 "C:\Users\farza\Desktop\Lab Review\AWSDesktopGUI\DesktopAWSGUI\TheCoolMovieApp\Views\Movie\AddMovie.cshtml"
    }

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TheCoolMovieApp.Models.MovieModel> Html { get; private set; }
    }
}
#pragma warning restore 1591