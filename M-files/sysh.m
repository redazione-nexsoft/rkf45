function mu = sysh(N)
%calcola l'indice di condizionamento di una matrice di Hilbert
%di ordine N
   mu = norm(HILB(N))*norm(INVHILB(N));
   disp(mu)
end

   
