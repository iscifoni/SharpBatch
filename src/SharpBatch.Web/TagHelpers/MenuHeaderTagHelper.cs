using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SharpBatch.Web.TagHelpers
{
    [HtmlTargetElement("div", Attributes = LabelAttributeName)]
    public class MenuHeaderTagHelper : TagHelper
    {
        private const string LabelAttributeName = "sb-menu-header-label";

        [HtmlAttributeName(LabelAttributeName)]
        public string Label { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            output.Content.GetContent();
            //< li class="header">LABELS</li>
            var tagBuilder = new TagBuilder("li");

            tagBuilder.InnerHtml.SetContent(Label);
            tagBuilder.AddCssClass("header");

            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
            output.MergeAttributes(tagBuilder);
            output.TagName = tagBuilder.TagName;
        }
    }
}
