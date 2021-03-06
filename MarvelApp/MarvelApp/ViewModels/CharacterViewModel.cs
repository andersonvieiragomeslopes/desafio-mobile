﻿using Acr.UserDialogs;
using MarvelApp.Models;
using MarvelApp.Services;
using MarvelApp.Views.Template;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MarvelApp.ViewModels
{
    public class CharacterViewModel:BaseViewModel
    {
        #region Attributes
        private DataService dataService;
        private NavigationService navigationService;
        private ObservableRangeCollection<Result> _heroes;
        private ObservableRangeCollection<Item> _items;
        List<Result> heroes;
        private bool _Click = true;
        string _filter;
        private string _message;
        #endregion
        #region Properties
        public ObservableRangeCollection<Result> Heroes {
            get {
                return _heroes;
            }
            set {
                SetProperty(ref _heroes, value);
            }
        }
        public ObservableRangeCollection<Item> Items {
            get {
                return _items;
            }
            set {
                SetProperty(ref _items, value);
            }
        }
        public bool IsClick {
            get {
                return _Click;
            }
            set {
                SetProperty(ref _Click, value);
            }
        }
        public string Filter {
            get {
                return _filter;
            }
            set {
                SetProperty(ref _filter, value);
                Search(Filter);
            }
        }
        public string Message {
            get {
                return _message;
            }
            set {
                SetProperty(ref _message, value);
            }
        }
        #endregion
        #region Commands
        public Command<Result> GoToDetails { get; set; }

        #endregion
        #region Constructors
        public CharacterViewModel(DataService dataService, NavigationService navigationService)
        {
            Message = "CARREGANDO OS PERSONAGENS..";

            this.dataService = dataService;
            this.navigationService = navigationService;
            Items = new ObservableRangeCollection<Item>();
            heroes = new List<Result>();
            Heroes = new ObservableRangeCollection<Result>(heroes.OrderBy(p => p.Name));
            GoToDetails = new Command<Result>(async (heroe) => await GoToHeroesDetails(heroe));

            IsClick = true;
        } 
        #endregion    
        #region Methods
        private void Search(string filter)
        {
            if (filter.Length > 2 && filter != null)
            {
                Heroes = new ObservableRangeCollection<Result>(heroes
                        .Where(c => c.Name.ToLower().Contains(filter.ToLower()))
                        .OrderBy(c => c.Name));
                if (Heroes.Count == 0)
                    Message = "NENHUM PERSONAGEM FOI ENCONTRADO";
            }
            else if (filter.Length == 2 && filter != null)
            {
                UserDialogs.Instance.Toast(new ToastConfig("É necessário digitar mais que 2 caracteres!")
                    .SetBackgroundColor(System.Drawing.Color.DarkOrange)
                    .SetMessageTextColor(System.Drawing.Color.White)
                    .SetDuration(TimeSpan.FromSeconds(3))
                    .SetPosition(ToastPosition.Bottom)
                    );
            }
            else if (string.IsNullOrEmpty(filter))
            {
                Heroes = new ObservableRangeCollection<Result>(
                heroes.OrderBy(p => p.Name));

            }
        }

        private async Task GoToHeroesDetails(Result result)
        {
            //Trava duplo clique acidental :) 
            if (!IsClick)
            {
                IsClick = true;
                return;
            }
            IsClick = false;
            await navigationService.NavigateOnView("CharacterDetailView", result);

            // await App.Current.MainPage.Navigation.PushModalAsync(new CharacterDetailView(item), true);
            //libera caso ele volte a página :)
            IsClick = true;

        }
        public async Task InitializeAsync(string search = "")
        {
            try
            {
                UserDialogs.Instance.Loading("Estamos carregando os filmes, aguarde um instante..", null, null, true, MaskType.Gradient);

                var guid = Guid.NewGuid().ToString();
                var publickey = GetHash(guid + AppSettings.PrivateKey + AppSettings.PublicKey);
                var endpoint = $"characters?apikey={AppSettings.PublicKey}&hash={publickey}&ts={guid}&limit=10&offset=1";
                var response = await dataService.GetAsync<Character>(endpoint, "character", 100);
                var list = (List<Models.Result>)response.Result;
                if (list.Count > 0 && list != null)
                {
                    Heroes.AddRange(list);
                    foreach (var item in list)
                    {
                        heroes.Add(item);
                    }
                }
                else
                {
                    Heroes = new ObservableRangeCollection<Result>(heroes.OrderBy(p => p.Name));
                    Heroes.AddRange(Heroes);
                    Message = "NENHUM PERSONAGEM FOI CARREGADO";

                }
                UserDialogs.Instance.ShowLoading("Finalizando..", MaskType.Gradient);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception exception)
            {
                UserDialogs.Instance.ShowLoading("Finalizando..", MaskType.Gradient);
                UserDialogs.Instance.HideLoading();
                var properties = new Dictionary<string, string> { { "CharacterViewModel.cs", "InitializeAsync" } };
                //Crashes.TrackError(exception, properties);

                Heroes = new ObservableRangeCollection<Result>();
                Heroes.AddRange(Heroes);
            }

        }
        //Inspirado nessa implementação com adaptações https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        public string GetHash(string input)
        {
            using (var hash = MD5.Create())
            {
                byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                var strBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    strBuilder.Append(data[i].ToString("x2"));
                }

                return strBuilder.ToString();
            }
        } 
        #endregion
    }
}
