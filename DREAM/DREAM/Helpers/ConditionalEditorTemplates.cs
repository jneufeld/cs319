using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DREAM.Helpers
{
    public static class ConditionalEditorTemplates
    {
        public static IHtmlString ControlFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            bool readOnlyTemplate,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null)
        {
            if (readOnlyTemplate)
                return html.DisplayFor(expression);
            else
            {
                var span = new TagBuilder("div");
                var textBox = html.TextBoxFor(expression, htmlAttributes);
                span.AddCssClass("span5");
                span.InnerHtml = html.ValidationMessageFor(expression).ToHtmlString();
                string rawHtml = textBox.ToHtmlString() + Environment.NewLine + span.ToString(TagRenderMode.Normal);
                return html.Raw(rawHtml);
            }
        }

        public static IHtmlString ControlAreaFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            bool readOnlyTemplate,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null)
        {
            if (readOnlyTemplate)
                return html.DisplayFor(expression);
            else
            {
                var span = new TagBuilder("div");
                var textBox = html.TextAreaFor(expression, htmlAttributes);
                span.AddCssClass("span5");
                span.InnerHtml = html.ValidationMessageFor(expression).ToHtmlString();
                string rawHtml = textBox.ToHtmlString() + Environment.NewLine + span.ToString(TagRenderMode.Normal);
                return html.Raw(rawHtml);
            }
        }

        public static IHtmlString ControlDropDownFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            bool readOnlyTemplate,
            Expression<Func<TModel, TProperty>> displayExpression,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            object htmlAttributes = null)
        {
            if (readOnlyTemplate)
                return html.DisplayFor(displayExpression);
            else
            {
                return html.DropDownListFor(expression, selectList, htmlAttributes);
            }
        }

        public static IHtmlString ControlGroupFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            bool readOnlyTemplate,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null)
        {
            var controlGroup = new TagBuilder("div");
            controlGroup.AddCssClass("control-group");
            var label = html.LabelFor(expression);
            var controls = new TagBuilder("div");
            controls.AddCssClass("controls");
            controls.InnerHtml = html.ControlFor(readOnlyTemplate, expression, htmlAttributes).ToHtmlString();
            string inner = label.ToHtmlString() + Environment.NewLine + controls.ToString(TagRenderMode.Normal);
            controlGroup.InnerHtml = inner;

            return html.Raw(controlGroup.ToString());
        }
    }
}