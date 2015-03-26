package ADReNA_API.Util;

import java.util.Random;

public class Randomizer {

	private Randomizer(){}
	
	private static Random rnd = new Random();

    /*
     * Retorna um valor double aleatório
     */
    public static double NextDouble()
    {
        return rnd.nextDouble();
    }

    /*
     * Retorna um valor double aleatório de 0 até determinado número
     */
    public static double NextDouble(int max)
    {
        return rnd.nextDouble() * max;
    }
}
