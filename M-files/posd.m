function [A] = POSMAT(N)
% genera una matrice casuale simmetrica a diagonale
% dominante

   A = zeros(N);
   for i = 1:N
      for j = 1:i-1
         A(i,j) = rand;         
	 A(j,i) = A(i, j);	
	 A(i,i) = A(i,i) + abs(A(i,j));	
%         A(j,j) = A(i,i) + abs(A(j,i));
      end,
   end,
   for i=1:N
      for j=i+1:N
         A(i,i) = A(i,i) + abs(A(i,j));
      end,
   end,
end   
   