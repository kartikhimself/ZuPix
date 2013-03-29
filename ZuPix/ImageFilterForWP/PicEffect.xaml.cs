﻿
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Devices;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using ZuPix.ImageFilter;

using System.Windows.Resources;
using ZuPix.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;

namespace ZuPix
{
	/// <summary>
	/// Main application page
	/// </summary>
	public partial class PicEffect :PhoneApplicationPage
	{
		WriteableBitmap original;
		WriteableBitmap resized;
		bool wasResized;
		Size oldViewportSize;
		CameraCaptureTask cameraCaptureTask;
		PhotoChooserTask photoChooserTask;
		//DateTime lastTouchUpdate;

        public PicEffect()
		{
			InitializeComponent();

			// Init vars
			oldViewportSize = new Size(Viewport.ActualWidth, Viewport.ActualHeight);
			wasResized = false;

			// Attach touch event handler
			Touch.FrameReported += Touch_FrameReported;

			// Init tasks
			cameraCaptureTask = new CameraCaptureTask();
			cameraCaptureTask.Completed += PhotoProviderTaskCompleted;
            photoChooserTask = new PhotoChooserTask();
			photoChooserTask.Completed += PhotoProviderTaskCompleted;

			// Disable the camera button if the app runs in the emulator
			// Todo: The BtnCamera reference returns null in WP7 v1
			// BtnCamera.IsEnabled = Microsoft.Devices.Environment.DeviceType != DeviceType.Emulator;
			// That's why we have to use this more hacky trick:
			var buttons = ApplicationBar.Buttons.Cast<Microsoft.Phone.Shell.ApplicationBarIconButton>();
			var btn = buttons.Where(b => b.IconUri.ToString().ToLower().Contains("camera")).FirstOrDefault();
			//if (btn != null)
			//{
				//btn.IsEnabled = Microsoft.Devices.Environment.DeviceType != DeviceType.Emulator;
			//}
            BackKeyPress += OnBackKeyPressed;

		}

		private void Initialize()
		{
			// Load and show the initial image
			if (original == null)
			{

               original = new WriteableBitmap(0, 0).FromResource("problem_troll.jpg");  
               ShowImage(original);
			}
		}

        private void ApplicationBarMenuItemAbout_Click(object sender, EventArgs e)
        {
            
            NavigationService.Navigate(new Uri("/About.xaml",UriKind.Relative));
            
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string selectedIndex;

            // check if navigation context contains selectedItem query string, if so get the value to obtain selected item index
            if (NavigationContext.QueryString.TryGetValue("SelectedItem", out selectedIndex))
            {
                int selIndex = int.Parse(selectedIndex);

                PicturesCategoryModel model = ((App)Application.Current).PicturesModel;

                if (original == null)
                {

                   Picture picture =  model.getPicture(selIndex);
                   System.IO.Stream stream = picture.GetImage();
                   var bmpi = new BitmapImage();
                   bmpi.SetSource(stream);
                   original = new WriteableBitmap(bmpi);
                   ShowImage(original);

                    
                }
            }
        }
		private void ApplySelectedEffectAndShowImageAsync(WriteableBitmap bitmap)
		{
			// Find selected effect
			IImageFilter effect = null;
			if (ListBoxEffects != null)
			{
				var item = ListBoxEffects.SelectedItem as EffectItem;
				if (item != null)
				{
					effect = item.Effect;
				}
			}

			// Apply selected effect
            if (effect == null)
            {
                // Present original
                ShowImage(bitmap);
            }
            else
            {
                var dispatcher = Dispatcher;
               

                    // Apply Effect on int[] since WriteableBitmap can't be used in background thread
                    var width = bitmap.PixelWidth;
                    var height = bitmap.PixelHeight;
                    var resultPixels = effect.process(new ZuPix.ImageFilter.CustomImage(bitmap));

                    // Present result
                    // WriteableBitmap ctor has to be invoked on the UI thread
                    resultPixels.copyPixelsFromBuffer();
                    dispatcher.BeginInvoke(() => ShowImage(resultPixels.destImage));
                
            }
		}

