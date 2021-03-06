﻿using Notify.Data;
using Notify.Interfaces;
using Notify.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        public ObservableCollection<ItemTranslation> HistoryItems { get; private set; }

        public HistoryPage()
        {
            HistoryItems = new ObservableCollection<ItemTranslation>();

            InitializeComponent();

            BindingContext = this;
            Title = "History";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var historyDatabaseController = new HistoryDatabaseController();
            var historyResults = historyDatabaseController.GetLastTranslations();

            HistoryItems.Clear();
            foreach (var historyResult in historyResults)
            {
                HistoryItems.Add(historyResult);
            }
        }

        public async void lvItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as ItemTranslation;
            if (!string.IsNullOrEmpty(tapped.EnglishExample) && !string.IsNullOrEmpty(tapped.GermanExample))
            {
                DependencyService.Get<IMessage>().LongAlert($"{tapped.GermanExample}\n\n\n{tapped.EnglishExample}");
            }
        }
    }
}