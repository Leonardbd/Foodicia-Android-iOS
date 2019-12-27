using System;
using System.Collections.Generic;
using System.Text;


namespace CaptoApplication
{
    public static class RandomFunctionality
    {

        public static string WhatMeal(string meal)
        {
            string message = "";
            if(meal.Equals("Alla recept"))
            {
                message = "Söker efter recept...";
            }
            else if (meal.Equals("Frukost"))
            {
                message = "Söker efter frukost...";
            }
            else if (meal.Equals("Lunch"))
            {
                message = "Söker efter lunch...";
            }
            else if (meal.Equals("Middag"))
            {
                message = "Söker efter middag...";
            }
            else if (meal.Equals("Vegetariskt"))
            {
                message = "Söker efter vegetariskt...";
            }
            else if (meal.Equals("Veganskt"))
            {
                message = "Söker efter veganskt...";
            }

            return message;
        }
    }
}
