   M S B A M L   `   `   `  ����I  EWPFChessClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:WPFChessClient.PagesWPFChessClient.Pages  5      '    !WPFChessClient.Pages.GamePlayPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:WPFChessClient.Pages     5        N� Title$  GamePlayPage��5	      .��5      �  � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��2* �6   �� 5      $��1* �6    � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $��3* �6   �� 5      $��1* �6   .+�5   
   �� -   5        $��
GameCanvas��5      }�. $��1��5    F   $��3��6V   �� 5"   
   $��Center=�6   $
��Left=�60   $��2��6K   $��1��6[   .+�5#      �� -   5        $��TextBlockTimerSecond��5#       �� Padding$ 15q�64   $��
20 10 0 10q�6A   $��27(�6U   6e   �� 5$      -   5        $��TextBlockTimerFirst��5$      $ 15q�63   $��
20 10 0 10q�6@   $��27(�6T   6d   5%      �� 5(   
   -   5        $��TextBlockFirstPlayerName��5(      $��30(�64   $��2��6B   $��2��6R   $	��Top=�6_   $��	20 0 0 10q�6w   6�   �� 5)   
   -   5        $��TextBlockSecondPlayerName��5)      $��30(�65   $��2��6C   $��0��6S   $��Bottom=�6`   $��	20 10 0 0q�6{   6�   �� 5+   
   -   5        $��SecondPlayerSurrender��5+      $��20(�6.   $��2��6<   $��0��6L   $	��Top=�6Y   $��
10 10 10 0q�6q   .��6�   Surrender Black6�   �� 5,   
   -   5        $��FirstPlayerSurrender��5,      $��20(�6-   $��2��6;   $��2��6K   $��Bottom=�6X   $��
10 0 10 10q�6s   .��6�   Surrender White6�   �� 5/   
   $��0��6   $��1��6%   .+�50      �� -   5        $��TextBlockGameResult��50      $ 15q�63   $��60(�6A   $��Center=�6O   6n   �� 51      -	   5        $��TextBlockWinner��51      $ 15q�6/   $��26(�6=   $��Center=�6K   6k   52      �� 54   
   -
   5        $
��Exit��54      $��3��6   $��15q�6*   $	��160��67   $��50��6C   .��6b   Quit to main menu6w   �� 56   
   -   5        $��SaveUnendedGame��56      $��15q�6F   $	��160��6S   $��50��6_   .��6k   Save6q   5;      5<      