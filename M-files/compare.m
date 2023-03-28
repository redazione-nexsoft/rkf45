function compare(fundiff, fileName)
%Compara i risultati contenuti nel file <fileName>
%con quelli calcolati
% USO:
%    compare (<funzione diff.>, <nome file>)
%

   fid = fopen(fileName,'r');

   if (fid == -1)
       disp(['Impossibile aprire il file: ' fileName])       
   else
       A = fscanf(fid, '%e', 1);
       B = fscanf(fid, '%e', 1);
       Y0 = fscanf(fid, '%e', 1);
       TOL = fscanf(fid, '%e', 1);
       N = fscanf(fid, '%d', 1);
       for i=1:N
          X(i) = fscanf(fid, '%e', 1);
          Y(i) = fscanf(fid, '%e', 1);
       end
       [XC, YC] = ode45(fundiff, A, B, Y0, TOL, 0);
       hold on;
       plot(XC, YC, '-b');
       plot(X, Y, 'xg');
       title(['Grafico della funzione: ' fundiff]);
%       axis([A B min(YC) max(YC)]);
       hold off;
       fclose(fid);
   end
end.
