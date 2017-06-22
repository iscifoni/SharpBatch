using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SharpBatch.Web.TagHelpers
{
    [HtmlTargetElement("BoxTitle")]
    public class BoxTitleTagHelper:TagHelper
    {
        
        private string boxContent = @"<h3 class=""box-title"">{0}</h3>";

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

            output.Content.SetHtmlContent(htmlResponse);
        }
    }
}
