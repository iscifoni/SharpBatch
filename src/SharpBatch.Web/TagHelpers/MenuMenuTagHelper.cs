using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SharpBatch.Web.TagHelpers
{
    [HtmlTargetElement("div", Attributes = AttributeName)]
    public class MenuMenuTagHelper : TagHelper
    {
        private const string AttributeName = "sb-menu";

        [HtmlAttributeName(AttributeName)]
        public string Label { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var content = await output.GetChildContentAsync();


            var tagBuilder = new TagBuilder("ul");

            tagBuilder.InnerHtml.SetHtmlContent(content);
            tagBuilder.AddCssClass("sidebar-menu");

            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
            output.MergeAttributes(tagBuilder);
            output.TagName = tagBuilder.TagName;
        }
    }
}
