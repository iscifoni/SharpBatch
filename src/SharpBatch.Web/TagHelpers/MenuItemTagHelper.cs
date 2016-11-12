using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SharpBatch.Web.TagHelpers
{
    [HtmlTargetElement("sbmenuitem", Attributes = LabelAttributeName)]
    [HtmlTargetElement("sbmenuitem", Attributes = UrlAttributeName)]
    public class MenuItemTagHelper:TagHelper
    {
        private const string LabelAttributeName = "sb-item-label";
        private const string UrlAttributeName = "sb-item-url";
        //0 Url
        //1 Label
        private const string htmlContent = "<a href='{0}'><i class='fa fa-circle-o'></i> {1}</a>";
        [HtmlAttributeName(LabelAttributeName)]
        public string Label { get; set; }
        [HtmlAttributeName(UrlAttributeName)]
        public string Url { get; set; }

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


            output.Content.AppendFormat(htmlContent, Url, Label);
            output.TagName = "li";

        }
    }
}
