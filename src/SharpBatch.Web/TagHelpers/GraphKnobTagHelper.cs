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
    [HtmlTargetElement("GraphKnob", Attributes = ValueName)]
    [HtmlTargetElement("GraphKnob", Attributes = HeightName)]
    [HtmlTargetElement("GraphKnob", Attributes = WidthName)]
    [HtmlTargetElement("GraphKnob", Attributes = TitleName)]
    [HtmlTargetElement("GraphKnob", Attributes = SkinName)]
    [HtmlTargetElement("GraphKnob", Attributes = ThicknessName)]
    [HtmlTargetElement("GraphKnob", Attributes = FgColorName)]
    public class GraphKnobTagHelper:TagHelper
    {
        private const string ValueName = "sb-Knob-Value";
        private const string HeightName = "sb-Knob-Height";
        private const string WidthName = "sb-Knob-Width";
        private const string TitleName = "sb-Knob-Title";
        private const string SkinName = "sb-Knob-Skin";
        private const string ThicknessName = "sb-Knob-Thickness";
        private const string FgColorName = "sb-Knob-fgColor";

        [HtmlAttributeName(ValueName)]
        public string Value { get; set; }

        [HtmlAttributeName(HeightName)]
        public string Height { get; set; }

        [HtmlAttributeName(WidthName)]
        public string Width { get; set; }

        [HtmlAttributeName(TitleName)]
        public string Title { get; set; }

        [HtmlAttributeName(SkinName)]
        public string Skin { get; set; }

        [HtmlAttributeName(ThicknessName)]
        public string Thickness { get; set; }

        [HtmlAttributeName(FgColorName)]
        public string FgColor { get; set; }

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

            var tagBuilder = new TagBuilder("input");
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
            //tagBuilder.InnerHtml.SetHtmlContent(htmlResponse);
            tagBuilder.AddCssClass("knob");


            tagBuilder.Attributes.Add("type", "text");
            if (!string.IsNullOrEmpty(Value))
            {
                tagBuilder.Attributes.Add("value", Value);
            }

            if (!string.IsNullOrEmpty(Skin))
            {
                tagBuilder.Attributes.Add("data-skin", Skin);
            }

            if (!string.IsNullOrEmpty(Thickness))
            {
                tagBuilder.Attributes.Add("data-thickness", Thickness);
            }

            if (!string.IsNullOrEmpty(Width))
            {
                tagBuilder.Attributes.Add("data-width", Width);
            }

            if (!string.IsNullOrEmpty(Height))
            {
                tagBuilder.Attributes.Add("data-height", Height);
            }

            if (!string.IsNullOrEmpty(FgColor))
            {
                tagBuilder.Attributes.Add("data-fgColor", FgColor);
            }

            tagBuilder.Attributes.Add("data-readonly", "true");
            
            output.Content.SetHtmlContent(tagBuilder);

            if (!string.IsNullOrEmpty(Title))
            {
                var titleTagBuilder = new TagBuilder("div");
                titleTagBuilder.Attributes.Add("class", "knob-label");
                titleTagBuilder.InnerHtml.SetContent(Title);
                output.Content.AppendHtml(titleTagBuilder);
            }

            //output.MergeAttributes(tagBuilder);
            if (!output.Attributes.ContainsName("class"))
            {
                output.Attributes.Add("class", "text-center");
            }
            else
            {
                TagHelperAttribute classAttribute;
                var found = output.Attributes.TryGetAttribute("class", out classAttribute);

                TagHelperAttribute newClassAttribute = new TagHelperAttribute("class", classAttribute.Value + " text-center");
                output.Attributes.SetAttribute(newClassAttribute);
            }

            output.TagName = "div";

        }
    }
}
