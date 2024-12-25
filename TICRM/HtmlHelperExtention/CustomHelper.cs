using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TICRM.HtmlHelperExtention
{
    public static class CustomHelper
    {
        public static IHtmlString imageas(this HtmlHelper helper, string src, string alt)
        {
            return new MvcHtmlString(string.Format("<img src='{0}' alt='{1}></img>'", src, alt));
        }

        public static MvcHtmlString imagesa(string src, string alt)
        {
            return new MvcHtmlString(string.Format("<img src='{0}' alt='{1}></img>'", src, alt));
        }

        public static MvcHtmlString Image(string source, string altTxt, string width, string height)
        {
            //TagBuilder creates a new tag with the tag name specified
            var ImageTag = new TagBuilder("img");
            //MergeAttribute Adds attribute to the tag
            ImageTag.MergeAttribute("src", source);
            ImageTag.MergeAttribute("alt", altTxt);
            ImageTag.MergeAttribute("width", width);
            ImageTag.MergeAttribute("height", height);
            //Return an HTML encoded string with SelfClosing TagRenderMode
            return MvcHtmlString.Create(ImageTag.ToString(TagRenderMode.SelfClosing));
        }


        public static IEnumerable<T> CollectionNotNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }



    }
}