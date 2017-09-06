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
    [HtmlTargetElement("AppButton")]
    [HtmlTargetElement(Attributes = ImgName)]
    [HtmlTargetElement(Attributes = BadgeValueName)]
    [HtmlTargetElement(Attributes = LabelName)]
    [HtmlTargetElement(Attributes = BadgeColorName)]
    public class AppButtonTagHelper:TagHelper
    {
        private const string ImgName = "sb-AppBut-Img";
        private const string BadgeValueName = "sb-AppBut-Badge";
        private const string BadgeColorName = "sb-AppBut-BadgeColor";
        private const string LabelName = "sb-AppBut-Label";

        [HtmlAttributeName(ImgName)]
        public string Img { get; set; }

        [HtmlAttributeName(BadgeValueName)]
        public string BadgeValue { get; set; }

        [HtmlAttributeName(LabelName)]
        public string Label { get; set; }

        [HtmlAttributeName(BadgeColorName)]
        public string BadgeColor { get; set; }

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

            var tagBuilder = new TagBuilder("a");
            tagBuilder.InnerHtml.SetHtmlContent(content);
            tagBuilder.AddCssClass("btn btn-app");

            if (!string.IsNullOrEmpty(BadgeValue))
            {
                var badgeTagBuilder = new TagBuilder("span");
                badgeTagBuilder.AddCssClass($"badge {BadgeColor??string.Empty}");
                badgeTagBuilder.InnerHtml.SetContent(BadgeValue);

                tagBuilder.InnerHtml.AppendHtml(badgeTagBuilder);
            }

            if (!string.IsNullOrEmpty(Img))
            {
                var imgTagBuilder = new TagBuilder("i");
                imgTagBuilder.AddCssClass($"fa {Img}");

                tagBuilder.InnerHtml.AppendHtml(imgTagBuilder);
            }

            if (!string.IsNullOrEmpty(Label))
            {
                tagBuilder.InnerHtml.AppendHtml(Label);
            }

            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
            output.MergeAttributes(tagBuilder);
            output.TagName = tagBuilder.TagName;
        }
    }
}
