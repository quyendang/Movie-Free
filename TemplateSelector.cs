using FreeApp.ViewModels;
using Microsoft.Phone.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FreeApp
{
    public abstract class TemplateSelector : ContentControl
    {
        public abstract DataTemplate SelectTemplate(object item, DependencyObject container);

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }
    public class ContactTemplateSelector : TemplateSelector
    {
        public DataTemplate ImageLeft
        {
            get;
            set;
        }

        public DataTemplate ImageRight
        {
            get;
            set;
        }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var i = item as ChatItem;
            if (i.MessageText.Contains(DeviceStatus.DeviceName.ToString()))
            {
                return ImageRight;
            }
            else
            {
                return ImageLeft;
            }
        }
    }
}
