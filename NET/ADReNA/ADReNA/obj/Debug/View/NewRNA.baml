   M S B A M L   `   `   `  ����
  ADReNA    ADReNA.View.NewRNA   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Icon$. (/ADReNA;component/Images/transparent.ico��5   	    � Title$ Nova RNA��5   	   $	��345��6   $	��450��6'   .��5      � $��
15,15,15,5q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   $��0,0,0,10q�6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   .+�5      �� $��0��6   .��6%   	Modelo:6.   �� 5      -   5        $��cbModelo��5      $��1��6T   $	��350��6d   .@�5      �� .��6   Backpropagation60   �� 5      .��6   	Kohonen6(   5      5      � 5   
   -   5        $��BackpropagationGrid��5      $��1��6   $	��220��67   $y�Visible=�6D    5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5       $
��Auto �6    �� 5!      $
��Auto �6    �� 5"      $
��Auto �6    �� 5#      $
��Auto �6    �� 5$      $
��Auto �6     5&      �� 5'      $
��Auto �6#   �� 5(      $
��Auto �6#   .+�5+      �� $��0��6   $��0��6$   $��14(�61   $
��Bold"�6?   $��0,10,0,0q�6Q   $��2��6c   .��6w   Configuração obrigatória:6�   �� 5-      $��0��6   $��1��6$   .��61   0.Quantidade de neurônios na camada de entrada:6`   �� 5.      -   5        $&�� txtBackpropagationInputLayerSize��5.      $��1��6>   $��1��6N   $��80��6[   6�   �� 50      $��0��6   $��2��6$   .��61   /-Quantidade de neurônios na camada de saída:6^   �� 51      -   5        $��txtOutputLayerSize��51      $��1��60   $��2��6@   $��80��6M   6�   �� 53      $��0��6   $��3��6$   $��14(�61   $
��Bold"�6?   $��	00,10,0,0q�6Q   $��2��6d   .��6x   Configuração opcional:6�   �� 55      $��0��6   $��4��6$   .��61   97Quantidade de neurônios em cada camada intermediária:6h   �� 56      -   5        $��txtHiddenLayer��56      $��1��6,   $��4��6<   $��80��6I   6�   �� 57      $��0��6   $��6��6$   $��Right=�61   .��6M   0.Obs.: Camadas separadas por vírgula, exemplo:6}   �� 58      $��1��6   $��6��6&   $��80��63   {�.   �� 
IsReadOnly . $��10,5,10��6b   5;      � 5=   
   -   5        $��KohonenGrid��5=      $��1��6   $	��220��6/   $y�	Collapsed=�6<    5>      �� 5?      $
��Auto �6    �� 5@      $
��Auto �6    �� 5A      $
��Auto �6    �� 5B      $
��Auto �6    �� 5C      $
��Auto �6    �� 5D      $
��Auto �6     5F      �� 5G      $
��Auto �6#   �� 5H      $
��Auto �6#   .+�5K      �� $��0��6   $��0��6$   $��14(�61   $
��Bold"�6?   $��0,10,0,0q�6Q   $��2��6c   .��6w   Configuração obrigatória:6�   �� 5M      $��0��6   $��1��6$   .��61   0.Quantidade de neurônios na camada de entrada:6`   �� 5N      -   5        $��txtKohonenInputLayerSize��5N      $��1��66   $��1��6F   $��80��6S   6�   �� 5P      $��0��6   $��2��6$   .��61   64Quantidade N neurônios na camada competitiva (NxN):6f   �� 5Q      -   5        $��txtCompetitiveLayerSize��5Q      $��1��65   $��2��6E   $��80��6R   6�   �� 5S      $��0��6   $��3��6$   $��14(�61   $
��Bold"�6?   $��	00,10,0,0q�6Q   $��2��6d   .��6x   Configuração opcional:6�   �� 5U      $��0��6   $��4��6$   .��61   86Peso máximo gerado durante a inicialização da rede:6f   �� 5V      -	   5        $��txtMaximumWeightRange��5V      $��1��63   $��4��6C   $��80��6P   $	��100��6�   � 5X      $��0��6   $��2��6#   $��5��67   $	��395��6D   .+�5Y      �� ��5Z      �� 5[       �� TextWrapping$
 Wrap=�6%   $����Observação: Ao criar uma nova rede de Kohonen, os pesos de suas conexões sinápticas iniciais são valores aleatórios entre 0 e o valor informado no campo acima.��69   5]      5^      5_      � 5a   
   $��2��6    5b      �� 5c      $��* �6#   �� 5d      $
��Auto �6#   .+�5g      �� -
   5        $��0��5g      $	��100��6%   $��5q�61   $��Right=�6<   .��6p   
Cancelar6z   �� 5h      -   5        $��	btnCreate��5h      $��1��6&   $	��100��66   $��5q�6B   $��Right=�6M   {�.  .��6�   Criar6�   5i      5k      5l      