﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Riverside.UniversalBrush.Models;
using System;
using Windows.UI.Xaml.Controls;

namespace Riverside.UniversalBrush.OuterNav
{
    public sealed partial class OuterNavPage : Page
    {
        public OuterNavPage(OuterNavViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
            _viewModel = viewModel;
            _viewModel.NavigateToItem += _viewModel_NavigateToItem;

            this.InitializeComponent();

            NavigateToViewModel(_viewModel.SelectedNavItem);
        }

        private readonly OuterNavViewModel _viewModel;
        public OuterNavViewModel ViewModel { get { return _viewModel; } }

        private void _viewModel_NavigateToItem(OuterNavViewModel source, INavItem navItem)
        {
            NavigateToViewModel(navItem);
        }

        private void NavigateToViewModel(object viewModel)
        {
            Type pageType = null;

            switch (viewModel)
            {
                case Riverside.UniversalBrush.ControlPalette.ControlPaletteViewModel controlPalette:
                    pageType = typeof(Riverside.UniversalBrush.ControlPalette.ControlPaletteView);
                    break;
            }

            if(pageType == null)
            {
                return;
            }

            NavigationFrame.Navigate(pageType, viewModel);
        }
    }
}
