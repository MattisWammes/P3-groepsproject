﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EigenMaaltijd.Pages
{
    public class MealRepository : DBtools
    {
        //get meal information with bindpropperty and logged user information.

        //add a meal to the list


        public int AddMeal(Meal meal)
        {
            using IDbConnection _db = Connect();

            int rows = _db.Execute
                (
                "INSERT INTO maaltijden (UserID, Name, Ingredients, Portions, PortionSize, Ingevroren, Betalingsmethode, Img, Prijs) VALUES (@userID, @Name, @ingredients, @portions, @portionSize, @ingevroren, @betalingsmethode, @img, @prijs)",
                new { userID = meal.UserID, name = meal.Name, ingredients = meal.Ingredients, portions = meal.Portions, portionSize = meal.PortionSize, ingevroren = meal.Ingevroren, betalingsmethode = meal.Betalingsmethode, img = meal.Img, prijs = meal.Prijs  }
                );
            return 0;
        }


        public int DeleteMeal(int MealID)
        {
            using IDbConnection _db = Connect();
            int rows = _db.Execute
                (
                "DELETE FROM maaltijden WHERE MealID = @mealID",
                new { mealID = MealID }
                );
            return rows;
        }

        public int ChangeMeal(Meal Meal)
        {
            using IDbConnection _db = Connect();
            int rows = _db.Execute
                (
                "UPDATE maaltijden SET Name = @name, Ingredients = @ingredients, Portions = @portions, PortionSize = @portionSize WHERE MealID = @mealID ",
                new { 
                    mealID = Meal.MealID,
                    name = Meal.Name,
                    ingredients = Meal.Ingredients,
                    portions = Meal.Portions,
                    portionSize = Meal.PortionSize 
                });
            return rows;
        }


        public List<Meal> GetAllMeals()
        {
            using IDbConnection _db = Connect();
            List<Meal> returnList = _db.Query<Meal>
                ("SELECT * FROM maaltijden").ToList();
            return returnList;
        }


        public List<Meal> GetMealsFromUserID(int UserID)
        {
            using IDbConnection _db = Connect();
            List<Meal> returnList = _db.Query<Meal>
                ("SELECT * FROM maaltijden WHERE UserID = @userID",
                new {userID = UserID }).ToList();
            return returnList;
        }


        public Meal GetMealFromMealID(int MealID)
        {
            using IDbConnection _db = Connect();
            Meal returnList = _db.QuerySingle<Meal>
                ("SELECT * FROM maaltijden WHERE MealID = @mealID",
                new { mealID = MealID });
            return returnList;
        }




        public int AddFavourite(int MealID, int UserID)
        {
            using IDbConnection _db = Connect();

            int rows = _db.Execute
                (
                "INSERT INTO favourite (UserID, MealID) VALUES (@userID, @mealID))",
                new {userID = UserID, mealID = MealID}
                );
            return rows;
        }

        public int RemoveFavourite(int MealID, int UserID)
        {
            using IDbConnection _db = Connect();
            int rows = _db.Execute
                (
                "DELETE FROM favourite WHERE MealID = @mealID AND UserID = @userID",
                new {mealID = MealID, userID = UserID}
                );
            return rows;
        }

        public List<Meal> GetFavourites(int UserID)
        {
            using IDbConnection _db = Connect();
            List<Meal> returnList = null;
            List<int> favouriteIdList = _db.Query<int>
                ("SELECT MealID FROM favourite WHERE UserID = @userID",
                new {userID = UserID} ).ToList();
            foreach (int mealID in favouriteIdList)
            {
                Meal favouriteMeal = _db.QuerySingleOrDefault<Meal>
                ("SELECT * FROM maaltijden WHERE MealID = @MealID",
                new { MealID = mealID});
            }

            return returnList;
        }

        public int NewOrder(User user, Meal meal)
        {


            return 0;
        }

        public int RemoveOrder(User user, Meal meal)
        {


            return 0;
        }

        public int addRating(int mealID, int Rating, int userID)
        {

            using IDbConnection _db = Connect();

            int rows = _db.Execute
                (
                "INSERT INTO rating (MealID, UserID, Rating) VALUES (@mealID, @userID, @rating)",
                new { mealID = mealID, rating = Rating, userID = userID }
                );
            return rows;

        }
        public double GetAverageRating(int MealID)
        {

            using IDbConnection _db = Connect();

            double AvgRating = _db.QuerySingleOrDefault<double>
                (
                "SELECT IFNULL(avg(Rating), 0) from rating WHERE MealID = @mealID",
                new { mealID = MealID});
            return AvgRating;


        }

        public int ChangeRating(int mealID, int Rating)
        {

            using IDbConnection _db = Connect();

            int rows = _db.Execute
                (
                "UPDATE rating SET Rating = @rating WHERE mealID = @mealID",
                new { rating = Rating, mealID = mealID }
                );
            return rows;
        }

        public List<Meal> Search(string searchTerm)
        {
        
           List<Meal> _mealRepository = new MealRepository().GetAllMeals();
            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                return _mealRepository.Where(m => m.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            else
            {
                return _mealRepository;
            }


        }


    }
}
