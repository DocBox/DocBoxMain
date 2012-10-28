using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

    public static class HmlExtensions
    {
        public static MvcHtmlString ActionButton(this HtmlHelper html, string linkText, string action, string controllerName, string iconClass, object routeValues)
        {
            //<a href="/@lLink.ControllerName/@lLink.ActionName" title="@lLink.LinkText"><i class="@lLink.IconClass"></i><span class="">@lLink.LinkText</span></a>
            var lURL = new UrlHelper(html.ViewContext.RequestContext);

            // build the <span class="">@lLink.LinkText</span> tag
            var lSpanBuilder = new TagBuilder("span");
            lSpanBuilder.MergeAttribute("class", "");
            lSpanBuilder.SetInnerText(linkText);
            string lSpanHtml = lSpanBuilder.ToString(TagRenderMode.Normal);

            // build the <i class="@lLink.IconClass"></i> tag
            var lIconBuilder = new TagBuilder("i");
            lIconBuilder.MergeAttribute("class", iconClass);
            string lIconHtml = lIconBuilder.ToString(TagRenderMode.Normal);

            // build the <a href="@lLink.ControllerName/@lLink.ActionName" title="@lLink.LinkText">...</a> tag
            var lAnchorBuilder = new TagBuilder("a");
            lAnchorBuilder.MergeAttribute("href", lURL.Action(action, controllerName, routeValues));
            lAnchorBuilder.InnerHtml = lIconHtml + lSpanHtml; // include the <i> and <span> tags inside
            string lAnchorHtml = lAnchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(lAnchorHtml);
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper,
                string imageUrl,
                string altText,
                string actionName,
                string controllerName,
                object routeValues,
                object linkHtmlAttributes,
                object imgHtmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(imgHtmlAttributes);

            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imageUrl);
            builder.MergeAttribute("alt", altText);

            foreach (var key in attributes.Keys)
            {
                var value = attributes[key];
                string valueAsString = null;
                if (value != null)
                {
                    valueAsString = value.ToString();
                }
                builder.MergeAttribute(key, valueAsString);
            }

            //var link = helper.ActionLink("[placeholder]", actionName, controllerName, routeValues, linkHtmlAttributes);
            //var text = link.ToHtmlString();
            var text = "";
            text.Replace("[placeholder]", builder.ToString(TagRenderMode.SelfClosing));

            return MvcHtmlString.Create(text);
        }

    }