        private void pivotMain_LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            if (e.Item == PivotItemPic)
                ApplicationBar.IsVisible = true;
            else
                ApplicationBar.IsVisible = false;
        }

        private void ApplySelectedEffectAndSaveAsync()
        {
            if (ListBoxEffects.SelectedItem == null)
            {
                return;
            }

            // Set Save.. state and get UI parameters
            Viewport.Opacity = 0.2;
            ProgessBar.Visibility = Visibility.Visible;
            var effect = ((EffectItem)ListBoxEffects.SelectedItem).Effect;
            var dispatcher = Dispatcher;
           
                                                try
                                                {
                                                    // Apply Effect on int[] since WriteableBitmap can't be used in background thread
                                                    var width = original.PixelWidth;
                                                    var height = original.PixelHeight;
                                                    var resultPixels = effect.process(new ZuPix.ImageFilter.CustomImage(original));
                                                    // Convert int[] to WriteabelBitmap
                                                    // WriteableBitmap ctor has to be invoked on the UI thread
                                                    dispatcher.BeginInvoke(() =>
                                                                            {
                                                                                // Turbo copy the pixels to the WriteableBitmap
                                                                                resultPixels.copyPixelsFromBuffer();
                                                                                var result = resultPixels.destImage;
                                                                                // Save WriteableBitmap
                                                                                var name =
                                                                                    String.Format("ZuPix_{0:yyyy-MM-dd_hh-mm-ss-tt}.jpg",
                                                                                                  DateTime.Now);

                                                                                try
                                                                                {
                                                                                    result.SaveToMediaLibrary(name);
                                                                                }
                                                                                catch (InvalidOperationException)
                                                                                {
                                                                                    MessageBox.Show("Please remove your phone from Zune");
                                                                                }

                                                                            });
                                                }
                                                finally
                                                {
                                                    // Set controls to initial state
                                                    dispatcher.BeginInvoke(() =>
                                                                            {
                                                                                ProgessBar.Visibility = Visibility.Collapsed;
                                                                                MessageBox.Show("Picture Saved ");
                                                                                Viewport.Opacity = 1;
                                                                            });

                                                }
                                            
        }

		private void ResizeAndShowImage(WriteableBitmap bitmap)
		{
			resized = bitmap;

			// Fast and simple resize by using UIElement rendering
			if (Viewport != null)
			{
				Viewport.Source = bitmap;
				resized = new WriteableBitmap(Viewport, null);
			}

			// Apply selected effect and show image
			ApplySelectedEffectAndShowImageAsync(resized);

		}

       

		private void ShowImage(ImageSource result)
		{
			// Show image and scroll to start page if needed
			if (Viewport != null)
			{
				Viewport.Source = result;
                if (PivotCtrl != null && PivotCtrl.SelectedIndex != 0)
				{
					PivotCtrl.SelectedIndex = 0;
				}
			}
		}

		// TODO: Extract to TiltShift parameter Viewmodel
        //private void SetTiltShiftFocus(IList<TouchPoint> points)
        //{
        //    IImageFilter effect = null;
        //    if (ListBoxEffects != null)
        //    {
        //        var item = ListBoxEffects.SelectedItem as EffectItem;
        //        if (item != null)
        //        {
        //            effect = item.Effect;
        //        }
        //    }

        //    var tiltFx = effect as  TiltShiftEffect;
        //    if (tiltFx == null)
        //    {
        //        return;
        //    }
        //    var isManipulating = points.Any(p => p.Action == TouchAction.Down || p.Action == TouchAction.Move);
        //    if (isManipulating)
        //    {
        //        if (points.Count > 1 && (DateTime.Now - lastTouchUpdate).Milliseconds > 60)
        //        {
        //            var y1 = (int)points[0].Position.Y;
        //            var y2 = (int)points[1].Position.Y;

