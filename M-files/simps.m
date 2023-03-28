script
%calcola l'area della funzione myfun in [0,1]
   
   q = quad('myfun', 0, 1, 10^(-7));
   %disegna la funzione
   fplot('myfun',[0,1], '-b');
   %mostra l'area
   title(['Area = ',num2str(q, 12)]);
   grid
end
