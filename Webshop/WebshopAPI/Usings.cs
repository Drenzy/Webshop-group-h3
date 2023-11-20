global using Microsoft.EntityFrameworkCore;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using Microsoft.AspNetCore.Mvc;
global using WebshopAPI.Database;
global using WebshopAPI.Entities;
global using WebshopAPI.Repositories;
global using WebshopAPI.DTOs;
global using WebshopAPI.Authentication;
global using Microsoft.AspNetCore.Mvc.Filters;

// usings for JwtUtils
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.Extensions.Options;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;

global using Microsoft.OpenApi.Models; //Swagger authentication
global using System.Text.Json.Serialization; // ENUM stringidy