        //            // FallOff is expected as relative coordinate
        //            var ih = 1f / resized.PixelHeight;

        //            // Top most point is upper FallOff
        //            if (y1 < y2)
        //            {
        //                tiltFx.UpperFallOff = y1 * ih;
        //                tiltFx.LowerFallOff = y2 * ih;
        //            }
        //            else
        //            {
        //                tiltFx.UpperFallOff = y2 * ih;
        //                tiltFx.LowerFallOff = y1 * ih;
        //            }

        //            // Apply selected effect
        //            var processed = tiltFx.ProcessOnlyFocusFadeOff(resized);

        //            // Add FallOff marker lines
        //            const int markerHeight = 4;
        //            processed.FillRectangle(0, y1 - markerHeight, resized.PixelWidth, y1 + markerHeight, Colors.LightGray);
        //            processed.FillRectangle(0, y2 - markerHeight, resized.PixelWidth, y2 + markerHeight, Colors.LightGray);

        //            Viewport.Source = processed;
        //            lastTouchUpdate = DateTime.Now;
        //        }
        //    }
        //    else
        //    {
        //        // Apply selected effect
        //        Viewport.Source = tiltFx.ProcessOnlyFocusFadeOff(resized);
        //    }
        //}

		private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
		{
			Initialize();
            
		}

        void OnBackKeyPressed(object sender, System.ComponentModel.CancelEventArgs e)
        {

            e.Cancel = false;
            
        }


       

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (resized != null)
			{
				// Apply selected effect and show image
				ApplySelectedEffectAndShowImageAsync(resized);
			}
		}

		private void ApplicationBarIconFolderButton_Click(object sender, EventArgs e)
		{
			photoChooserTask.Show();
            if (ListBoxEffects != null &&
               ListBoxEffects.SelectedItem != null)
            {
                ListBoxEffects.SelectedItem = null;
               
            }
		}

		private void ApplicationBarIconCameraButton_Click(object sender, EventArgs e)
		{
			cameraCaptureTask.Show();
		}

		private void PhotoProviderTaskCompleted(object sender, PhotoResult e)
		{
			// Load the photo from the task result
			if (e != null && e.TaskResult == TaskResult.OK)
			{
                
				var bmpi = new BitmapImage();
				bmpi.SetSource(e.ChosenPhoto);
				original = new WriteableBitmap(bmpi);

				// Remove Image control to force a full Resize operation
				// Otherwise the LayoutUpdated event gets never called for the new size
				ViewportContainer.Children.Remove(Viewport);
                Viewport.Source = null;
                Viewport.LayoutUpdated -= Viewport_LayoutUpdated;
                Viewport = new Image { Source = original };
				Viewport.LayoutUpdated += Viewport_LayoutUpdated;
				ViewportContainer.Children.Add(Viewport);

				// Reset LayoutUpdated flags
				oldViewportSize = new Size(0, 0);
				wasResized = false;

				ShowImage(original);
			}
		}

		private void ApplicationBarIconSaveButton_Click(object sender, EventArgs e)
		{
			ApplySelectedEffectAndSaveAsync();
		}

        private void ApplicationBarIconRefreshButton_Click(object sender, EventArgs e)
        {
            if (ListBoxEffects != null &&
                ListBoxEffects.SelectedItem != null)
            {
                ListBoxEffects.SelectedItem = null;
                
            }
        }

		private void Viewport_LayoutUpdated(object sender, EventArgs e)
		{
			// Resize here only for the first time to get the real actual width and height
			if (Viewport != null && !wasResized && (Viewport.ActualWidth != oldViewportSize.Width || Viewport.ActualHeight != oldViewportSize.Height))
			{
				ResizeAndShowImage(original);
				oldViewportSize = new Size(Viewport.ActualWidth, Viewport.ActualHeight);
				wasResized = true;
			}
		}

		private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
		{
			//SetTiltShiftFocus(e.GetTouchPoints(Viewport));
		}
	}
}