﻿using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Xamarin.Forms.Core.UnitTests
{
	[TestFixture]
	public class BindableLayoutTests : BaseTestFixture
	{
		[Test]
		public void TracksEmpty()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>();
			BindableLayout.SetItemsSource(layout, itemsSource);

			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
		}

		[Test]
		public void TracksAdd()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>();
			BindableLayout.SetItemsSource(layout, itemsSource);

			itemsSource.Add(1);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
		}

		[Test]
		public void TracksInsert()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>();
			BindableLayout.SetItemsSource(layout, itemsSource);

			itemsSource.Insert(0, 1);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
		}

		[Test]
		public void TracksRemove()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>() { 0, 1 };
			BindableLayout.SetItemsSource(layout, itemsSource);

			itemsSource.RemoveAt(0);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));

			itemsSource.Remove(1);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
		}

		[Test]
		public void TracksReplace()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>() { 0, 1, 2 };
			BindableLayout.SetItemsSource(layout, itemsSource);

			itemsSource[0] = 3;
			itemsSource[1] = 4;
			itemsSource[2] = 5;
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
		}

		[Test]
		public void TracksMove()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>() { 0, 1 };
			BindableLayout.SetItemsSource(layout, itemsSource);

			itemsSource.Move(0, 1);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));

			itemsSource.Move(1, 0);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
		}

		[Test]
		public void TracksClear()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>() { 0, 1 };
			BindableLayout.SetItemsSource(layout, itemsSource);

			itemsSource.Clear();
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
		}

		[Test]
		public void TracksNull()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>(Enumerable.Range(0, 10));
			BindableLayout.SetItemsSource(layout, itemsSource);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));

			itemsSource = null;
			BindableLayout.SetItemsSource(layout, itemsSource);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
		}

		[Test]
		public void ItemTemplateIsSet()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>(Enumerable.Range(0, 10));
			BindableLayout.SetItemsSource(layout, itemsSource);

			BindableLayout.SetItemTemplate(layout, new DataTemplateBoxView());

			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
			Assert.AreEqual(itemsSource.Count, layout.Children.Cast<BoxView>().Count());
		}

		[Test]
		public void ItemTemplateSelectorIsSet()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>(Enumerable.Range(0, 10));
			BindableLayout.SetItemsSource(layout, itemsSource);
			BindableLayout.SetItemTemplateSelector(layout, new DataTemplateSelectorFrame());

			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
			Assert.AreEqual(itemsSource.Count, layout.Children.Cast<Frame>().Count());
		}

		[Test]
		public void ItemTemplateTakesPrecendenceOverItemTemplateSelector()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>(Enumerable.Range(0, 10));
			BindableLayout.SetItemsSource(layout, itemsSource);
			BindableLayout.SetItemTemplate(layout, new DataTemplateBoxView());
			BindableLayout.SetItemTemplateSelector(layout, new DataTemplateSelectorFrame());

			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
			Assert.AreEqual(itemsSource.Count, layout.Children.Cast<BoxView>().Count());
		}

		[Test]
		public void ItemsSourceTakePrecendenceOverLayoutChildren()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			layout.Children.Add(new Label());
			layout.Children.Add(new Label());
			layout.Children.Add(new Label());

			var itemsSource = new ObservableCollection<int>(Enumerable.Range(0, 10));
			BindableLayout.SetItemsSource(layout, itemsSource);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
		}

		[Test]
		public void LayoutIsGarbageCollectedAfterItsRemoved()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>(Enumerable.Range(0, 10));
			BindableLayout.SetItemsSource(layout, itemsSource);

			var pageRoot = new Grid();
			pageRoot.Children.Add(layout);
			var page = new ContentPage() { Content = pageRoot };

			var weakReference = new WeakReference(layout);
			pageRoot.Children.Remove(layout);
			layout = null;

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.IsFalse(weakReference.IsAlive);
		}

		[Test]
		public void ThrowsExceptionOnUsingDataTemplateSelectorForItemTemplate()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>(Enumerable.Range(0, 10));
			BindableLayout.SetItemsSource(layout, itemsSource);

			Assert.Throws(typeof(NotSupportedException), () => BindableLayout.SetItemTemplate(layout, new DataTemplateSelectorFrame()));
		}

		[Test]
		public void DontTrackAfterItemsSourceChanged()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>(Enumerable.Range(0, 10));
			BindableLayout.SetItemsSource(layout, itemsSource);
			BindableLayout.SetItemsSource(layout, new ObservableCollection<int>(Enumerable.Range(0, 10)));

			bool wasCalled = false;
			layout.ChildAdded += (_, __) => wasCalled = true;
			itemsSource.Add(11);
			Assert.IsFalse(wasCalled);
		}

		[Test]
		public void WorksWithNullItems()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int?>(Enumerable.Range(0, 10).Cast<int?>());
			itemsSource.Add(null);
			BindableLayout.SetItemsSource(layout, itemsSource);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
		}

		[Test]
		public void WorksWithDuplicateItems()
		{
			var layout = new StackLayout
			{
				IsPlatformEnabled = true,
				Platform = new UnitPlatform()
			};

			var itemsSource = new ObservableCollection<int>(Enumerable.Range(0, 10));
			foreach (int item in itemsSource.ToList())
			{
				itemsSource.Add(item);
			}

			BindableLayout.SetItemsSource(layout, itemsSource);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));

			itemsSource.Remove(0);
			Assert.IsTrue(IsLayoutWithItemsSource(itemsSource, layout));
		}

		// Checks if for every item in the items source there's a corresponding view
		static bool IsLayoutWithItemsSource(IEnumerable itemsSource, Layout layout)
		{
			if (itemsSource == null)
			{
				return layout.Children.Count() == 0;
			}

			int i = 0;
			foreach (object item in itemsSource)
			{
				if (BindableLayout.GetItemTemplate(layout) is DataTemplate dataTemplate ||
					BindableLayout.GetItemTemplateSelector(layout) is DataTemplateSelector dataTemplateSelector)
				{
					if (!Equals(item, layout.Children[i].BindingContext))
					{
						return false;
					}
				}
				else
				{
					if (!Equals(item?.ToString(), ((Label)layout.Children[i]).Text))
					{
						return false;
					}
				}

				++i;
			}

			return layout.Children.Count == i;
		}

		class DataTemplateBoxView : DataTemplate
		{
			public DataTemplateBoxView() : base(() => new BoxView())
			{
			}
		}

		class DataTemplateFrame : DataTemplate
		{
			public DataTemplateFrame() : base(() => new Frame())
			{
			}
		}

		class DataTemplateSelectorFrame : DataTemplateSelector
		{
			DataTemplateFrame dt = new DataTemplateFrame();

			protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
			{
				return dt;
			}
		}
	}
}
