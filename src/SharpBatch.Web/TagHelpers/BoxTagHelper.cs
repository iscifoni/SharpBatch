using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SharpBatch.Web.TagHelpers
{
    [HtmlTargetElement("Box")]
    public class BoxTagHelper:TagHelper
    {
        private string boxContent = @"<div class=""box"">{0}</div>";

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

            output.Content.SetHtmlContent(content);
            //output.MergeAttributes(tagBuilder);
            if (!output.Attributes.ContainsName("class"))
            {
                output.Attributes.Add("class", "box");
            }
            else
            {
                var found = output.Attributes.TryGetAttribute("class", out var classAttribute);

                TagHelperAttribute newClassAttribute = new TagHelperAttribute("class", classAttribute.Value + " box");
                output.Attributes.SetAttribute(newClassAttribute);
            }

            output.TagName = "div";
        }
    }
}
