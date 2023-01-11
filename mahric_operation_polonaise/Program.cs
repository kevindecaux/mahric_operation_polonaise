using System;
using System.Collections.Generic;

namespace mahric_operation_polonaise
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<string> list_operation;
            if (! ((list_operation = Saisie())== null))
            {
                Affichage(Resolution(list_operation));
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
        private static List<String> Saisie()
        {
            String saisie;
            List<String> list;


            Console.WriteLine("Entrée votre équation en séparant les membre par un espace");
           
            
            saisie= Console.ReadLine();

            list = String_to_List(saisie, ' ');
            if ( list == null || list.Count<3 )
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
            
            return list; 
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
        public static List<String> String_to_List(string entree, char separator)
        {
            List<string> list_operation = new List<string>();
            String block;
            int startIndex = 0;
            for (int i = 0; i < entree.Length; i++)
            {
                if (entree[i] == separator)
                {
                    block=entree.Substring(startIndex, i - startIndex);
                    if (Not_Unaire(block))
                    {
                        list_operation.Add(block);
                    }
                    else
                    {
                        return null;
                    }


                    startIndex = i + 1;
                }
            }
            list_operation.Add(entree.Substring(startIndex));
            return list_operation;
        }

        /// <summary>
        /// fonction vérifiant si des opérations unaires sont présentes 
        /// </summary>
        /// <param name="block"> chaine de caractère a vérifié</param>
        /// <returns> renvoie True s'il n'y a pas d'opération unaire
        /// False sinon 
        /// </returns>
        private static bool Not_Unaire(string block)
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
            if (carractere == '+' || carractere == '-' || carractere == '*' || carractere == '/')
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// Fonction qui appelle plusieurs méthodes pour résoudre l'opération et renvoie le résultat
        /// </summary>
        /// <param name="list_operation"> Liste des opérateur et opérande de l'opération</param>
        /// <returns> résultat de l'opération</returns>
        /// <exception cref="Exception"> Erreur en cas d'opération non faisable </exception>
        private static int Resolution(List<String> list_operation)
        {
            int index_first_operator;


            while (list_operation.Count > 1)
            {
                index_first_operator = Find_Index_First_Operator(list_operation);
                if (index_first_operator <2 || index_first_operator>=list_operation.Count)
                {
                    throw new Exception("Expression non solvable"); 
                }
                else
                {
                    
                    if ((list_operation = Calcul(list_operation, index_first_operator)) == null)
                    {
                        throw new Exception("Expression non solvable");
                        
                    }
                }
            }
            return int.Parse(list_operation[0]);
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
        private static List<String> Calcul(List<String> list_operation, int index_first_operator)
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
            list_operation[index_first_operator - 2] = result.ToString();
            list_operation.RemoveAt(index_first_operator - 1);
            list_operation.RemoveAt(index_first_operator - 1);
            return list_operation;
        }





        /// <summary>
        /// Fonction qui renvoie l'index du premier opérateur de l'opération
        /// </summary>
        /// <param name="list_operation"> liste des opérandes et des opérateurs </param>
        /// <returns> index du premier opérateur </returns>
        private static int Find_Index_First_Operator(List<String> list_operation)
        {
            int index = 0;
            while ( list_operation.Count > index && list_operation[index] != "+" && list_operation[index] != "-" && list_operation[index] != "*" && list_operation[index] != "/" )
            {
                index++;
                
            }
            return index;
        }

        /// <summary>
        /// Fonction qui affiche le résultat de l'opération
        /// </summary>
        /// <param name="result"> résultat de l'opération</param>
        private static void Affichage(int result)
        {
            
           
                Console.WriteLine("le résultat est : " + result);
            
        }

    }
}
