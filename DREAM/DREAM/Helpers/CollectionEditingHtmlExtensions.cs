using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

/*
 * Copyright (C) 2011 by Ivan Zlatev <ivan@ivanz.com>
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

namespace DREAM.Helpers
{
    public static class CollectionEditingHtmlExtensions
    {
        /// <summary>
        /// Begins a collection item by inserting either a previously used .Index hidden field value for it or a new one.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="collectionName">The name of the collection property from the Model that owns this item.</param>
        /// <returns></returns>
        public static IDisposable BeginCollectionItem<TModel>(this HtmlHelper<TModel> html, string collectionName)
        {
            if (String.IsNullOrEmpty(collectionName))
                throw new ArgumentException("collectionName is null or empty.", "collectionName");

            string collectionIndexFieldName = String.Format("{0}.Index", collectionName);

            if (html.ViewData.TemplateInfo.HtmlFieldPrefix != "")
            {
                collectionIndexFieldName = html.ViewData.TemplateInfo.HtmlFieldPrefix + "." + collectionIndexFieldName;
            }

            string itemIndex = null;
            if (html.ViewData.ContainsKey(JQueryTemplatingEnabledKey))
            {
                itemIndex = "${" + collectionName + "_index}";
            }
            else
            {
                itemIndex = GetCollectionItemIndex(collectionIndexFieldName);
            }

            string collectionItemName = String.Format("{0}[{1}]", collectionName, itemIndex);

            if (html.ViewData.ContainsKey(JQueryTemplatingEnabledKey))
            {
                collectionIndexFieldName = "${prefix}" + collectionIndexFieldName;
                collectionItemName = "${prefix}" + collectionItemName;
            }

            TagBuilder indexField = new TagBuilder("input");
            indexField.MergeAttributes(new Dictionary<string, string>() {
                { "name", collectionIndexFieldName },
                { "value", itemIndex },
                { "type", "hidden" },
                { "autocomplete", "off" }
            });

            html.ViewContext.Writer.WriteLine(indexField.ToString(TagRenderMode.SelfClosing));
            return new CollectionItemNamePrefixScope(html.ViewData.TemplateInfo, collectionItemName);
        }

        private const string JQueryTemplatingEnabledKey = "__BeginCollectionItem_jQuery";

        public static MvcHtmlString CollectionItemJQueryTemplate<TModel, TCollectionItem>(this HtmlHelper<TModel> html,
                                                                                          string partialViewName,
                                                                                          TCollectionItem modelDefaultValues)
        {
            ViewDataDictionary<TCollectionItem> viewData = new ViewDataDictionary<TCollectionItem>(modelDefaultValues);
            foreach (var kv in html.ViewData)
            {
                viewData.Add(kv);
            }
            viewData.Add(JQueryTemplatingEnabledKey, true);
            return html.Partial(partialViewName, modelDefaultValues, viewData);
        }

        /// <summary>
        /// Tries to reuse old .Index values from the HttpRequest in order to keep the ModelState consistent
        /// across requests. If none are left returns a new one.
        /// </summary>
        /// <param name="collectionIndexFieldName"></param>
        /// <returns>a GUID string</returns>
        private static string GetCollectionItemIndex(string collectionIndexFieldName)
        {
            Queue<string> previousIndices = (Queue<string>)HttpContext.Current.Items[collectionIndexFieldName];
            if (previousIndices == null)
            {
                HttpContext.Current.Items[collectionIndexFieldName] = previousIndices = new Queue<string>();

                string previousIndicesValues = HttpContext.Current.Request[collectionIndexFieldName];
                if (!String.IsNullOrWhiteSpace(previousIndicesValues))
                {
                    foreach (string index in previousIndicesValues.Split(','))
                        previousIndices.Enqueue(index);
                }
            }

            return previousIndices.Count > 0 ? previousIndices.Dequeue() : Guid.NewGuid().ToString();
        }

        private class CollectionItemNamePrefixScope : IDisposable
        {
            private readonly TemplateInfo _templateInfo;
            private readonly string _previousPrefix;

            public CollectionItemNamePrefixScope(TemplateInfo templateInfo, string collectionItemName)
            {
                this._templateInfo = templateInfo;

                _previousPrefix = templateInfo.HtmlFieldPrefix;
                if (_previousPrefix != "")
                {
                    templateInfo.HtmlFieldPrefix = _previousPrefix + "." + collectionItemName;
                }
                else
                {
                    templateInfo.HtmlFieldPrefix = collectionItemName;
                }
            }

            public void Dispose()
            {
                _templateInfo.HtmlFieldPrefix = _previousPrefix;
            }
        }
    }


}
