﻿#pragma checksum "..\..\..\Pages\GameStartingPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "05293AE883A829DCE6A46DD29AFA322AA197CEAE76AE515810E2783A6B598798"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WPFChessClient.Pages;


namespace WPFChessClient.Pages {
    
    
    /// <summary>
    /// GameStartingPage
    /// </summary>
    public partial class GameStartingPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\Pages\GameStartingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FirstPlayerNameTexBox;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\Pages\GameStartingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SecondPlayerNameTexBox;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\Pages\GameStartingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TimeTextBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WPFChessClient;component/pages/gamestartingpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\GameStartingPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.FirstPlayerNameTexBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 14 "..\..\..\Pages\GameStartingPage.xaml"
            this.FirstPlayerNameTexBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.FirstPlayerNameTexBox_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 14 "..\..\..\Pages\GameStartingPage.xaml"
            this.FirstPlayerNameTexBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.FirstPlayerNameTexBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SecondPlayerNameTexBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 16 "..\..\..\Pages\GameStartingPage.xaml"
            this.SecondPlayerNameTexBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.SecondPlayerNameTexBox_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 16 "..\..\..\Pages\GameStartingPage.xaml"
            this.SecondPlayerNameTexBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SecondPlayerNameTexBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TimeTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 18 "..\..\..\Pages\GameStartingPage.xaml"
            this.TimeTextBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.TimeTextBox_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 18 "..\..\..\Pages\GameStartingPage.xaml"
            this.TimeTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TimeTextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 20 "..\..\..\Pages\GameStartingPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

