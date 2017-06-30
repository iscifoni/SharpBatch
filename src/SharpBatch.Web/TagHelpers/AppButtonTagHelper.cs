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

                tagBuilder.InnerHtml.AppendHtml(badgeTagBuilder.InnerHtml);
            }

            if (!string.IsNullOrEmpty(Img))
            {
                var imgTagBuilder = new TagBuilder("i");
                imgTagBuilder.AddCssClass($"fa {Img}");

                tagBuilder.InnerHtml.AppendHtml(imgTagBuilder.InnerHtml);
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
