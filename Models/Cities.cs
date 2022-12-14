using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace MiniProject.Models
{
    public class cities
    {
        //public List<SelectListItem> department;

        [Required(ErrorMessage = "Please enter CityId")]
        public int CityId { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter name")]
        [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string CityName { get; set; }

        public static List<cities> AllCitie()
        {
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = MP; Integrated Security = True;";
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "allcitie";
                SqlDataReader dr = cmd.ExecuteReader();
                List<cities> lc = new List<cities>();
                cities ct;
                if (dr.HasRows == false) throw new Exception(" Empty");
                while (dr.Read())
                {
                    ct = new cities();
                    ct.CityId = dr.GetInt32("CityId");
                    ct.CityName = dr.GetString("CityName");

                    lc.Add(ct);
                }
                dr.Close();
                cn.Close();
                return lc;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void Delete(int CityId)
        {

            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = MP; Integrated Security = True;";
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "deletecity";
                cmd.Parameters.AddWithValue("@CityId", CityId);
                cmd.ExecuteNonQuery();
                cn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static cities GetByCityId(int CityId)
        {
            try
            {

                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = MP; Integrated Security = True;";
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "citydetail";
                cmd.Parameters.AddWithValue("@CityId", CityId);
                SqlDataReader dr = cmd.ExecuteReader();

                cities ct = new cities();
                ct = new cities();
                dr.Read();
                ct.CityId = dr.GetInt32("CityId");
                ct.CityName = dr.GetString("CityName");

                dr.Close();
                cn.Close();
                return ct;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static void UpdateCities(cities c)
        {
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = MP; Integrated Security = True;";
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "updatecities";
                cmd.Parameters.AddWithValue("@CityId", c.CityId);
                cmd.Parameters.AddWithValue("@CityName", c.CityName);

                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static void AddNewCity(cities c)
        {
            try
            {

                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = MP; Integrated Security = True;";
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insertcities";
                cmd.Parameters.AddWithValue("@CityId", c.CityId);
                cmd.Parameters.AddWithValue("@CityName", c.CityName);

                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        
    }
}
