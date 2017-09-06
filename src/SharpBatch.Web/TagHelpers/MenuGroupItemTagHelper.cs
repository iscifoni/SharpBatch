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
    [HtmlTargetElement("div", Attributes = NameAttributeName)]
    [HtmlTargetElement("div", Attributes = LabelAttributeName)]
    [HtmlTargetElement("div", Attributes = UrlAttributeName)]
    [HtmlTargetElement("div", Attributes = IconAttributeName)]
    public class MenuGroupItemTagHelper:TagHelper
    {
        private const string NameAttributeName = "sb-menu-group-name";
        private const string LabelAttributeName = "sb-menu-group-label";
        private const string UrlAttributeName = "sb-menu-group-url";
        private const string IconAttributeName = "sb-menu-group-icon";

        
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
