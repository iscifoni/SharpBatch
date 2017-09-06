//Copyright 2016 Scifoni Ivano
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

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
