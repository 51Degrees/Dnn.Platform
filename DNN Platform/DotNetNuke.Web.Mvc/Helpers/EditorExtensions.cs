﻿#region Copyright
// 
// DotNetNuke® - http://www.dnnsoftware.com
// Copyright (c) 2002-2014
// by DNN Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DotNetNuke.Web.Mvc.Helpers
{
    public static class EditorExtensions
    {
        public static MvcHtmlString EditorFor<TModel, TValue>(this DnnHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var htmlHelper = html.HtmlHelper as HtmlHelper<TModel>;
            return htmlHelper.EditorFor(expression);
        }

        public static MvcHtmlString EditorFor<TModel, TValue>(this DnnHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object additionalViewData)
        {
            var htmlHelper = html.HtmlHelper as HtmlHelper<TModel>;
            return htmlHelper.EditorFor(expression, additionalViewData);
        }

        public static MvcHtmlString EditorFor<TModel, TValue>(this DnnHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName)
        {
            var htmlHelper = html.HtmlHelper as HtmlHelper<TModel>;
            return htmlHelper.EditorFor(expression, templateName);
        }

        public static MvcHtmlString EditorFor<TModel, TValue>(this DnnHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName, object additionalViewData)
        {
            var htmlHelper = html.HtmlHelper as HtmlHelper<TModel>;
            return htmlHelper.EditorFor(expression, templateName, additionalViewData);
        }

        public static MvcHtmlString EditorFor<TModel, TValue>(this DnnHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName, string htmlFieldName)
        {
            var htmlHelper = html.HtmlHelper as HtmlHelper<TModel>;
            return htmlHelper.EditorFor(expression, templateName, htmlFieldName);
        }

        public static MvcHtmlString EditorFor<TModel, TValue>(this DnnHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            var htmlHelper = html.HtmlHelper as HtmlHelper<TModel>;
            return htmlHelper.EditorFor(expression, templateName, htmlFieldName, additionalViewData);
        }
    }
}
