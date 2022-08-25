import java.util.*;

class soma {
public static Scanner sc = new Scanner(System.in);

public static void main (String args[]){
	int n1, n2, soma; //Declaração de variáveis
	System.out.println("Insira um número inteiro");
	n1 = sc.nextInt(); //Lê o primeiro número
	System.out.println("Insira um número inteiro");
	n2 = sc.nextInt(); //Lê o segundo número
	soma = n1 + n2; //Soma
	System.out.println("A soma é:" + soma); //Imprime o resultado
	}
}