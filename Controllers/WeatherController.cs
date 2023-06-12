using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{   
    MySqlConnection con = new MySqlConnection(
        WebApplication.CreateBuilder().Configuration.GetConnectionString("SqlServer"));
    
    [HttpGet]
    public string Get()
    {
        List<string> nameList = new List<string>();
        
        con.Open();
        MySqlCommand cmd_Name = new MySqlCommand("select name from data where name IS NOT NULL and ns1_lang = 'en_US'", con);

        MySqlDataReader reader_Name = cmd_Name.ExecuteReader();
        while(reader_Name.Read())
        {
            nameList.Add(reader_Name.GetString("name"));
        }
        con.Close();
        
        return JsonConvert.SerializeObject(nameList);
    }
    
    [HttpGet("{timestamp}")]
    public string Get(string timestamp)
    {
        con.Open();
        MySqlCommand cmd_Timestamp = new MySqlCommand("select timestamp from data where tableid = 1;", con);
        MySqlDataReader reader_Timestamp = cmd_Timestamp.ExecuteReader();
        while(reader_Timestamp.Read())
        {
            timestamp = reader_Timestamp.GetString("timestamp");
        }

        return timestamp;
    }

    [HttpGet("{region}/{hour}")]
    public string Get(string region, int hour)
    {
        Weather tempWeather = new Weather();

        con.Open();
        MySqlCommand cmd_WeatherCode = new MySqlCommand (
            "select value from data where description = '" + region + 
            "' and description2 = 'Weather' and h = '" + hour.ToString() + "'", con);    
        MySqlDataReader reader_WeatherCode = cmd_WeatherCode.ExecuteReader();
        if(reader_WeatherCode.Read()) 
            tempWeather.WeatherCode = reader_WeatherCode.GetString("value");
        con.Close();

        con.Open();
        MySqlCommand cmd_Temperature = new MySqlCommand (
            "select value from data where description = '" + region + 
            "' and description2 = 'Temperature' and h = '" + hour.ToString() + 
            "' and unit = 'C'", con);        
        MySqlDataReader reader_Temperature = cmd_Temperature.ExecuteReader();
        if(reader_Temperature.Read()) 
            tempWeather.Temperature = reader_Temperature.GetString("value");
        con.Close();
        
        con.Open();
        MySqlCommand cmd_Humidity = new MySqlCommand (
            "select value from data where description = '" + region + 
            "' and description2 = 'Humidity' and h = '" + hour.ToString() + "'", con);
        MySqlDataReader reader_Humidity = cmd_Humidity.ExecuteReader();
        if(reader_Humidity.Read()) 
            tempWeather.Humidity = reader_Humidity.GetString("value");
        con.Close();

        con.Open();
        MySqlCommand cmd_WindSpeed = new MySqlCommand (
            "select value from data where description = '" + region + 
            "' and description2 = 'Wind speed' and h = '" + hour.ToString() + "'", con);
        MySqlDataReader reader_WindSpeed = cmd_WindSpeed.ExecuteReader();
        if(reader_WindSpeed.Read()) 
            tempWeather.WindSpeed = reader_WindSpeed.GetString("value");
        con.Close();

        con.Open();
        MySqlCommand cmd_WindDir = new MySqlCommand (
            "select value from data where description = '" + region + 
            "' and description2 = 'Wind direction' and h = '" + hour.ToString() + 
            "' and unit = 'CARD'", con);
        MySqlDataReader reader_WindDir = cmd_WindDir.ExecuteReader();
        if(reader_WindDir.Read()) 
            tempWeather.WindDir = reader_WindDir.GetString("value");
        con.Close();
            
        return JsonConvert.SerializeObject(tempWeather);
    }
}