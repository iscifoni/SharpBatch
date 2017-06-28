using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SharpBatch.Web.TagHelpers
{
    [HtmlTargetElement("InfoBoxText")]
    public class InfoBoxTextTagHelper : TagHelper
    {
        private string boxContent = @"<span class=""info-box-text"">{0}</span>";

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

            var htmlResponse = string.Format(boxContent, content.GetContent());

            output.TagName = "div";
            output.Content.SetHtmlContent(htmlResponse);
        }
    }
}
