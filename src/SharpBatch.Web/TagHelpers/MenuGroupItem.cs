using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;


namespace SharpBatch.Web.TagHelpers
{
    [HtmlTargetElement("sbmenugroup", Attributes = NameAttributeName)]
    [HtmlTargetElement("sbmenugroup", Attributes = LabelAttributeName)]
    [HtmlTargetElement("sbmenugroup", Attributes = UrlAttributeName)]
    [HtmlTargetElement("sbmenugroup", Attributes = IconAttributeName)]
    public class MenuGroupItem:TagHelper
    {
        private const string NameAttributeName = "sb-group-name";
        private const string LabelAttributeName = "sb-group-label";
        private const string UrlAttributeName = "sb-group-url";
        private const string IconAttributeName = "sb-group-icon";

        
        //0 url
        //1 Label
        //2 icon
        private string HtmlContentHeader = @"
                        <a href = '{0}' >
                            <i class='fa {2}'></i>
                            <span>{1}</span>
                            <span class='pull-right-container'>
                                <i class='fa fa-angle-left pull-right'></i>
                            </span>
                        </a>
                        <ul class='treeview-menu'>";

        private string HtmlContentFooter = @"    </ul>";


        [HtmlAttributeName(NameAttributeName)]
        public string Name { get; set; }
        [HtmlAttributeName(LabelAttributeName)]
        public string Label { get; set; }
        [HtmlAttributeName(UrlAttributeName)]
        public string Url { get; set; }
        [HtmlAttributeName(IconAttributeName)]
        public string Icon { get; set; }
        protected IHtmlGenerator Generator { get; }

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
            
            output.Content.GetContent();
            output.PreContent.AppendFormat(
                HtmlContentHeader,
                Url ?? "#",
                Label,
                Icon ?? string.Empty);
            output.PostContent.AppendHtml(HtmlContentFooter);
            output.TagName = "li";

            if (!output.Attributes.ContainsName("class"))
            {
                output.Attributes.Add("class", "treeview");
            }
            else
            {
                TagHelperAttribute classAttribute;
                var found = output.Attributes.TryGetAttribute("class", out classAttribute);
                //Debug.Assert(found);

                TagHelperAttribute newClassAttribute = new TagHelperAttribute("class", classAttribute.Value + " threeview");
                output.Attributes.SetAttribute(newClassAttribute);
            }
            //output.Content

        }
    }
}
