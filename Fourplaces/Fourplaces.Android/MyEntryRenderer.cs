using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fourplaces.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Fourplaces;
using Android.Content.Res;
using Android.Graphics;

[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRenderer))]
namespace Fourplaces.Android
{
    class MyEntryRenderer : EntryRenderer
    {
        public MyEntryRenderer(Context context) : base(context)
        {
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                

                Control.CompoundDrawableTintList = ColorStateList.ValueOf(global::Android.Graphics.Color.LightGray);

                Control.BackgroundTintList = ColorStateList.ValueOf(global::Android.Graphics.Color.LightGray);


            }
        }
    }
}