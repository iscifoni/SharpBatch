﻿//Copyright 2016 Scifoni Ivano
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
    [HtmlTargetElement("InfoBox", Attributes = BgColorAttributeName)]
    [HtmlTargetElement("InfoBox", Attributes = IconAttributeName)]
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
