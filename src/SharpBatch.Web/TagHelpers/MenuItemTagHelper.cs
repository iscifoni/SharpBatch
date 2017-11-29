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
