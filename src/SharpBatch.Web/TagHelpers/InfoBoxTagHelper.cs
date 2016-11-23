using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SharpBatch.Web.TagHelpers
{
    [HtmlTargetElement("div", Attributes = BgColorAttributeName)]
    [HtmlTargetElement("div", Attributes = IconAttributeName)]
    public class InfoBoxTagHelper : TagHelper
    {
        private const string BgColorAttributeName = "sb-infobox-bgcolor";
        private const string IconAttributeName = "sb-infobox-icon";

        [HtmlAttributeName(BgColorAttributeName)]
        public string BgColor { get; set; }

        [HtmlAttributeName(IconAttributeName)]
        public string Icon { get; set; }

        //0 : bgcolor
        //1 : icon
        //2 : content
        private string boxContent = @"
        <div class=""col-md-3 col-sm-6 col-xs-12"">
          <div class=""info-box"">
            <span class=""info-box-icon {0}""><i class=""fa {1}""></i></span>
            <div class=""info-box-content"">{2}</div>
            <!-- /.info-box-content -->
          </div>
          <!-- /.info-box -->
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
                content.GetContent());

            output.Content.SetHtmlContent(htmlResponse);
        }

    }
}
