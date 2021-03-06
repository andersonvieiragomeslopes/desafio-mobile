﻿using System.Threading.Tasks;
using MarvelApp.Models;
using MarvelApp.Views;
using MarvelApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(NavigationService))]
namespace MarvelApp.Services
{


    public sealed class NavigationService
    {
        public void SetMainView(string pageName)
        {
            switch (pageName)
            {
                case "HomeView":
                    Application.Current.MainPage = new NavigationPage(new HomeView());
                    break;
                case "SplashScreenView":
                    Application.Current.MainPage = new NavigationPage(new SplashScreenView());
                    break;
                case "NoInternetView":
                    Application.Current.MainPage = new NavigationPage(new NoInternetView());
                    break;
            }
        }
        public async Task NavigateOnView(string pageName, Result result)
        {
            switch (pageName)
            {
                case "CharacterDetailView":
                     await Application.Current.MainPage.Navigation.PushModalAsync(new CharacterDetailView(result), true);
                    break;
            }
        }
        public async Task NavigateBackView()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
