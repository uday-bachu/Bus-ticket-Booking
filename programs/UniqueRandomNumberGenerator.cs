using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Busticket.programs
{
   
    public class UniqueRandomNumberGenerator
    {
        
        private Random random = new Random();

        public int GenerateUniqueRandomNumber()
        {
             List<int> generatedNumbers = new List<int>();
        int randomNumber;
            do
            {

                randomNumber = random.Next(10000, 99999);
            } while (generatedNumbers.Contains(randomNumber));

            generatedNumbers.Add(randomNumber);
            return generatedNumbers[0];
        }
    }
}