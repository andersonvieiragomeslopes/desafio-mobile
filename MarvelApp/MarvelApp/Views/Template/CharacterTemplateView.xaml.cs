﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarvelApp.Views.Template
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterTemplateView : ContentView
    {
        public CharacterTemplateView()
        {
            InitializeComponent();
        }
    }
}