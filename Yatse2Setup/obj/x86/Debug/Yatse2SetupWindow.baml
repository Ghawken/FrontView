   M S B A M L   `   `   `  ����a  ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes  5   �    Yatse2Setup#   Yatse2Setup.Yatse2SetupWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6F   oMicrosoft_Windows_ThemesJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero     6�   -   5          � Title$  Yatse2 Setup��5   	   $	��511��6   $	��679��6+   ���	#FF393939 � 
ResizeMode$ NoResize=�6N    � WindowStyle$
 None=�6d    � WindowStartupLocation$ CenterScreen=�6w   .��5      � ��5      �� 5       �� ImageSource$ Background.png��6   .+�5      �� $	��186��6   $��
87,38,60,0q�6   $	��Top=�6.   $��Logo.png��6F   �� 5      -   5         ��Name$ 
btn_Cancel��5         Localized_1#���   6"   $��Right=�6J   $��28��6f   $��0,0,8,8q�6r   $��Bottom=�6�   $	��105��6�   6�   �� 5      -   5         ��Name$ brd_Install��5      $��0q�6#   $��
8,228,8,49q�67   ���	#A5000000 �� CornerRadius$ 20��6b   ._�5      � $��0,0,0,-1q�6   .+�5   	   �� ���	#D8000000$��80,75,83,68q�6$   
���   � �� RadiusX$ 15��6H   	 �� RadiusY$	 15��6U   �� 5    	   $��21��6   $��
90,0,0,110q�6   $
��Wrap=�63     Localized_2#���  6G   $��Bottom=�6l   
�������$
��Left=�6�   $	��107��6�   
 �� TextAlignment$
 Right=�6�   �� 5!   	   -   5         ��Name$ txb_TargetDirectory��5!      $��201,101,207,111q�6.    �� TextWrapping$
 Wrap=�6G   $��	C:\Yatse2��6[   {�.  6l   �� 5"   	   -   5        $ 
btn_Browse��5"        Localized_3#���  6$   $��Right=�6L   $��0,101,111,111q�6h   $��81��6   6�   �� 5#   	   -   5        $ btn_Installx86��5#        Localized_4#���  6(   $��28��6P   $��
201,0,0,25q�6\   $��Bottom=�6p    J� RenderTransformOrigin$ 0.533,0.607&�6�   $
��Left=�6�   $	��105��6�   �� 5$   	   -   5        $ btn_Installx64��5$        Localized_5#���  6(   $��28��6P   $��
0,0,207,25q�6\   $��Bottom=�6p   $ 0.533,0.607&�6�   $��Right=�6�   $	��105��6�   {�.  6�   �� 5%   	   -   5         ��Name$  chk_Install_CreateShortcut��5%        Localized_16#���  66   $��15��6_   $��201,0,207,81q�6k   $��Bottom=�6�   
������� d� 	IsChecked$
 TrueY�6�   5&   	   5'      �� 5(      -	   5        $ 
brd_Update��5(      $��0q�6"   $��
8,228,8,49q�66   ���	#A5000000$ 20��6a   $y�Hidden=�6s   ._�5)      � $��0,0,0,-1q�6   .+�5*   	   �� ���	#D8000000$��80,75,83,84q�6$   
���   �$ 15��6H   $	 15��6U   �� 5+   	   $��21��6   $��
90,0,0,110q�6   $
��Wrap=�63   #���  6G   $��Bottom=�6l   
�������$
��Left=�6�   $	��107��6�   $
 Right=�6�   �� 5,   	   -
   5        $ txb_SourceDirectory��5,      $��201,101,207,111q�6.   $
 Wrap=�6G   $��	C:\Yatse2��6[   {�.  6l   �� 5-   	   -   5        $ btn_BrowseUpdate��5-      #���  6*   $��Right=�6R   $��0,101,111,111q�6n   $��81��6�   6�   �� 5.   	   -   5        $ 
btn_Update��5.        Localized_11#���  6$   $��28��6M   $��279,0,279,25q�6Y   $��Bottom=�6o   6�   5/   	   50      �� 51      -   5        $ brd_InstallInfos��51      $��0q�6(   $��
8,228,8,49q�6<   ���	#A5000000$ 20��6g   $y�Hidden=�6y   ._�52      � $��0,0,0,-1q�6   .+�53   	   �� ���	#D8000000$��80,21,83,90q�6$   
���   �$ 15��6H   $	 15��6U   �� 54   	   -   5         ��Name$ txb_InstallInfos��54      $��50��6-   $��99,0,99,148q�69   $
��Wrap=�6N   $��Bottom=�6b   
�������$
 Center=�6�   �� 55   	   -   5        $ 
btn_Launch��55        Localized_15#���  6$   $��28��6M   $��220,0,220,26q�6Y   $��Bottom=�6o   6�   � 56   	   -   5         �Name$ prg_progress��56      $��99,104,99,103q�6+    � IsIndeterminate .  � Maximum$ 10Q�6Y   57   	   58      �� 59      -   5        $ brd_BuildlInfos��59      $��0q�6'   $��
8,228,8,49q�6;   ���	#A5000000$ 20��6f   $y�Hidden=�6x   ._�5:      � $��0,0,0,-1q�6   .+�5;   	   �� ���	#D8000000$��	8,34,8,38q�6$   
���   �$ 15��6F   $	 15��6S   �� 5<   	   $��22��6   $��	21,8,18,0q�6   $
��Wrap=�62   $	��Top=�6F   
�������$��14.667(�6q   $
 Center=�6�     Localized_13#���  6�   �� 5=   	   -   5        $ btn_GoUpdate��5=      #���  6&   $��28��6O   $��279,0,279,6q�6[   $��Bottom=�6p   $ 0.533,0.607&�6�   6�   �� 5>   	   $��15,41,18,50q�6   $
��Auto=�6+   .��5?   
   �� -   5        $ txb_BuildInfos��5?      $
��Wrap=�6,   
�������$
 Justify=�6S   5@   
   5A   	   5B      �� 5C      -   5         ��Name$ cb_Language��5C      $
��Left=�6%   $��25��6@   $��8,0,0,8q�6L   $��Bottom=�6]   $	��174��6x   6�   �� 5D      -   5        $��Right=�5D      $	��100��6)   $��
Paypal.gif��65   $��Bottom=�6I   $��UniformToFill=�6d    �� StretchDirection$ DownOnly=�6|   $��30��6�   $��	0,0,293,8q�6�   5E      5F      