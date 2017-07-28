using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SharpBatch.Web.TagHelpers
{
    [HtmlTargetElement("div", Attributes = LabelAttributeName)]
    [HtmlTargetElement("div", Attributes = UrlAttributeName)]
    public class MenuItemTagHelper:TagHelper
    {
        private const string LabelAttributeName = "sb-menu-item-label";
        private const string UrlAttributeName = "sb-menu-item-url";
        //0 Url
        //1 Label
        private const string htmlContent = "<a href='{0}'><i class='fa fa-circle-o'></i> {1}</a>";
        [HtmlAttributeName(LabelAttributeName)]
        public string Label { get; set; }
        [HtmlAttributeName(UrlAttributeName)]
        public string Url { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

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
            
            var tagBuilder = new TagBuilder("li");

            if (this.ViewContext.HttpContext.Request.Path.ToString().Contains(Url))
            {
                tagBuilder.AddCssClass("active");
            }
            tagBuilder.InnerHtml.AppendHtml(string.Format(htmlContent, Url, Label));
            output.Content.AppendHtml(tagBuilder);
            output.TagName = "li";

        }
    }
}
