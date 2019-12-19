using System;
using System.Collections.Generic;
using System.Text;


namespace CaptoApplication
{
    public static class RandomFunctionality
    {

        public static string WhatMeal(int hour)
        {
            string message = "";
            if (hour > 4 && hour < 10)
            {
                message = "Söker efter frukost...";
            }
            else if (hour > 10 && hour < 15)
            {
                message = "Söker efter lunch...";
            }
            else if (hour > 15 && hour < 22)
            {
                message = "Söker efter middag...";
            }
            else
            {
                message = "Söker efter kvällssnacks...";
            }

            return message;
        }
    }
}
