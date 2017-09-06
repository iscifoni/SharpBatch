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
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SharpBatch.Web.TagHelpers
{
    [HtmlTargetElement("SmallBox", Attributes = BgColorAttributeName)]
    [HtmlTargetElement("SmallBox", Attributes = IconAttributeName)]
    [HtmlTargetElement("SmallBox", Attributes = UrlAttributeName)]
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
