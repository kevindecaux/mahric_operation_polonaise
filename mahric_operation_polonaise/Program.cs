using System;

namespace mahric_operation_polonaise
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            String[] tableau_operation;
            if (!((tableau_operation = Saisie()) == null))
            {
                Affichage(Resolution(tableau_operation));
            }




        }


        /// <summary>
        /// saisie de l'opération par l'utilisateur ainsi que transformation en liste, 
        /// si la liste est nulle ou inférieure à 3 opérateurs ou opérande redemande s'il veut recommencer la saisie
        /// </summary>
        /// <returns> 
        /// Renvoie la liste des opérateur et opérande 
        /// ou null si l'utilisateur annule la saisie
        /// </returns>
        private static String[] Saisie()
        {
            String saisie;
            String[] tableau_operation;


            Console.WriteLine("Entrée votre équation en séparant les membre par un espace");


            saisie = Console.ReadLine();

            tableau_operation = String_to_List(saisie, ' ');
            if (tableau_operation == null || tableau_operation.Length < 3)
            {
                Console.WriteLine("Saisie incorrecte. A pour relancer et une autre pour terminer");
                saisie = Console.ReadLine();

                if (saisie == "A")
                {
                    return Saisie();
                }
                else
                {
                    return null;
                }
            }
     
            return tableau_operation;
        }



        /// <summary>
        /// Fonction qui transcrit une chaine de caractère en liste d'opérateur et d'opérande en vérifiant qu'il n'y a pas d'opération unaire  
        /// </summary>
        /// <param name="entree"> chaine de caractère entrée par l'utilisateur </param>
        /// <param name="separator"> séparateur utilisé entre les opérateurs ainsi que les opérandes</param>
        /// <returns>
        /// renvoi la liste des opérateur et opérande  
        /// ou Null s'il y a une opération unaire
        /// </returns>
        public static String[] String_to_List(string entree, char separator)
        {
            if (entree == null)
            {
                throw new Exception("Opération vide");
            }
            String[] tableau_operation = new String[entree.Length];
            String block = "";
            int index_tableau = 0;
            int startIndex = 0;
            
            for (int i = 0; i < entree.Length; i++)
            {
                if (entree[i] == separator)
                {
                    block = recup_block(entree, startIndex, i );
                    if (Not_Unaire(block))
                    {
                        tableau_operation[index_tableau] = block;
                        index_tableau++;
                    }
                    else
                        return null;
                    startIndex = i + 1;
                }
            }
            tableau_operation[index_tableau] = recup_block(entree, startIndex, entree.Length);

            return copy_tableau(tableau_operation, index_tableau + 1);
        }

        /// <summary>
        /// fonction qui récupère les valeur entre deux index
        /// </summary>
        /// <param name="entree"> valeur a récupérer</param>
        /// <param name="index_start"> index de début</param>
        /// <param name="index_fin"> index de fin</param>
        /// <returns> chaine de caractère a stocker dans le tableau</returns>
        public static string recup_block(string entree, int index_start, int index_fin)
        {
            string block = "";
            for (int i = index_start; i < index_fin; i++)
            {
                block =block+ entree[i];
                
            }
            return block;
        }

        /// <summary>
        /// fonction vérifiant si des opérations unaires sont présentes 
        /// </summary>
        /// <param name="block"> chaine de caractère a vérifié</param>
        /// <returns> renvoie True s'il n'y a pas d'opération unaire
        /// False sinon 
        /// </returns>
        public static bool Not_Unaire(string block)
        {
            if (block.Length == 1 && Is_Operatore(block[0]))
            {
                return true;
            }
            else
            {
                for (int i = 0; i < block.Length; i++)
                {

                    if (Is_Operatore(block[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        /// <summary>
        /// Fonction qui renvoie vraie si le caractère est un opérateur
        /// </summary>
        /// <param name="carractere">Caractère a vérifié</param>
        /// <returns> Booléen True si opérateur
        /// False sinon opérateur </returns>
        private static bool Is_Operatore(char carractere)
        {
            return (carractere == '+' || carractere == '-' || carractere == '*' || carractere == '/');
        }



        /// <summary>
        /// Fonction qui appelle plusieurs méthodes pour résoudre l'opération et renvoie le résultat
        /// </summary>
        /// <param name="tableau_operation"> Liste des opérateur et opérande de l'opération</param>
        /// <returns> résultat de l'opération</returns>
        /// <exception cref="Exception"> Erreur en cas d'opération non faisable </exception>
        private static double Resolution(String[] tableau_operation)
        {
            int index_first_operator;


            while (tableau_operation.Length > 1)
            {
                index_first_operator = Find_Index_First_Operator(tableau_operation);
                if (index_first_operator < 2 || index_first_operator >= tableau_operation.Length)
                {
                    throw new Exception("Expression non solvable, operateur non trouver ou mal placer");
                }
                else
                {
                    if ((tableau_operation = Calcul(tableau_operation, index_first_operator)) == null)
                    {
                        throw new Exception("Expression non solvable, calcul impossible");

                    }
                }
            }
            return double.Parse(tableau_operation[0]);
        }

        /// <summary>
        /// fonction qui permet de calculer un des calculs de l'opération et renvoie la liste des opérateur et opérande modifié
        /// </summary>
        /// <param name="list_operation"> liste des opérandes et des opérateurs</param>
        /// <param name="index_first_operator"> Index du premier opérateur de l'opération</param>
        /// <returns> 
        /// renvoie l'opération modifiée si le calcul est faisable
        /// renvoie null si le calcul n'est pas faisable
        /// </returns>
        private static String[] Calcul(String[] list_operation, int index_first_operator)
        {
            double result = 0;
            try
            {
                switch (list_operation[index_first_operator])
                {
                    case "+":
                        result = double.Parse(list_operation[index_first_operator - 2]) + double.Parse(list_operation[index_first_operator - 1]);
                        break;
                    case "-":
                        result = double.Parse(list_operation[index_first_operator - 2]) - double.Parse(list_operation[index_first_operator - 1]);
                        break;
                    case "*":
                        result = double.Parse(list_operation[index_first_operator - 2]) * double.Parse(list_operation[index_first_operator - 1]);
                        break;
                    case "/":
                        if (double.Parse(list_operation[index_first_operator - 1])==0)
                        {
                            return null;
                        }
                        result = double.Parse(list_operation[index_first_operator - 2]) / double.Parse(list_operation[index_first_operator - 1]);
                        break;
                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            list_operation = decalage(list_operation, index_first_operator, result);
            return list_operation;
        }




        /// <summary>
        /// fonction qui permet de mettre le résultat du calcul dans le tableau et de suprimmé les opérateurs et opérandes en décalant le tableau a gauche.
        /// </summary>
        /// <param name="tableau_operation"> tableau des opérandes et opérateurs</param>
        /// <param name="index_first_operator"> index du premier opérateur</param>
        /// <param name="result"> valeur du résultat du calcul</param>
        /// <returns> tableau modifié pour contenir le résultat et suprimer les opérandes et l'opérateur du calcul</returns>
        private static String[] decalage(String[] tableau_operation, int index_first_operator, double result)
        {
            String[] copy_list_operation = copy_tableau(tableau_operation, tableau_operation.Length - 2);
            copy_list_operation[index_first_operator - 2] = result.ToString();
            for (int i = index_first_operator + 1; i < tableau_operation.Length; i++)
            {
                copy_list_operation[i - 2] = tableau_operation[i];
            }
            return copy_list_operation;
        }


        

        /// <summary>
        /// fonction qui permet de recopier un tableau dans un autre tableau de taille différente
        /// </summary>
        /// <param name="tableau"> tableau a copié </param>
        /// <param name="taille"> taille du nouveau tableau</param>
        /// <returns> tableau de la nouvelle taille contenant les valeur de l'ancien tableau</returns>
        private static String[] copy_tableau(String[] tableau, int taille)
        {
            String[] copy_tableau = new String[taille];
            for (int i = 0; i < taille; i++)
            {
                copy_tableau[i] = tableau[i];
            }
            return copy_tableau;
        }


        

        /// <summary>
        /// Fonction qui renvoie l'index du premier opérateur de l'opération
        /// </summary>
        /// <param name="list_operation"> liste des opérandes et des opérateurs </param>
        /// <returns> index du premier opérateur </returns>
        private static int Find_Index_First_Operator(String[] list_operation)
        {
            int index = 0;
            while ( list_operation.Length > index && list_operation[index] != "+" && list_operation[index] != "-" && list_operation[index] != "*" && list_operation[index] != "/" )
            {
                index++;
                
            }
            return index;
        }




        

        /// <summary>
        /// Fonction qui affiche le résultat de l'opération
        /// </summary>
        /// <param name="result"> résultat de l'opération</param>
        private static void Affichage(double result)
        {
            
           
                Console.WriteLine("le résultat est : " + result);
            
        }

    }
}
