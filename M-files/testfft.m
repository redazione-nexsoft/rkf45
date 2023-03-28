function [X] = testfft(inFile)
%Calcola l'FFT dei numeri complessi salvati nel file <inFile>
% USO:
%   F = testfft(<input file>)
%
   fid = fopen(inFile, 'r');

   if (fid == -1)
       disp(['Impossibile aprire il file: ' inFile])       
   else
       N = fscanf(fid, '%d', 1);
       for j=1:N
          Y(j) = fscanf(fid, '%e', 1) + i*fscanf(fid, '%e', 1);
       end

       Y = fft(Y);

       for j=1:N
          X(j,1) = Y(j);
       end

       fclose(fid);
   end
end.

