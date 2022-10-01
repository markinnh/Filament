using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Filament.WPF6.Helpers
{
    internal class DynamicDataTemplateSelector : DataTemplateSelector
    {


        //public TemplateCollection Templates
        //{
        //    get { return (TemplateCollection)GetValue(TemplatesProperty); }
        //    set { SetValue(TemplatesProperty, value); }
        //}
        public static TemplateCollection GetTemplates(UIElement element)
        {
            return (TemplateCollection)element.GetValue(TemplatesProperty);
        }
        public static void SetTemplates(UIElement element, TemplateCollection templates)
        {
            element.SetValue(TemplatesProperty, templates);
        }
        // Using a DependencyProperty as the backing store for Templates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemplatesProperty =
            DependencyProperty.Register("Templates", typeof(TemplateCollection), typeof(DynamicDataTemplateSelector), new PropertyMetadata(new TemplateCollection()));


        //public TemplateCollection? Templates { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container == null)
                return base.SelectTemplate(item, container);


            if (item != null && container is UIElement element)
            {
                TemplateCollection templates = GetTemplates(element);
                if (templates != null)
                    foreach (var template in templates)
                        if (template.Type.IsInstanceOfType(item))
                            return template.DataTemplate;

                return base.SelectTemplate(item, container);
            }
            else
                return base.SelectTemplate(item, container);
        }
    }
    public class TemplateCollection : List<Template> { }
    public class Template : DependencyObject
    {


        public Type Type
        {
            get { return (Type)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(Type), typeof(Template));



        public DataTemplate DataTemplate
        {
            get { return (DataTemplate)GetValue(DataTemplateProperty); }
            set { SetValue(DataTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataTemplateProperty =
            DependencyProperty.Register("DataTemplate", typeof(DataTemplate), typeof(Template));


    }
}
