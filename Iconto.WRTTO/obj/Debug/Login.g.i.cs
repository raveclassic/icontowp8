﻿#pragma checksum "C:\Users\Kirill\Documents\Visual Studio 2012\Projects\iConto\Iconto.WRTTO\Login.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "163A39013B2C275FD09FE167BBB0CA01"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Iconto.PCL.Common;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Iconto.WRTTO {
    
    
    public partial class Login : Iconto.PCL.Common.BasePage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.PhoneTextBox LoginTextBox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox PasswordTextBox;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Iconto.WRTTO;component/Login.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.LoginTextBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("LoginTextBox")));
            this.PasswordTextBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("PasswordTextBox")));
        }
    }
}

