﻿using MarvelApp.Models;
using MarvelApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarvelApp.Views.Template
{
    [DesignTimeVisible(true)]
    public partial class FavoriteTemplateView : ContentView
    {
        public FavoriteTemplateView()
        {
            InitializeComponent();
        }
        protected override async void OnBindingContextChanged()
        {
            await OnBindingContextChangedAsync();
        }

        private async Task OnBindingContextChangedAsync()
        {
            base.OnBindingContextChanged();
            //Analytics.TrackEvent("CharacterTemplate");

            if (BindingContext is AppPage appPage)
                if (appPage.ViewModel is FavoriteViewModel viewModel)
                    await viewModel.LoadFavorites();
        }
    }
}