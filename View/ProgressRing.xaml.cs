using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;

namespace FreeApp.View
{
    public partial class ProgressRing : Control
    {
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressRing), new PropertyMetadata((object)false, new PropertyChangedCallback(ProgressRing.IsActiveChanged)));
        public static readonly DependencyProperty TemplateSettingsProperty = DependencyProperty.Register("TemplateSettings", typeof(ProgressRing.TemplateSettingValues), typeof(ProgressRing), new PropertyMetadata((object)new ProgressRing.TemplateSettingValues(100.0)));
        private bool hasAppliedTemplate;
        public bool IsActive
        {
            get
            {
                return (bool)this.GetValue(ProgressRing.IsActiveProperty);
            }
            set
            {
                this.SetValue(ProgressRing.IsActiveProperty, (object)(bool)(value ? true : false));
            }
        }
        public ProgressRing.TemplateSettingValues TemplateSettings
        {
            get
            {
                return (ProgressRing.TemplateSettingValues)this.GetValue(ProgressRing.TemplateSettingsProperty);
            }
            set
            {
                this.SetValue(ProgressRing.TemplateSettingsProperty, (object)value);
            }
        }
        public ProgressRing()
        {
            this.InitializeComponent();
            this.TemplateSettings = new ProgressRing.TemplateSettingValues(60.0);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.hasAppliedTemplate = true;
            this.UpdateState(this.IsActive);
        }

        private void UpdateState(bool isActive)
        {
            if (!this.hasAppliedTemplate)
                return;
            VisualStateManager.GoToState((Control)this, isActive ? "Active" : "Inactive", true);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double width = 100.0;
            if (!DesignerProperties.IsInDesignTool)
                width = this.Width != double.NaN ? this.Width : availableSize.Width;
            this.TemplateSettings = new ProgressRing.TemplateSettingValues(width);
            return base.MeasureOverride(availableSize);
        }

        private static void IsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((ProgressRing)d).UpdateState((bool)args.NewValue);
        }
        public class TemplateSettingValues : DependencyObject
        {
            public static readonly DependencyProperty MaxSideLengthProperty = DependencyProperty.Register("MaxSideLength", typeof(double), typeof(ProgressRing.TemplateSettingValues), new PropertyMetadata((object)0.0));
            public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(ProgressRing.TemplateSettingValues), new PropertyMetadata((object)0.0));
            public static readonly DependencyProperty EllipseOffsetProperty = DependencyProperty.Register("EllipseOffset", typeof(Thickness), typeof(ProgressRing.TemplateSettingValues), new PropertyMetadata((object)new Thickness()));

            public double MaxSideLength
            {
                get
                {
                    return (double)this.GetValue(ProgressRing.TemplateSettingValues.MaxSideLengthProperty);
                }
                set
                {
                    this.SetValue(ProgressRing.TemplateSettingValues.MaxSideLengthProperty, (object)value);
                }
            }

            public double EllipseDiameter
            {
                get
                {
                    return (double)this.GetValue(ProgressRing.TemplateSettingValues.EllipseDiameterProperty);
                }
                set
                {
                    this.SetValue(ProgressRing.TemplateSettingValues.EllipseDiameterProperty, (object)value);
                }
            }

            public Thickness EllipseOffset
            {
                get
                {
                    return (Thickness)this.GetValue(ProgressRing.TemplateSettingValues.EllipseOffsetProperty);
                }
                set
                {
                    this.SetValue(ProgressRing.TemplateSettingValues.EllipseOffsetProperty, (object)value);
                }
            }

            public TemplateSettingValues(double width)
            {
                this.MaxSideLength = 400.0;
                this.EllipseDiameter = width / 10.0;
                this.EllipseOffset = new Thickness(this.EllipseDiameter);
            }
        }
    }
}
