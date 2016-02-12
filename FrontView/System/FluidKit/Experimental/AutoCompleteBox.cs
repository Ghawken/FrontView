using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace FluidKit.Experimental
{
	[TemplatePart(Name = SearchTextPartName, Type = typeof(TextBox))]
	[TemplatePart(Name = PopupPartName, Type = typeof(Popup))]
	[TemplatePart(Name = SearchResultsPartName, Type = typeof(ListBox))]
	public class AutoCompleteBox : Control
	{
		#region Fields

		private const string SearchTextPartName = "PART_SearchTextBox";
		private const string PopupPartName = "PART_Popup";
		private const string SearchResultsPartName = "PART_ResultsListBox";
		private bool _isNavigating;

		private TextBox _searchTextBox;
		private ListBox _resultsListBox;
		private ICollectionView _resultsView;
		private int _resultCounter;

		#endregion

		#region Properties

		public int MaxResults
		{
			get { return (int)GetValue(MaxResultsProperty); }
			set { SetValue(MaxResultsProperty, value); }
		}

		public object SelectedItem
		{
			get { return GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public Func<object, string, bool> FilterCallback
		{
			get;
			set;
		}

		public Func<object, string> SelectedValueCallback
		{
			get;
			set;
		}

		public ICollectionView ResultsItemsSource
		{
			get { return (ICollectionView)GetValue(ResultsItemsSourceProperty); }
			private set { SetValue(ResultsItemsSourceProperty, value); }
		}

		public Style ResultsListBoxStyle
		{
			get { return (Style)GetValue(ResultsListBoxStyleProperty); }
			set { SetValue(ResultsListBoxStyleProperty, value); }
		}

		public Style SearchTextBoxStyle
		{
			get { return (Style)GetValue(SearchTextBoxStyleProperty); }
			set { SetValue(SearchTextBoxStyleProperty, value); }
		}

		public bool IsResultsOpen
		{
			get { return (bool)GetValue(IsResultsOpenProperty); }
			set { SetValue(IsResultsOpenProperty, value); }
		}

		public IList ItemsSource
		{
			get { return (IList)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public bool HasResults
		{
			get { return (bool)GetValue(HasResultsProperty); }
			private set { SetValue(HasResultsProperty, value); }
		}

		#endregion

		public event EventHandler CommitRequested;

		#region Dependency Properties

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(AutoCompleteBox), new PropertyMetadata(""));

		public static readonly DependencyProperty MaxResultsProperty = DependencyProperty.Register(
			"MaxResults", typeof(int), typeof(AutoCompleteBox), new PropertyMetadata(10));

		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
			"SelectedItem", typeof(object), typeof(AutoCompleteBox));

		public static readonly DependencyProperty SearchTextBoxStyleProperty = DependencyProperty.Register(
			"SearchTextBoxStyle", typeof(Style), typeof(AutoCompleteBox));

		public static readonly DependencyProperty IsResultsOpenProperty = DependencyProperty.Register(
			"IsResultsOpen", typeof(bool), typeof(AutoCompleteBox));

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
			"ItemsSource", typeof(IList), typeof(AutoCompleteBox), new PropertyMetadata(OnItemsSourceChanged));

		public static readonly DependencyProperty ResultsItemsSourceProperty = DependencyProperty.Register(
			"ResultsItemsSource", typeof(ICollectionView), typeof(AutoCompleteBox));

		public static readonly DependencyProperty ResultsListBoxStyleProperty = DependencyProperty.Register(
			"ResultsListBoxStyle", typeof(Style), typeof(AutoCompleteBox));

		public static readonly DependencyProperty HasResultsProperty = DependencyProperty.Register(
			"HasResults", typeof(bool), typeof(AutoCompleteBox), new PropertyMetadata(true));

		#endregion

		static AutoCompleteBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteBox), new FrameworkPropertyMetadata(typeof(AutoCompleteBox)));
		}

		public AutoCompleteBox()
		{
			FilterCallback = (item, text) =>
			                 	{
			                 		var searchText = item.ToString().ToLower();
			                 		return searchText.Contains(text.ToLower());
			                 	};
			SelectedValueCallback = item =>
			                        	{
			                        		return item.ToString();
			                        	};
		}
		private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteBox box = (AutoCompleteBox)d;
			IEnumerable source = (IEnumerable)e.NewValue;
			if (source != null)
			{
				CollectionViewSource cvs = new CollectionViewSource();
				cvs.Source = source;
				cvs.View.Filter = box.FilterResults;

				box._resultsView = cvs.View;
				box.ResultsItemsSource = box._resultsView;
			}
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			_resultCounter = MaxResults;
		}

		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			if (e.OldFocus == _searchTextBox && !string.IsNullOrEmpty(_searchTextBox.Text))
			{
				if (SelectedValueCallback != null && _resultsListBox.SelectedItem != null)
				{
					_searchTextBox.Text = SelectedValueCallback(_resultsListBox.SelectedItem);
				}
			}
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			_isNavigating = false;

			switch (e.Key)
			{
				case Key.Escape:
				case Key.Tab:
					IsResultsOpen = false;
					break;

				case Key.Enter:
					// Commit action
					if (CommitRequested != null)
					{
						CommitRequested(this, EventArgs.Empty);
					}

					Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)(() => _searchTextBox.Focus()));
					IsResultsOpen = false;
					break;

				case Key.Up:
					if (_resultsView.IsEmpty) ApplyFilter();
					if (HasResults) IsResultsOpen = true;

					_isNavigating = true;
					MoveSelection(SelectionType.Previous);
					ChangeSearchText();
					break;

				case Key.Down:
					if (_resultsView.IsEmpty) ApplyFilter();
					if (HasResults) IsResultsOpen = true;

					_isNavigating = true;
					MoveSelection(SelectionType.Next);
					ChangeSearchText();
					break;

				default:
					_searchTextBox.Focus();
					break;
			}
		}

		private void ChangeSearchText()
		{
			if (SelectedValueCallback != null && SelectedItem != null)
			{
				Text = SelectedValueCallback(SelectedItem);
			}

		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			// Search TextBox
			_searchTextBox = GetTemplateChild(SearchTextPartName) as TextBox;
			if (_searchTextBox != null)
			{
				_searchTextBox.TextChanged += OnSearchTextChanged;
			}

			// ListBox
			_resultsListBox = GetTemplateChild(SearchResultsPartName) as ListBox;
			if (_resultsListBox != null)
			{
				_resultsListBox.Focusable = false;
				_resultsListBox.MouseDoubleClick += (s, args) =>
				                                    	{
				                                    		ChangeSearchText();
				                                    		IsResultsOpen = false;
				                                    	};
			}

		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			_searchTextBox.Focus();
		}

		private void MoveSelection(SelectionType direction)
		{
			switch (direction)
			{
				case SelectionType.Previous:
					if (!_resultsView.MoveCurrentToPrevious())
					{
						_resultsView.MoveCurrentToLast();
					}
					break;
				case SelectionType.Next:
					if (!_resultsView.MoveCurrentToNext())
					{
						_resultsView.MoveCurrentToFirst();
					}
					break;
				case SelectionType.First:
					_resultsView.MoveCurrentToFirst();
					break;
			}

			SelectedItem = _resultsListBox.SelectedItem;
		}

		private bool FilterResults(object obj)
		{
			if (FilterCallback != null && _searchTextBox != null)
			{
				bool found = FilterCallback(obj, Text);
				if (found)
				{
					if (_resultCounter == 0) return false;
					_resultCounter--;
				}
				return found;
			}

			return false;
		}

		private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
		{
			if (_isNavigating) return;
			ApplyFilter();

			IsResultsOpen = HasResults;
			if (HasResults)
			{
				MoveSelection(SelectionType.First);
			}
		}

		private void ApplyFilter()
		{
			SelectedItem = null;
			_resultCounter = MaxResults;
			_resultsView.Refresh();

			HasResults = !_resultsView.IsEmpty;
		}

		private enum SelectionType
		{
			Previous,
			Next,
			First
		}
	}
}