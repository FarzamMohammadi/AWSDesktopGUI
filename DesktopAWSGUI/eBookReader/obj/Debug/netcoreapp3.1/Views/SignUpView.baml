   M S B A M L   `   `   `  ����F  BeBookReader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:eBookReader.ViewseBookReader.Views  5      "    eBookReader.Views.SignUpView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:eBookReader.Views     5      .��5	      � 
�������.+�5
   
   �� $��Register Page��6   $��	0,0,0,392q�6(   $��30(�6;   $��Center=�6I   ��5      �� 5        �� EndPoint$  0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5      �  � Color$ Black��6#   � 5      $ 	#FFEE8181��6#    � Offset$ 1Q�65   5      5      �� 5   
   $��	0,392,0,0q�6   $��13(�6#   $
��Left=�61   ���	#FFEC2828
�������$��Italic$�6}   .��5      �� AWS integrated eBook Reader�� 65   !Programmed by: Farzam Mohammadi6a   5      �� 5   
   -   5        $��Sign Up��5      $��522,254,82,93q�6   $��20(�66   6D   �� 5   
   -   5         ��Name$ 	emailTBox��5      $
��Left=�6%   $��
142,80,0,0q�6@    �� TextWrapping$
 Wrap=�6T   $	��Top=�6h   $	��576��6�   $��53��6�   $��20(�6�   $
��Left=�6�   $��Center=�6�   $y�Visible=�6�   .��6�   ��5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      �� 6   6sys&clr-namespace:System;assembly=mscorlib     6    �� 
TargetType" ��6K    �� 	Resources 5         CueBannerBrush j� 
AlignmentX	 j� 
AlignmentY
 j� Stretch !� Visual��%�   &	        !� 5      $
 Left=�6=   $	 Center=�6O   $

 None=�6c    5      �� 5   "   $��example@gmail.com��6(   
�������$��20(�6V   5      5        �� Triggers 5!      P� 5"       P� Property � �� �� Empty P� Value# Z 62   . �5#      ��  �� Property � �� �� Value# [  6;   5$      P� 5%       � �� 62   Z� . �5&      ��  � ��# [  6;   5'      P� 5(       �@J�IsKeyboardFocused$
 True��6?   . �5)      ��  � ��
 �����6;   5*      5,      .��5-      5.      G� 5/   
   -   5         G�Name$ passwordTBox��5/      $
��Left=�6,   $��142,151,0,0q�6G   $	��Top=�6\   $	��576��6t   $��53��6�   $��20(�6�   $
��Left=�6�   $��Center=�6�   �� 50   
   $��Email:��6   $
��Left=�6!   $��	40,88,0,0q�6<   $	��Top=�6O   $��20(�6g   �� 51   
   $��	Password:��6   $
��Left=�6$   $��
40,159,0,0q�6?   $	��Top=�6S   $��20(�6k   �� 52   
   -   5         ��Name$ loginBtn��52      $��Log In��53      $��142,246,462,101q�54      $��20(�6&   64   55      56      