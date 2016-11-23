using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SharpBatch.Web.TagHelpers
{
    [HtmlTargetElement("div", Attributes = BgColorAttributeName)]
    [HtmlTargetElement("div", Attributes = IconAttributeName)]
    [HtmlTargetElement("div", Attributes = UrlAttributeName)]
    public class SmallBoxTagHelper:TagHelper
    {
        private const string BgColorAttributeName = "sb-smallbox-bgcolor";
        private const string IconAttributeName = "sb-smallbox-icon";
        private const string UrlAttributeName = "sb-smallbox-url";


        [HtmlAttributeName(BgColorAttributeName)]
        public string BgColor { get; set; }
        [HtmlAttributeName(UrlAttributeName)]
        public string Url { get; set; }
        [HtmlAttributeName(IconAttributeName)]
        public string Icon { get; set; }


        //0 : bgcolor
        //1 : icon
        //2 : url
        //3 : content
        private string boxContent = @"
    <div class=""col-lg-3 col-xs-6"">
        <!-- small box -->
        <div class=""small-box {0}"">
            <div class=""inner"">{3}</div>
            <div class=""icon"">
                <i class=""ion {1}""></i>
            </div>
            <a href = ""{2}"" class=""small-box-footer"">More info<i class=""fa fa-arrow-circle-right""></i></a>
        </div>
    </div>";

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

            var htmlResponse = string.Format(boxContent,
                BgColor,
                Icon,
                Url,
                content.GetContent());

            output.Content.SetHtmlContent(htmlResponse);


        }
    }
}
